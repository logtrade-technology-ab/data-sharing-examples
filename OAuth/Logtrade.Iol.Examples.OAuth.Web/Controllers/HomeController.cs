using Logtrade.Iol.Examples.OAuth.Core;
using Logtrade.Iol.Examples.OAuth.Core.Models;
using Logtrade.Iol.Examples.OAuth.Core.Services;
using Logtrade.Iol.Examples.OAuth.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Logtrade.Iol.Examples.OAuth.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly OAuthConnector oAuthConnector;

        public HomeController(OAuthConnector oAuthConnector)
        {
            this.oAuthConnector = oAuthConnector;
        }

        [Route("")]
        public IActionResult Home()
        {
            var invalidConfig =
                string.IsNullOrWhiteSpace(Env.Settings.IolDomain)
                || string.IsNullOrWhiteSpace(Env.Settings.ClientId);

            if (invalidConfig)
                return Redirect("/setup");

            return View(new HomeViewModel());
        }

        [HttpPost, Route("connect")]
        public IActionResult Connect(HomeViewModel model)
        {
            var connection = new ConnectionRequestModel()
            {
                ClientId = Env.Settings.ClientId,
                ConnectionName = $"Iol OAuth Test", //$"Iol OAuth Test {DateTime.Now.ToString("dd MMM yyyy hh:mm")}",
                RequestingPartyId = model.RequestingParty
            };

            if (model.Scope == ScopeEnum.StringArray)
            {
                connection.ScopeList = new List<string>()
                {
                    "api.tradeunit.edit",
                    "api.transportinstruction.edit"
                };
            }
            else if (model.Scope == ScopeEnum.ObjectArray)
            {
                connection.ScopeObjects = new List<ScopeModel>()
                {
                    new ScopeModel()
                    {
                        Scope = "api.tradeunit.edit",
                        Filtered = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("currencyCode", "SEK")
                        },
                        Excluded = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("quantity", "")
                        }
                    }
                };
            }

            var connectionString = oAuthConnector.GenerateConnectionString(connection);
            return View(new ConnectViewModel()
            {
                ConnectionString = connectionString
            });
        }

        [Route("setup")]
        public IActionResult Setup()
        {
            return View();
        }

        [Route("confirm_oauth"), HttpPost]
        public IActionResult ConfirmConnection([FromBody] IolResponseModel model)
        {
            var result = oAuthConnector.ProcessResponse(model);

            if (result.Success)
            {
                return Json(new BearerTokenResponseModel()
                {
                    BearerToken = result.BearerToken
                });
            }
            else
            {
                return BadRequest();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}