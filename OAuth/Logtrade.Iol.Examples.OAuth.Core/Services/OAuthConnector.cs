using Logtrade.Iol.Examples.OAuth.Core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Microsoft.Extensions.Logging;

namespace Logtrade.Iol.Examples.OAuth.Core.Services
{
    public class OAuthConnector
    {
        private static List<string> dummyStateDatabase = new List<string>();

        private readonly ILogger<OAuthConnector> logger;

        public OAuthConnector(ILogger<OAuthConnector> logger)
        {
            this.logger = logger;
        }

        public string GenerateConnectionString(ConnectionRequestModel connection)
        {
            var state = Guid.NewGuid().ToString().Replace("-", "");

            connection.State = state;

            //you will want to store state to verify any connection we send back to you
            //this code just stores it in memory as an example
            dummyStateDatabase.Add(state);

            var url = $"{Env.Settings.IolDomain}/oauth/connect";
            url += $"?client_id={connection.ClientId}";
            url += $"&state={connection.State}";
            url += $"&connection_name={HttpUtility.UrlEncode(connection.ConnectionName)}";

            if (!string.IsNullOrEmpty(connection.RequestingPartyId))
            {
                url += $"&requesting_party={connection.RequestingPartyId}";
            }

            if (connection.ScopeList.Count > 0)
            {
                var scopeString = string.Join('+', connection.ScopeList.Where(s => !string.IsNullOrWhiteSpace(s)));
                url += $"&scope={scopeString}";

            }
            else if (connection.ScopeObjects.Count > 0)
            {
                var scopeString = JsonSerializer.Serialize(connection.ScopeObjects);
                url += $"&scope={HttpUtility.UrlEncode(scopeString)}";
            }

            logger.LogDebug("Generated connection string of {url}", url);

            return url;
        }

        public ConnectionResult ProcessResponse(IolResponseModel model)
        {
            logger.LogTrace("Iol notification of connection with response {messageType} for state {state}", model.MessageType, model.State);

            if(!string.IsNullOrEmpty(model.AccessToken))
            {
                logger.LogTrace("Received access token: {accessToken}", model.AccessToken);
            }

            if (model.MessageType != "connection_successful")
            {
                //here you would handle the error
                //it might be connection_rejected if the client cliecks "Deny"
                //or you may recieve a message later of connection_revoked if a connection is deleted
                //you should not return a bearer token, but respond with a 200

                return new ConnectionResult()
                {
                    Success = true
                };
            }

            if (CheckResponseModel(model))
            {
                //You would setup the connection here for your side
                //and generate a bearer token

                var bearerToken = "example_bearertoken";

                logger.LogTrace("Successfully connected request {state} to {legalPartyId}", model.State, model.AcceptingParty["legalPartyId"]);

                return new ConnectionResult()
                {
                    Success = true,
                    BearerToken = bearerToken
                };
            }
            else
            {
                //if you can't process our request you should respond with an error
                logger.LogTrace("Rejected connection request {state}", model.State);

                return new ConnectionResult()
                {
                    Success = false
                };
            }
        }

        private bool CheckResponseModel(IolResponseModel model)
        {
            //check our example in-memory state store made this request
            return dummyStateDatabase.Contains(model.State);
        }
    }
}
