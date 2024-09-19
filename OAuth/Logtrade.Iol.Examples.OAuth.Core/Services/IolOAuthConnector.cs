using Logtrade.Iol.Examples.OAuth.Core.Models;
using Logtrade.Iol.Examples.OAuth.Core.Models.ExampleRepository;
using Logtrade.Iol.Examples.OAuth.Core.Models.Iol;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Logtrade.Iol.Examples.OAuth.Core.Services
{
    public class IolOAuthConnector(ExampleRepository exampleRepo, HttpClient httpClient, ILogger<IolOAuthConnector> logger)
    {
        public async Task<ConnectionRequestResult> MakeAssistedConnectionRequest(ConnectionRequestModel connection)
        {
            var result = new ConnectionRequestResult();

            //generate a unique state
            var state = Guid.NewGuid().ToString().Replace("-", "");

            //you will want to store this state to verify any connection we send back to you
            //this exampleRepo just stores it in memory as an example
            exampleRepo.Insert(new()
            {
                State = state,
                IolOrganisationIdentity = connection.ConnectToId,
                ConnectionType = ConnectionType.AssistedConnection
            });

            var data = JsonSerializer.Serialize(new RequestConnection()
            {
                ClientId = connection.ClientId,
                ConnectToId = connection.ConnectToId,
                State = state
            });

            var url = Env.Settings.IolDomain + "/api/application/request-connection";

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(data, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "oyuy8rhk3fydknt9ci1aomuaemh1ikft4xmgbq1jr134y93jesuo");
            
            try
            {
                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    result.ErrorCode = response.StatusCode.ToString();
                    result.ErrorMessage = await response.Content.ReadAsStringAsync();
                }

                var connectionRequest = JsonSerializer.Deserialize<ConnectionRequest>(await response.Content.ReadAsStringAsync());

                result.Succeeded = true;
                result.Request = connectionRequest;
            } 
            catch(Exception e)
            {
                result.ErrorCode = "500";
                result.ErrorMessage = $"Exception caught {e.Message}";
            }

            return result;
        }

        public string GenerateManualConnectionString(
            string clientId,
            string requestingPartyId = "",
            string name = "")
        {
            //generate a unique state
            var state = Guid.NewGuid().ToString().Replace("-", "");

            //you will want to store this state to verify any connection we send back to you
            //this exampleRepo just stores it in memory as an example
            exampleRepo.Insert(new()
            {
                State = state,
                ConnectionType = ConnectionType.ManualConnection
            });

            var url = $"{Env.Settings.IolDomain}/oauth/connect";
            url += $"?client_id={clientId}";
            url += $"&state={state}";

            if (!string.IsNullOrEmpty(requestingPartyId))
            {
                url += $"&requesting_party={HttpUtility.UrlEncode(requestingPartyId)}";
            }

            //you can also over-ride the default name for a connection here
            if (!string.IsNullOrWhiteSpace(name))
            {
                url += $"&connection_name={HttpUtility.UrlEncode(name)}";
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

            return model.MessageType switch
            {
                "connection_successful" => HandleNewConnection(model),
                "connection_rejected" => HandleRejectedConnection(model),
                "connection_revoked" => HandleRevokedConnection(model),
                _ => throw new Exception("Unrecognized MessageType"),
            };
        }

        public ConnectionResult HandleNewConnection(IolResponseModel model)
        {
            var connection = exampleRepo.GetByState(model.State);

            if (connection != null)
            {
                logger.LogTrace("Successfully connected request {state} to {iolOrganisationIdentity}", model.State, model.AcceptingParty["legalPartyId"]);

                //You would setup the connection here for your side
                //and generate a bearer token
                var bearerToken = "example_bearertoken";
                connection.ConnectionState = "connection_successful";

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

        public ConnectionResult HandleRejectedConnection(IolResponseModel model)
        {
            logger.LogTrace("The connection to {ClientId} with state {State} was rejected", model.ClientId, model.State);

            var connection = exampleRepo.GetByState(model.State);

            if(connection != null)
            {
                connection.ConnectionState = "connection_rejected";
            }
            
            return new ConnectionResult()
            {
                Success = true
            };
        }

        public ConnectionResult HandleRevokedConnection(IolResponseModel model)
        {
            //here you would handle someone revoking your application's connection
            //you could use the original state or the acceptingParty to match it to your db entry
            //note that depending on your application complexity
            //you could theoretically have multiple connections to the same organisation
            //but most of the time you only want one
            logger.LogTrace("The connection to {ClientId} with state {State} was revoked", model.ClientId, model.State);
            var connection = exampleRepo.GetByState(model.State);

            if (connection != null)
            {
                connection.ConnectionState = "connection_revoked";
            }

            return new ConnectionResult()
            {
                Success = true
            };
        }
    }
}
