using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.UnitTestFramework;
using NBehave.ReSharper.Plugin.UnitTestRunner;

namespace NBehave.ReSharper.Plugin.UnitTestProvider
{
    public class NBehaveStepTestElement : NBehaveUnitTestElementBase
    {
        private readonly string _step;

        private readonly string _identity;

        public NBehaveStepTestElement(string step, IProjectFile featureFile, IUnitTestProvider testProvider, ProjectModelElementEnvoy projectModel, NBehaveUnitTestElementBase parent)
            : base(featureFile, testProvider, parent.Id + "/" + step, projectModel, parent)
        {
            _step = step;
            _identity = IdentityGenerator.NextValue().ToString().PadLeft(9, '0');
        }

        public override string ShortName
        {
            get { return _identity + ": " + Step; }
        }

        public override string Kind
        {
            get { return "NBehave step"; }
        }

        public string Step
        {
            get { return _step; }
        }

        public override string GetPresentation(IUnitTestElement parent = null)
        {
            return Step;
        }

        public override IList<UnitTestTask> GetTaskSequence(IList<IUnitTestElement> explicitElements)
        {
            var taskSequence = (Parent != null) ? Parent.GetTaskSequence(explicitElements, null) : new List<UnitTestTask>();
            string scenario = (Parent is NBehaveBackgroundTestElement) ? ((NBehaveBackgroundTestElement)Parent).Scenario : "";
            scenario = (Parent is NBehaveScenarioTestElement) ? ((NBehaveScenarioTestElement)Parent).Scenario : scenario;
            taskSequence.Add(new UnitTestTask(this, new NBehaveStepTask(FeatureFile, scenario, _step)));
            return taskSequence;
        }

        public override UnitTestElementDisposition GetDisposition()
        {
            //Denna metod anropas om man tex trycker p� enter p� en nod.
            return null;
        }

        public override IDeclaredElement GetDeclaredElement()
        {
            return null;
        }

        public bool Equals(NBehaveStepTestElement other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other._step, _step);
        }

        public override bool Equals(IUnitTestElement other)
        {
            return Equals(other as NBehaveStepTestElement);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NBehaveStepTestElement);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ _step.GetHashCode();
            }
        }
    }
}