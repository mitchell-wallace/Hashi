using External;
using System.Text.Json;

namespace HashiAPI_Tests
{
    public class JiraSessionTests
    {


        [Test]
        public void JiraJsonDeserialiseUsers_MatchExpectedResult()
        {
            // Arrange
            string usersRaw, usersProcessed;

            using (StreamReader r = new StreamReader("SampleJSON\\Jira-Users-raw.json"))
            {
                usersRaw = r.ReadToEnd();
            }

            using (StreamReader r = new StreamReader("SampleJSON\\Jira-Users-processed-by-hashi.json"))
            {
                usersProcessed = r.ReadToEnd();
            }

            // Act
            string usersActual = JsonSerializer.Serialize(
                JiraSession.DeserialiseUsers(usersRaw));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(usersProcessed),
                Is.EqualTo(
                    Helpers.JsonCleanup(usersActual)
                    ));

            //Console.WriteLine($"UsersProcessed:\n{usersProcessed}\n\nUsersActual:\n{usersActual}");
        }

        [Test]
        public void JiraJsonDeserialiseProjects_MatchExpectedResult()
        {
            string projectsRaw, projectsProcessed;

            // Arrange
            using (StreamReader r = new StreamReader("SampleJSON\\Jira-Projects-raw.json"))
            {
                projectsRaw = r.ReadToEnd();
            }

            using (StreamReader r = new StreamReader("SampleJSON\\Jira-Projects-processed-by-hashi.json"))
            {
                projectsProcessed = r.ReadToEnd();
            }

            // Act
            string projectsActual = JsonSerializer.Serialize(
                JiraSession.DeserialiseProjects(projectsRaw));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(projectsProcessed), 
                Is.EqualTo(
                    Helpers.JsonCleanup(projectsActual)
                    ));
        }
    }
}
