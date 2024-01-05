using Microsoft.EntityFrameworkCore;
using HashiAPI_1.Models;
namespace HashiAPI_1.Controllers;

public static class UserEndpoints
{
    public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        // GET all
        routes.MapGet("/api/User", async (hashi_dbContext db) =>
        {
            // Console.WriteLine("DEBUG: api/User to string: " + await db.Users.ToListAsync());
            return await db.Users.ToListAsync();
        })
        .WithName("GetAllUsers");

        // GET query
        routes.MapGet("/api/User/{QUERY}", async (string query, hashi_dbContext db) =>
        {
            var usr = db.Users;
            List<User> result = await GetUsersFiltered(query, usr);

            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetUserBySearch");

        // PUT
        routes.MapPut("/api/User/{UID}", async (int UID, User user, hashi_dbContext db) =>
        {
            var foundModel = await db.Users.FindAsync(UID);

            if (foundModel is null)
            {
                return Results.NotFound();
            }
            //update model properties here
            foundModel.Email = (user.Email != null && user.Email != "")
                ? user.Email : foundModel.Email;
            foundModel.DisplayName = (user.DisplayName != null && user.DisplayName != "")
                ? user.DisplayName : foundModel.DisplayName;
            foundModel.JiraId = (user.JiraId != null && user.JiraId != "")
                ? user.JiraId : foundModel.JiraId;
            foundModel.WrikeId = (user.WrikeId != null && user.WrikeId != "")
                ? user.WrikeId : foundModel.WrikeId;

            foundModel.UpdatedDate = (user.UpdatedDate != null)
                ? user.UpdatedDate : foundModel.UpdatedDate;

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateUser");

        // POST
        routes.MapPost("/api/User/", async (User user, hashi_dbContext db) =>
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"/Users/{user.UID}", user);
        })
        .WithName("CreateUser");

        // DELETE
        routes.MapDelete("/api/User/{UID}", async (int UID, hashi_dbContext db) =>
        {
            if (await db.Users.FindAsync(UID) is User user)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Results.Ok(user);
            }

            return Results.NotFound();
        })
        .WithName("DeleteUser");
    }

    public static async Task<List<User>> GetUsersFiltered(string query, IEnumerable<User> usr)
    {
        List<User> result = new List<User>();
        await Task.Run(() =>
        {
            result = usr.AsEnumerable().
                Where(q => query.Split(" ").All(
                k => (q.Email.IndexOf(
                    k, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (q.DisplayName is not null && 
                        q.DisplayName.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                    (q.JiraId.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                    (q.WrikeId.Contains(k, StringComparison.OrdinalIgnoreCase))

            )).ToList();
        });
        return result;
    }
}
