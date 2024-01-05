using HashiAPI_1.Models;
namespace HashiAPI_1.Controllers;

public static class WrikeEndpoints
{
    private static readonly External.WrikeSession wrike = External.WrikeSession.GetInstance();
    public static void MapWrikeEndpoints (this IEndpointRouteBuilder routes)
    {
        // GET all projects
        routes.MapGet("/api/Wrike/Project", async () =>
        {
            var result = await wrike.GetProjects();
            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetAllWrikeProjects");

        // GET query projects
        routes.MapGet("/api/Wrike/Project/{QUERY}", async (string query) =>
        {
            var proj = await wrike.GetProjects();
            List<WrikeProject> result = await GetProjectsFiltered(query, proj);

            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetWrikeProjectBySearch");

        // GET all users
        routes.MapGet("/api/Wrike/User", async () =>
        {
            var result = await wrike.GetUsers();
            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetAllWrikeUsers");

        // GET query users
        routes.MapGet("/api/Wrike/User/{QUERY}", async (string query) =>
        {
            var usr = await wrike.GetUsers();
            List<WrikeUser> result = await GetUsersFiltered(query, usr);
            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetWrikeUserBySearch");

    }

    private static async Task<List<WrikeProject>> GetProjectsFiltered(string query, List<WrikeProject> proj)
    {
        var result = new List<WrikeProject>();
        await Task.Run(() =>
        {
            foreach (WrikeProject p in proj)
            {
                result = proj.AsEnumerable().
                    Where(q => query.Split(" ").All(
                    k => (q.Title.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                        (q.Id.Contains(k, StringComparison.OrdinalIgnoreCase))
                )).ToList();
            }
        });
        return result;
    }

    private static async Task<List<WrikeUser>> GetUsersFiltered(string query, List<WrikeUser> usr)
    {
        var result = new List<WrikeUser>();
        await Task.Run(() =>
        {
            foreach (WrikeUser u in usr)
            {
                result = usr.AsEnumerable().
                    Where(q => query.Split(" ").All(
                    k => (q.Id is not null && 
                            q.Id.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                        (q.Email is not null && 
                            q.Email.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                        (q.DisplayName() is not null && 
                            q.DisplayName().Contains(k, StringComparison.OrdinalIgnoreCase))
                )).ToList<WrikeUser>();
            }
        });
        return result;
    }
}
