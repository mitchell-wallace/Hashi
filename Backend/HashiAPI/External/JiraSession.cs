using HashiAPI_1.Models;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;

namespace External {
	public class JiraSession
	{

		private DateTime lastUpdate;
		private TimeSpan refreshRate; // 2 minutes
			// refreshRate defines the minimum time elapsed before JiraConnect
			// will hit the API again
		private List<JiraProject> projects;
		private List<JiraUser> users;

		private static RestClient? jiraClient;

		private static JiraSession jiraSession = new JiraSession(); // singleton-style single instance

		public static JiraSession GetInstance() {return jiraSession;} // returns the singleton instance

		private JiraSession() {
			lastUpdate = DateTime.MinValue;
			refreshRate = new TimeSpan(0, 2, 0); 
			projects = new List<JiraProject>();
			users = new List<JiraUser>();
			// Initialise();
		}

		public bool Connect() {
			if (System.Environment.GetEnvironmentVariable("HASHI_JiraUrl") is not null &&
				System.Environment.GetEnvironmentVariable("HASHI_JiraUsername") is not null &&
				System.Environment.GetEnvironmentVariable("HASHI_JiraSecret") is not null) {

				try
				{
                    jiraClient = new RestClient(
                    System.Environment.GetEnvironmentVariable("HASHI_JiraUrl")!);
                    jiraClient.Authenticator = new HttpBasicAuthenticator(
                        System.Environment.GetEnvironmentVariable("HASHI_JiraUsername")!,
                        System.Environment.GetEnvironmentVariable("HASHI_JiraSecret")!);
                    return true;
                }
				catch (Exception e)
				{
                    Console.WriteLine("ERROR: An exception occurred while attempting to connect " +
						$"to Jira: \n{e}");
                    return false;
                }
			}
			else { // TODO send error message to client if unable to retrieve
				Console.WriteLine("ERROR: One or more environment variables required for " +
					"connection to Jira is NULL; connection to Jira is unavailable");	
				return false;
			}
		}

		public async Task<bool> Initialise(bool verboseDebug = false)
		{
			if (!Connect()) return false; // attempts to connect before attempting to retrieve data
			await GetProjects();
			await GetUsers();
			DateTime lastUpdate = DateTime.Now; // reset timeout

			if (verboseDebug) {
				Console.WriteLine("===== External API initial connection debug output - Jira =====");
				foreach(JiraProject p in projects) {
					Console.WriteLine("\tProject: " +p.ToString());
				}
				foreach(JiraUser u in users) {
					Console.WriteLine("\tUser: " + u.ToString());
				}
				// Console.WriteLine("\n\t---\n\n\t" + (await GetProjects()).ToString());
				Console.WriteLine("===== End debug output - Jira =====");
			}

			return true;
		}

		public async Task<List<JiraProject>> GetProjects(bool verboseDebug = false) {
			if (jiraClient is null)
            { // if this somehow gets called before initialisation, we need to initialise
                Console.WriteLine("ERROR: JiraSession has not been initialised; unable to retrieve users. " +
                    "Attemping to recover...");
                if (await Initialise()) Console.WriteLine("JiraSession initialised successfully.");
                else
                {
                    Console.WriteLine("JiraSession could not be initialised; returning empty list");
                    return new List<JiraProject>();
                }
			}
			if (lastUpdate + refreshRate < DateTime.Now) {
				var request = new RestRequest("project/", Method.Get);
				CancellationToken cancellationToken = default;
				RestResponse response = await jiraClient!.GetAsync(request, cancellationToken);
				if (response.IsSuccessful)
				{
					await Task.Run( () => {
						try {
							string? s = response.Content!.ToString();
							projects = DeserialiseProjects(s);
						}
						catch (Exception e) {
							Console.WriteLine("An error has occurred while deserialising Jira API projects response;"
							+ $"\nResponse received: NULL or INVALID\nException: {e}");
						}
					});
				}
			}
			if (verboseDebug)
			{
                Console.WriteLine("=== Request debug output - Jira Projects ===");
                foreach (JiraProject j in projects) { Console.WriteLine("\tJProj: " + j.ToString()); }
                Console.WriteLine("=== End debug out");
            }
			return projects;
		}

		public static List<JiraProject> DeserialiseProjects(string s)
		{
			List<JiraProject> projects2 = new();
            try
            {
                projects2 = JsonConvert.DeserializeObject<List<JiraProject>>(s)!;
                projects2.RemoveAll(p => p.IsPrivate == true);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error has occurred while deserialising Jira API projects response;"
                + $"\nResponse received: {s}\nException: {e}");
                return new List<JiraProject>();
            }

			return projects2;
        }

		public async Task<List<JiraUser>> GetUsers(bool verboseDebug = false) {
            if (jiraClient is null)
            { // if this somehow gets called before initialisation, we need to initialise
                Console.WriteLine("ERROR: JiraSession has not been initialised; unable to retrieve users. " +
                    "Attemping to recover...");
                if (await Initialise()) Console.WriteLine("JiraSession initialised successfully.");
                else
                {
                    Console.WriteLine("JiraSession could not be initialised; returning empty list");
                    return new List<JiraUser>();
                }
            }
            if (lastUpdate + refreshRate < DateTime.Now) {
				var request = new RestRequest("users/search/", Method.Get);
				CancellationToken cancellationToken = default;
				RestResponse response = await jiraClient!.GetAsync(request, cancellationToken);
				if (response.IsSuccessful)
				{
					await Task.Run( () => {
						try {
							string? s = response.Content!.ToString();
							users = DeserialiseUsers(s);
						}
						catch (Exception e) {
							Console.WriteLine("An error has occurred while deserialising Jira API projects response;"
							+ $"\nResponse received: NULL or INVALID\nException: {e}");
						}
					});
				}
			}
            if (verboseDebug)
            {
                Console.WriteLine("=== Request debug output - Jira Users ===");
                foreach (JiraProject j in projects) { Console.WriteLine("\tJUser: " + j.ToString()); }
                Console.WriteLine("=== End debug out");
            }
            return users;
		}

		public static List<JiraUser> DeserialiseUsers(string s)
		{
			List<JiraUser> users2 = new();
            try
            {
                users2 = JsonConvert.DeserializeObject<List<JiraUser>>(s)!;
                users2.RemoveAll(u => u.accountType != "atlassian");
                users2.RemoveAll(u => u.active == false);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error has occurred while deserialising Jira API users response;"
                + $"\nResponse received: {s}\nException: {e}");
                return new List<JiraUser>();
            }
			return users2;
        }
	}
}
