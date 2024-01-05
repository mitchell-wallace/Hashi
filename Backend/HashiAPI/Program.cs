using HashiAPI_1.Controllers;
using HashiAPI_1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.StaticFiles;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<hashi_dbContext>();
//builder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "wwwroot";
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Initialise Jira and Wrike API connections
await External.JiraSession.GetInstance().Initialise();
await External.WrikeSession.GetInstance().Initialise();

// enable default to serving a static file
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});
app.UseStaticFiles();

// misc setup
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// setup API endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapProjectEndpoints();
app.MapUserEndpoints();
app.MapWrikeEndpoints();
app.MapJiraEndpoints();

app.Run();