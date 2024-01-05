using HashiAPI_1.Controllers;
using HashiAPI_1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace HashiAPI_Tests
{
    public class DatabaseTestsForProjects
    {
        [Test]
        public void ProjectObject_TestJSONIsValid()
        // this test validates that our sample JSON is valid
        {
            // Arrange

            // Act
            string projectsActual = JsonSerializer.Serialize(
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<Project>>(projectsSource));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(projectsActual),
                Is.Not.Null);
        }

        [Test]
        public void ProjectObject_IsConsistentWithSampleJSON()
        // this test validates that our sample JSON is consistent with our Projects object
        {
            // Arrange
            string expectedResult = projectsSource;

            // Act
            string projectsActual = JsonSerializer.Serialize(
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<Project>>(projectsSource));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(projectsActual),
                Is.EqualTo(
                    Helpers.JsonCleanup(expectedResult)
                    ));
        }

        [Test]
        public async Task ProjectsFiltering_ReturnAll()
        // this test validates that our object filtering will return all when it's supposed to
        {
            // Arrange
            string expectedResult = projectsSource;

            // Act
            var projectSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Project>>(projectsSource)!;
                // we validate projectSet will not be null in the first test
            string projectsActual = JsonSerializer.Serialize(
                await ProjectEndpoints.GetProjectsFiltered("A", projectSet));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(projectsActual),
                Is.EqualTo(
                    Helpers.JsonCleanup(expectedResult)
                    ));
        }

        [Test]
        public async Task ProjectsFiltering_ReturnNone()
        // this test validates that our object filtering will return none when it's supposed to
        {
            // Arrange
            string expectedResult = "[]";

            // Act
            var projectSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Project>>(projectsSource)!;
            string projectsActual = JsonSerializer.Serialize(
                await ProjectEndpoints.GetProjectsFiltered("xzxzxzxz", projectSet));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(projectsActual),
                Is.EqualTo(
                    Helpers.JsonCleanup(expectedResult)
                    ));
        }

        [Test]
        public async Task ProjectsFiltering_ReturnSome()
        // this test validates that our object filtering will return some when it's supposed to
        {
            // Arrange
            string expectedResult = @"[
                    {
                        ""pid"": 6,
                        ""projectName"": ""Foofle Search"",
                        ""jiraId"": ""FS948372A"",
                        ""wrikeId"": ""FS023874A"",
                        ""createdDate"": ""1900-01-01T00:00:00"",
                        ""updatedDate"": null
                    },
                    {
                        ""pid"": 11,
                        ""projectName"": ""Common Poor Bank"",
                        ""jiraId"": ""CB094328A"",
                        ""wrikeId"": ""CB394872A"",
                        ""createdDate"": ""2017-06-12T00:00:00"",
                        ""updatedDate"": null
                    },
                    {
                        ""pid"": 13,
                        ""projectName"": ""Fakebook"",
                        ""jiraId"": ""FK984327A"",
                        ""wrikeId"": ""FK948326A"",
                        ""createdDate"": ""2020-05-22T00:00:00"",
                        ""updatedDate"": null
                    }
                ]";

            // Act
            var projectSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Project>>(projectsSource)!;
            string projectsActual = JsonSerializer.Serialize(
                await ProjectEndpoints.GetProjectsFiltered("oo", projectSet));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(projectsActual),
                Is.EqualTo(
                    Helpers.JsonCleanup(expectedResult)
                    ));
        }

        private static string projectsSource =
            @"[
                  {
                    ""pid"": 1,
                    ""projectName"": ""Myspace"",
                    ""jiraId"": ""AAA1A"",
                    ""wrikeId"": ""XXX1A"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 2,
                    ""projectName"": ""Myspace competitor"",
                    ""jiraId"": ""BBB2A"",
                    ""wrikeId"": ""YYY2A"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 3,
                    ""projectName"": ""Popular app"",
                    ""jiraId"": ""CCC3A"",
                    ""wrikeId"": ""ZZZ3A"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 4,
                    ""projectName"": ""Fastgram"",
                    ""jiraId"": ""FG126704A"",
                    ""wrikeId"": ""FG057834A"",
                    ""createdDate"": ""2019-09-09T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 5,
                    ""projectName"": ""FMail"",
                    ""jiraId"": ""FM294875A"",
                    ""wrikeId"": ""FM937452A"",
                    ""createdDate"": ""2017-04-19T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 6,
                    ""projectName"": ""Foofle Search"",
                    ""jiraId"": ""FS948372A"",
                    ""wrikeId"": ""FS023874A"",
                    ""createdDate"": ""1900-01-01T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 7,
                    ""projectName"": ""Amazoff Shopped"",
                    ""jiraId"": ""AS947326A"",
                    ""wrikeId"": ""AS049328A"",
                    ""createdDate"": ""2023-10-10T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 8,
                    ""projectName"": ""COD Immobile"",
                    ""jiraId"": ""CI049327A"",
                    ""wrikeId"": ""CI023498A"",
                    ""createdDate"": ""2021-05-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 9,
                    ""projectName"": ""Flappy Emu-dev"",
                    ""jiraId"": ""FE843738A"",
                    ""wrikeId"": ""FE049389A"",
                    ""createdDate"": ""2020-07-07T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 10,
                    ""projectName"": ""Stoppy Street"",
                    ""jiraId"": ""SS943049A"",
                    ""wrikeId"": ""SS204949A"",
                    ""createdDate"": ""2019-02-02T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 11,
                    ""projectName"": ""Common Poor Bank"",
                    ""jiraId"": ""CB094328A"",
                    ""wrikeId"": ""CB394872A"",
                    ""createdDate"": ""2017-06-12T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 12,
                    ""projectName"": ""Crypto Miner"",
                    ""jiraId"": ""CM984372A"",
                    ""wrikeId"": ""CM284736A"",
                    ""createdDate"": ""2020-08-22T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 13,
                    ""projectName"": ""Fakebook"",
                    ""jiraId"": ""FK984327A"",
                    ""wrikeId"": ""FK948326A"",
                    ""createdDate"": ""2020-05-22T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 14,
                    ""projectName"": ""WeDontChat"",
                    ""jiraId"": ""WC043928A"",
                    ""wrikeId"": ""WC390283A"",
                    ""createdDate"": ""2018-12-31T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""pid"": 15,
                    ""projectName"": ""kiT koT"",
                    ""jiraId"": ""KK204938A"",
                    ""wrikeId"": ""KK103928A"",
                    ""createdDate"": ""2012-02-29T00:00:00"",
                    ""updatedDate"": null
                  }
            ]";
    }
}
