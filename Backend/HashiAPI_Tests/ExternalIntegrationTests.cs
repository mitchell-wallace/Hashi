using External;

namespace HashiAPI_Tests
{
    internal class ExternalIntegrationTests
        // these can be removed if these tests aren't needed
        // test for valid connections to Jira, Wrike, and database
    {

        [Test]
        public void JiraConnection_DoesNotFailImmediately()
        {
            Assert.That(JiraSession.GetInstance().Connect());
        }

        [Test]
        public void WrikeConnection_DoesNotFailImmediately()
        {
            Assert.That(WrikeSession.GetInstance().Connect());
        }
    }
}
