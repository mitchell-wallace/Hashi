using HashiAPI_1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HashiAPI_Tests
{
    internal class DatabaseIntegrationTests
        // these can be removed if these tests aren't needed
        // test for valid connections to Jira, Wrike, and database
    {

        [Test]
        public void RetrieveProjectsFromDatabase_GreaterThanZero()
        {
            // Arrange

            var builder = WebApplication.CreateBuilder();
            builder.Services.AddDbContext<hashi_dbContext>();

            DbSet<Project> proj;
            List<Project> projList;

            // Act
            using (hashi_dbContext db = new())
            {
                proj = db.Projects;
                projList = proj.ToList();
            }

            // Assert
            Assert.That(projList.Count(), Is.GreaterThan(0));

        }

        [Test]
        public void RetrieveUsersFromDatabase_GreaterThanZero()
        {
            // Arrange

            var builder = WebApplication.CreateBuilder();
            builder.Services.AddDbContext<hashi_dbContext>();

            DbSet<User> usr;
            List<User> usrList;

            // Act
            using (hashi_dbContext db = new())
            {
                usr = db.Users;
                usrList = usr.ToList();
            }

            // Assert
            Assert.That(usrList.Count(), Is.GreaterThan(0));

        }
    }
}
