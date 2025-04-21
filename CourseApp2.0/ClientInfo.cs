using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace CourseApp2._0
{
    public class ClientInfo
    {
        private const string Google_Secret = "Googleauthsecret.json";

        public static ClientInfo Load()
        {
            // Instead of relying on env variable
            string clientSecretFilename = Path.Combine(Directory.GetCurrentDirectory(), "Googleauthsecret.json");

            if (!File.Exists(clientSecretFilename))
            {
                throw new InvalidOperationException($"OAuth credentials file not found at: {clientSecretFilename}");
            }

            var secrets = JObject.Parse(File.ReadAllText(clientSecretFilename))["web"];

            if (secrets is null)
            {
                throw new InvalidOperationException(
                    $"The type of the OAuth2 client ID specified in Googleauthsecret.json should be Web Application. You can read more here: https://support.google.com/cloud/answer/6158849?hl=en");
            }

            var projectId = secrets["project_id"]?.ToString();
            var clientId = secrets["client_id"]?.ToString();
            var clientSecret = secrets["client_secret"]?.ToString();

            return new ClientInfo(projectId, clientId, clientSecret);
        }

        private ClientInfo(string projectId, string clientId, string clientSecret)
        {
            ProjectId = projectId;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string ProjectId { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
    }
}
