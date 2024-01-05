using HashiAPI_1.Models;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace External {

	public class WrikeSession
	{

		private DateTime lastUpdate;
		private TimeSpan refreshRate; // 2 minutes
			// refreshRate defines the minimum time elapsed before WrikeConnect
			// will hit the API again
		private List<WrikeProject> projects;
		private List<WrikeUser> users;
		private static RestClient? wrikeClient;

		private static WrikeSession wrikeSession = new WrikeSession(); // singleton-style single instance

		public static WrikeSession GetInstance() {return wrikeSession;} // returns the singleton instance


		private WrikeSession() {
			lastUpdate = DateTime.MinValue;
			refreshRate = new TimeSpan(0, 2, 0); 
			projects = new List<WrikeProject>();
			users = new List<WrikeUser>();
			// Initialise();
		}

		public bool Connect() {
			if (System.Environment.GetEnvironmentVariable("HASHI_WrikeUrl") is not null &&
                System.Environment.GetEnvironmentVariable("HASHI_WrikeToken") is not null)

            {
				try
				{
                    wrikeClient = new RestClient(
						System.Environment.GetEnvironmentVariable("HASHI_WrikeUrl")!);
                    wrikeClient.Authenticator = new RestSharp.Authenticators.OAuth2
                        .OAuth2AuthorizationRequestHeaderAuthenticator(
                        System.Environment.GetEnvironmentVariable("HASHI_WrikeToken")!, "Bearer");
                    return true;
                }
				catch (Exception e)
				{
                    Console.WriteLine("ERROR: An exception occurred while attempting to connect " +
                        $"to Wrike: \n{e}");
                    return false;
                }
			}
			else { // TODO send error message to client if unable to retrieve
				Console.WriteLine("ERROR: One or more environment variables required for " +
					"connection to Wrike is NULL; connection to Jira is unavailable");
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
				Console.WriteLine("===== External API initial connection debug output - Wrike =====");
				foreach(WrikeProject p in projects) {
					Console.WriteLine("\tProject: " +p.ToString());
				}
				foreach(WrikeUser u in users) {
					Console.WriteLine("\tUser: " + u.ToString());
				}
				// Console.WriteLine("\n\t---\n\n\t" + (await GetProjects()).ToString());
				Console.WriteLine("===== End debug output - Jira =====");
			}

			return true;
		}

        public async Task<List<WrikeProject>> GetProjects(bool verboseDebug = false)
        {
            if (wrikeClient is null)
            { // if this somehow gets called before initialisation, we need to initialise
                Console.WriteLine("ERROR: WrikeSession has not been initialised; unable to retrieve users. " +
                    "Attemping to recover...");
                if (await Initialise()) Console.WriteLine("WrikeSession initialised successfully.");
                else
                {
                    Console.WriteLine("WrikeSession could not be initialised; returning empty list");
                    return new List<WrikeProject>();
                }
            }
            if (lastUpdate + refreshRate < DateTime.Now)
            {
                projects = new List<WrikeProject>(); // reset list since we add one-by-one for Wrike

                var request = new RestRequest("folders/", Method.Get);
                CancellationToken cancellationToken = default;
                RestResponse response = await wrikeClient!.GetAsync(request, cancellationToken);
                if (response.IsSuccessful)
                {
                    await Task.Run(() => {
                        try
                        {
                            string? s = response.Content!.ToString();
                            projects = DeserialiseProjects(s);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An error has occurred while deserialising Wrike API projects response;"
                            + $"\nResponse received: NULL or INVALID\nException: {e}");
                        }



                    });
                }
            }
            if (verboseDebug)
            {
                Console.WriteLine("=== Request debug output - Wrike Projects ===");
                foreach (WrikeProject w in projects) { Console.WriteLine("\tWProj: " + w.ToString()); }
                Console.WriteLine("=== End debug out");
            }

            return projects;
        }

		public static List<WrikeProject> DeserialiseProjects(string s)
		{
			List<WrikeProject> projects2 = new();

            try
            {
                JObject raw = JObject.Parse(s);

                // get JSON data objects into a list
                IList<JToken> data = raw["data"]!.Children().ToList();

                // serialise JSON data into .NET objects
                foreach (JToken item in data)
                {
                    // JToken.ToObject is a helper method that uses JsonSerializer internally
                    WrikeProject proj = item.ToObject<WrikeProject>()!;

                    var projectData = item["project"];
                    if (projectData != null) projects2.Add(proj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error has occurred while deserialising Wrike API projects response;"
                + $"\nResponse received: {s}\nException: {e}");
                return new List<WrikeProject>();
            }

			return projects2;
        }

        public async Task<List<WrikeUser>> GetUsers(bool verboseDebug = false) 
		{
			if (wrikeClient is null)
            { // if this somehow gets called before initialisation, we need to initialise
                Console.WriteLine("ERROR: WrikeSession has not been initialised; unable to retrieve users. " +
					"Attemping to recover...");
				if (await Initialise()) Console.WriteLine("WrikeSession initialised successfully.");
				else {
					Console.WriteLine("WrikeSession could not be initialised; returning empty list");
					return new List<WrikeUser>();
				}
			}
			if (lastUpdate + refreshRate < DateTime.Now) {
				users = new List<WrikeUser>(); // reset list since we add one-by-one for Wrike

				var request = new RestRequest("contacts/", Method.Get);
				CancellationToken cancellationToken = default;
				RestResponse response = await wrikeClient!.GetAsync(request, cancellationToken);
				if (response.IsSuccessful)
				{
					await Task.Run( () => {
						try {
							string? s = response.Content!.ToString();
                            users = DeserialiseUsers(s);

						}
						catch (Exception e) {
							Console.WriteLine("An error has occurred while deserialising Wrike API users response;"
							+ $"\nResponse received: NULL or INVALID\nException: {e}");
						}
					});
				}
			}
			if (verboseDebug)
			{
				Console.WriteLine("=== Request debug output - Wrike Users ===");
				foreach (WrikeUser w in users) { Console.WriteLine("\tWUser:" + w.ToString()); }
				Console.WriteLine("=== End debug out");
			}

            return users;
		}

		public static List<WrikeUser> DeserialiseUsers(string s)
		{
			List<WrikeUser> users2 = new();

            try
            {
                JObject raw = JObject.Parse(s);

                // get JSON data objects into a list
                IList<JToken> data = raw["data"]!.Children().ToList();

                // serialise JSON data into .NET objects

                foreach (JToken item in data)
                {
                    // JToken.ToObject is a helper method that uses JsonSerializer internally
                    WrikeUser usr = item.ToObject<WrikeUser>()!;

                    var profiles = item["profiles"]!.Children().ToList();
                    if (profiles.Count > 0)
                    {
                        if (profiles[0]["email"] != null)
                            usr.Email = profiles[0]["email"]!.ToString();
                    }

                    if (usr.Type == "Person")
                    {
                        users2.Add(usr);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error has occurred while deserialising Wrike API users response;"
                + $"\nResponse received: {s}\nException: {e}");
                return new List<WrikeUser>();
            }

            return users2;
		}
	}
}