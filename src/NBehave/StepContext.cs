using NBehave.Internal;

namespace NBehave
{
    public static class Step
    {
        public static void Pend(string reason)
        {
            StepContext.Current.Pend(reason);
        }
    }

    public class StepContext : NBehaveContext
    {
        public FeatureContext FeatureContext { get; private set; }
        public ScenarioContext ScenarioContext { get; private set; }
        public string Step { get { return StringStep.Step; } }

        internal StringStep StringStep { get; set; }

        public StepContext(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            FeatureContext = featureContext;
            ScenarioContext = scenarioContext;
        }

        public static StepContext Current
        {
            get { return TinyIoC.TinyIoCContainer.Current.Resolve<StepContext>(); }
        }

        public override string ToString()
        {
            return Step;
        }

        public void Pend(string reason)
        {
            StringStep.Pend(reason);
        }
    }
}
