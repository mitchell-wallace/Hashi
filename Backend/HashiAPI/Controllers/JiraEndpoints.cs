using HashiAPI_1.Models;
namespace HashiAPI_1.Controllers;

public static class JiraEndpoints
{
    private static External.JiraSession jira = External.JiraSession.GetInstance();
    public static void MapJiraEndpoints (this IEndpointRouteBuilder routes)
    {
        // GET all projects
        routes.MapGet("/api/Jira/Project", async () =>
        {
            var result = await jira.GetProjects();
            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetAllJiraProjects");

        // GET query projects
        routes.MapGet("/api/Jira/Project/{QUERY}", async (string query) =>
        {
            var proj = await jira.GetProjects();
            List<JiraProject> result = await GetProjectsFiltered(query, proj);
            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetJiraProjectBySearch");

        // GET all users
        routes.MapGet("/api/Jira/User", async () =>
        {
            var result = await jira.GetUsers();
            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetAllJiraUsers");

        // GET query users
        routes.MapGet("/api/Jira/User/{QUERY}", async (string query) =>
        {
            var usr = await jira.GetUsers();
            List<JiraUser> result = await GetUsersFiltered(query, usr);
            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetJiraUserBySearch");

    }

    private static async Task<List<JiraProject>> GetProjectsFiltered(string query, List<JiraProject> proj)
    {
        var result = new List<JiraProject>();
        await Task.Run(() =>
        {
            foreach (JiraProject p in proj)
            {
                result = proj.AsEnumerable().
                    Where(q => query.Split(" ").All(
                    k => (q.Name.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                        (q.Key.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                        (q.Id.Contains(k, StringComparison.OrdinalIgnoreCase))
                )).ToList();

            }
        });
        return result;
    }

    private static async Task<List<JiraUser>> GetUsersFiltered(string query, List<JiraUser> usr)
    {
        var result = new List<JiraUser>();
        await Task.Run(() =>
        {
            foreach (JiraUser u in usr)
            {
                result = usr.AsEnumerable().
                    Where(q => query.Split(" ").All(
                    k => (q.accountId.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                        (q.emailAddress.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                        (q.displayName.Contains(k, StringComparison.OrdinalIgnoreCase))
                )).ToList<JiraUser>();
            }
        });
        return result;
    }
}
