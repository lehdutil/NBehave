using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.UnitTestFramework;

namespace NBehave.ReSharper.Plugin.UnitTestProvider
{
    public abstract partial class NBehaveUnitTestElementBase : IUnitTestElement
    {
        private readonly IUnitTestProvider _testProvider;
        private readonly string _id;
        private readonly ProjectModelElementEnvoy _projectModel;
        private readonly IList<IUnitTestElement> _children = new List<IUnitTestElement>();
        private NBehaveUnitTestElementBase _parent;
        private readonly IEnumerable<UnitTestElementCategory> _categories = new List<UnitTestElementCategory>(UnitTestElementCategory.Uncategorized);
        private IProject _project;

        public IProjectFile FeatureFile { get; private set; }

        protected NBehaveUnitTestElementBase(IProjectFile featureFile, IUnitTestProvider testProvider, string id, ProjectModelElementEnvoy pointer, NBehaveUnitTestElementBase parent)
        {
            FeatureFile = featureFile;
            ReadLockCookie.Execute(() =>
            {
                _project = featureFile.GetProject();
            });

            _testProvider = testProvider;
            _id = id;
            _projectModel = pointer;
            Parent = parent;
        }

        public string Id
        {
            get { return _id; }
        }

        public abstract string ShortName { get; }

        public UnitTestElementState State { get; set; }

        public IUnitTestProvider Provider
        {
            get { return _testProvider; }
        }

        public IUnitTestElement Parent
        {
            get { return _parent; }
            set
            {
                if (value == _parent)
                    return;
                if (_parent != null)
                {
                    _parent.RemoveChild(this);
                }

                _parent = (NBehaveUnitTestElementBase)value;
                if (_parent != null)
                    _parent.AppendChild(this);
            }
        }

        public ICollection<IUnitTestElement> Children
        {
            get { return _children; }
        }

        public string ExplicitReason { get; set; }

        public bool Explicit
        {
            get { return ExplicitReason != null; }
        }

        public abstract string Kind { get; }

        public IEnumerable<UnitTestElementCategory> Categories
        {
            get { return _categories; }
        }

        public abstract IList<UnitTestTask> GetTaskSequence(IList<IUnitTestElement> explicitElements);

        public virtual IEnumerable<IProjectFile> GetProjectFiles()
        {
            IProject project = GetProject();
            var files = project.GetAllProjectFiles().Where(_ => _.Location.FullPath == FeatureFile.Location.FullPath);
            return files.ToList();
        }

        public IProject GetProject()
        {
            return _projectModel.GetValidProjectElement() as IProject;
        }

        public abstract string GetPresentation(IUnitTestElement parent = null);

        public UnitTestNamespace GetNamespace()
        {
            //Features dont have namespaces
            return new UnitTestNamespace(Id);
        }

        public abstract UnitTestElementDisposition GetDisposition();

        public abstract IDeclaredElement GetDeclaredElement();

        private void AppendChild(IUnitTestElement element)
        {
            _children.Add(element);
        }

        private void RemoveChild(IUnitTestElement element)
        {
            _children.Add(element);
        }

        public override string ToString()
        {
            return Id;
        }

        public abstract bool Equals(IUnitTestElement other);

        protected bool Equals(NBehaveUnitTestElementBase other)
        {
            if (other == null)
                return false;
            return Equals(other._id, _id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NBehaveUnitTestElementBase);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}