using Microsoft.EntityFrameworkCore;
using HashiAPI_1.Models;
namespace HashiAPI_1.Controllers;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints (this IEndpointRouteBuilder routes)
    {
        // GET all
        routes.MapGet("/api/Project", async (hashi_dbContext db) =>
        {
            return await db.Projects.ToListAsync();
        })
        .WithName("GetAllProjects");

        // GET query
        routes.MapGet("/api/Project/{QUERY}", async (string query, hashi_dbContext db) =>
        {
            var proj = db.Projects;
            List<Project> result = await GetProjectsFiltered(query, proj);

            return (result.Count == 0) ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetProjectBySearch");

        // PUT
        routes.MapPut("/api/Project/{PID}", async (int PID, Project project, hashi_dbContext db) =>
        {
            var foundModel = await db.Projects.FindAsync(PID);

            if (foundModel is null)
            {
                return Results.NotFound();
            }
            //update model properties here
            foundModel.ProjectName = (project.ProjectName != null && project.ProjectName != "")
                ? project.ProjectName : foundModel.ProjectName;
            foundModel.JiraId = (project.JiraId != null && project.JiraId != "")
                ? project.JiraId : foundModel.JiraId;
            foundModel.WrikeId = (project.WrikeId != null && project.WrikeId != "")
                ? project.WrikeId : foundModel.WrikeId;

            foundModel.UpdatedDate = (project.UpdatedDate != null)
                ? project.UpdatedDate : foundModel.UpdatedDate;

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateProject");

        // POST
        routes.MapPost("/api/Project/", async (Project project, hashi_dbContext db) =>
        {
            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return Results.Created($"/Projects/{project.PID}", project);
        })
        .WithName("CreateProject");

        // DELETE
        routes.MapDelete("/api/Project/{PID}", async (int PID, hashi_dbContext db) =>
        {
            if (await db.Projects.FindAsync(PID) is Project project)
            {
                db.Projects.Remove(project);
                await db.SaveChangesAsync();
                return Results.Ok(project);
            }

            return Results.NotFound();
        })
        .WithName("DeleteProject");
    }

    public static async Task<List<Project>> GetProjectsFiltered(string query, IEnumerable<Project> proj)
    {
        List<Project> result = new();

        await Task.Run(() =>
        {
            result = proj.AsEnumerable().
                Where(q => query.Split(" ").All(
                k => (q.ProjectName.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                    (q.JiraId.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                    (q.WrikeId.Contains(k, StringComparison.OrdinalIgnoreCase))

            )).ToList();
        });
        return result;
    }
}
