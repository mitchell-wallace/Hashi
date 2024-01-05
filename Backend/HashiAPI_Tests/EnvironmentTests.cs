using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashiAPI_Tests
{
    internal class EnvironmentTests
        // these can be removed if these tests aren't needed
        // test for presence of environment variables needed for connecting to Jira, Wrike, and database
    {

        [Test]
        public void JiraUrl_InEnvironmentVariables()
        {
            Assert.That(Environment.GetEnvironmentVariable(
                    "HASHI_JiraUrl"), Is.Not.Null);
        }

        [Test]
        public void JiraUsername_InEnvironmentVariables()
        {
            Assert.That(Environment.GetEnvironmentVariable(
                    "HASHI_JiraUsername"), Is.Not.Null);
        }

        [Test]
        public void JiraSecret_InEnvironmentVariables()
        {
            Assert.That(Environment.GetEnvironmentVariable(
                    "HASHI_JiraSecret"), Is.Not.Null);
        }

        [Test]
        public void WrikeUrl_InEnvironmentVariables()
        {
            Assert.That(Environment.GetEnvironmentVariable(
                    "HASHI_WrikeUrl"), Is.Not.Null);
        }

        [Test]
        public void WrikeToken_InEnvironmentVariables()
        {
            Assert.That(Environment.GetEnvironmentVariable(
                    "HASHI_WrikeToken"), Is.Not.Null);
        }

        [Test]
        public void ConnectionString_InEnvironmentVariables()
        {
            Assert.That(Environment.GetEnvironmentVariable(
                    "SQLServerDBConnectionString"), Is.Not.Null);
        }
    }
}
