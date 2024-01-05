using HashiAPI_1.Controllers;
using HashiAPI_1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace HashiAPI_Tests
{
    public class DatabaseTestsForUsers
    {
        [Test]
        public void UserObject_TestJSONIsValid()
        // this test validates that our sample JSON is valid
        {
            // Arrange

            // Act
            string usersActual = JsonSerializer.Serialize(
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(usersSource));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(usersActual),
                Is.Not.Null);
        }

        [Test]
        public void UsersObject_IsConsistentWithSampleJSON()
            // this test validates that our sample JSON is consistent with our users object
        {
            // Arrange
            string expectedResult = usersSource;

            // Act
            string usersActual = JsonSerializer.Serialize(
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(usersSource));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(usersActual),
                Is.EqualTo(
                    Helpers.JsonCleanup(expectedResult)
                    ));
        }

        [Test]
        public async Task UsersFiltering_ReturnAll()
        // this test validates that our object filtering will return all when it's supposed to
        {
            // Arrange
            string expectedResult = usersSource; // we expect this query to return all

            // Act
            var userSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(usersSource)!;
                // we validate projectSet will not be null in the first test
            string usersActual = JsonSerializer.Serialize(
                await UserEndpoints.GetUsersFiltered("com", userSet));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(usersActual),
                Is.EqualTo(
                    Helpers.JsonCleanup(expectedResult)
                    ));
        }

        [Test]
        public async Task UsersFiltering_ReturnNone()
        // this test validates that our object filtering will return none when it's supposed to
        {
            // Arrange
            string expectedResult = "[]";

            // Act
            var userSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(usersSource)!;
            string usersActual = JsonSerializer.Serialize(
                await UserEndpoints.GetUsersFiltered("xzxzxzxz", userSet));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(usersActual),
                Is.EqualTo(
                    Helpers.JsonCleanup(expectedResult)
                    ));
        }

        [Test]
        public async Task UsersFiltering_ReturnSome()
        // this test validates that our object filtering will return some when it's supposed to
        {
            // Arrange
            string expectedResult = @"[
                {
                    ""uid"": 2,
                    ""email"": ""JohnSmith@example.com"",
                    ""displayName"": ""John Smith"",
                    ""jiraId"": ""ABC111"",
                    ""wrikeId"": ""XYZ777"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                },
                {
                    ""uid"": 4,
                    ""email"": ""JoeSchmoe@example.com"",
                    ""displayName"": ""Joe Schmoe"",
                    ""jiraId"": ""ABC333"",
                    ""wrikeId"": ""XYZ999"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                },
                {
                    ""uid"": 5,
                    ""email"": ""JohnDeere@example.com"",
                    ""displayName"": ""John Deere"",
                    ""jiraId"": ""ABC444"",
                    ""wrikeId"": ""XYZ444"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                }
            ]";

            // Act
            var userSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(usersSource)!;
            string usersActual = JsonSerializer.Serialize(
                await UserEndpoints.GetUsersFiltered("jo", userSet));

            // Assert
            Assert.That(
                Helpers.JsonCleanup(usersActual),
                Is.EqualTo(
                    Helpers.JsonCleanup(expectedResult)
                    ));
        }

        private static string usersSource =
            @"[
                  {
                    ""uid"": 1,
                    ""email"": ""MyspaceTom@example.com"",
                    ""displayName"": ""Myspace Tom"",
                    ""jiraId"": ""ABC123"",
                    ""wrikeId"": ""XYZ789"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 2,
                    ""email"": ""JohnSmith@example.com"",
                    ""displayName"": ""John Smith"",
                    ""jiraId"": ""ABC111"",
                    ""wrikeId"": ""XYZ777"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 3,
                    ""email"": ""JaneDoe@example.com"",
                    ""displayName"": ""Jane Doe"",
                    ""jiraId"": ""ABC222"",
                    ""wrikeId"": ""XYZ888"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 4,
                    ""email"": ""JoeSchmoe@example.com"",
                    ""displayName"": ""Joe Schmoe"",
                    ""jiraId"": ""ABC333"",
                    ""wrikeId"": ""XYZ999"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 5,
                    ""email"": ""JohnDeere@example.com"",
                    ""displayName"": ""John Deere"",
                    ""jiraId"": ""ABC444"",
                    ""wrikeId"": ""XYZ444"",
                    ""createdDate"": ""2022-04-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 6,
                    ""email"": ""PeterParker@example.com"",
                    ""displayName"": ""Peter Parker"",
                    ""jiraId"": ""SP456Y"",
                    ""wrikeId"": ""SP2RE1"",
                    ""createdDate"": ""2022-08-21T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 7,
                    ""email"": ""TaylorSwift@example.com"",
                    ""displayName"": ""Taylor Swift"",
                    ""jiraId"": ""SW8739"",
                    ""wrikeId"": ""SW0304"",
                    ""createdDate"": ""2022-08-22T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 8,
                    ""email"": ""JiminyCricket@example.com"",
                    ""displayName"": ""Jiminy Cricket"",
                    ""jiraId"": ""CR39685"",
                    ""wrikeId"": ""CR100022"",
                    ""createdDate"": ""2021-05-05T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 9,
                    ""email"": ""JeremyClarkson@example.com"",
                    ""displayName"": ""Jeremy Clarkson"",
                    ""jiraId"": ""CL29384"",
                    ""wrikeId"": ""CL039465"",
                    ""createdDate"": ""2020-02-28T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 10,
                    ""email"": ""RichardHammond@example.com"",
                    ""displayName"": ""Richard Hammond"",
                    ""jiraId"": ""HA10394"",
                    ""wrikeId"": ""HA873493"",
                    ""createdDate"": ""2021-09-11T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 11,
                    ""email"": ""JamesMay@example.com"",
                    ""displayName"": ""James May"",
                    ""jiraId"": ""MA384920"",
                    ""wrikeId"": ""MA102954"",
                    ""createdDate"": ""2019-11-14T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 12,
                    ""email"": ""MauriceMoss@example.com"",
                    ""displayName"": ""Maurice Moss"",
                    ""jiraId"": ""MO385629"",
                    ""wrikeId"": ""MO013859"",
                    ""createdDate"": ""2013-03-21T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 13,
                    ""email"": ""DenimReynholm@example.com"",
                    ""displayName"": ""Denim Rehnholm"",
                    ""jiraId"": ""RE294857"",
                    ""wrikeId"": ""RE947263"",
                    ""createdDate"": ""2014-01-17T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 14,
                    ""email"": ""SantaClaus@example.com"",
                    ""displayName"": ""Santa Claus"",
                    ""jiraId"": ""CL483092"",
                    ""wrikeId"": ""CL204965"",
                    ""createdDate"": ""2018-12-25T00:00:00"",
                    ""updatedDate"": null
                  },
                  {
                    ""uid"": 15,
                    ""email"": ""EasterBunny@example.com"",
                    ""displayName"": ""Easter Bunny"",
                    ""jiraId"": ""BU396047"",
                    ""wrikeId"": ""BU105739"",
                    ""createdDate"": ""2019-05-05T00:00:00"",
                    ""updatedDate"": null
                  }
            ]";
    }
}
