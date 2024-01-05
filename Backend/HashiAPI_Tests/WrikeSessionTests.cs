using External;
using HashiAPI_1.Models;
using System.Text.Json;

namespace HashiAPI_Tests
{
    public class WrikeSessionTests
    {

        [Test]
        public void WrikeJsonDeserialiseUsers_MatchExpectedResult()
        {
            // Arrange
            string usersRaw, usersProcessed;

            using (StreamReader r = new StreamReader("SampleJSON\\Wrike-Users-raw.json"))
            {
                usersRaw = r.ReadToEnd();
            }

            using (StreamReader r = new StreamReader("SampleJSON\\Wrike-Users-processed-by-hashi.json"))
            {
                usersProcessed = r.ReadToEnd();
            }


            // Act
            string usersActual = JsonSerializer.Serialize(
                WrikeSession.DeserialiseUsers(usersRaw));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(usersProcessed),
                Is.EqualTo(
                    Helpers.JsonCleanup(usersActual)
                    ));
        }

        [Test]
        public void WrikeJsonDeserialiseProjects_MatchExpectedResult()
        {
            string projectsRaw, projectsProcessed;

            // Arrange
            using (StreamReader r = new StreamReader("SampleJSON\\Wrike-Projects-raw.json"))
            {
                projectsRaw = r.ReadToEnd();
            }

            using (StreamReader r = new StreamReader("SampleJSON\\Wrike-Projects-processed-by-hashi.json"))
            {
                projectsProcessed = r.ReadToEnd();
            }

            // Act
            string projectsActual = JsonSerializer.Serialize(
                WrikeSession.DeserialiseProjects(projectsRaw));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(projectsProcessed),
                Is.EqualTo(
                    Helpers.JsonCleanup(projectsActual)
                    ));
        }
    }
}
