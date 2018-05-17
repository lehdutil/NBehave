using NBehave.EventListeners;
using NBehave.Extensions;
using NBehave.Specifications.Features;
using NUnit.Framework;

namespace NBehave.Specifications.EventListeners
{
    [TestFixture]
    public class FailSpecResultEventListenerSpec
    {
        [Test]
        public void Should_not_throw_if_feature_passes()
        {
            var listener = new FailSpecResultEventListener();
            var runner = ConfigurationNoAppDomain
                .New
                .SetScenarioFiles(new[] { TestFeatures.FeatureNamedStory })
                .SetAssemblies(new[] { "TestLib.dll" })
                .SetEventListener(listener)
                .Build(); Assert.DoesNotThrow(() => runner.Run());
        }

        [Test]
        public void Should_throw_TestFailedException()
        {
            var listener = new FailSpecResultEventListener();
            var runner = ConfigurationNoAppDomain
                .New
                .SetScenarioFiles(new[] { TestFeatures.FeatureWithFailingStep })
                .SetAssemblies(new[] { "TestLib.dll" })
                .SetEventListener(listener)
                .Build();
            Assert.Throws<StepFailedException>(() => runner.Run());
        }
    }
}