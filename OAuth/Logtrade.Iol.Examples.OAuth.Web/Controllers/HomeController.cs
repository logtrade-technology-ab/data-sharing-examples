using Logtrade.Iol.Examples.OAuth.Core.Models;
using Logtrade.Iol.Examples.OAuth.Core.Models.Iol;
using Logtrade.Iol.Examples.OAuth.Core.Services;
using Logtrade.Iol.Examples.OAuth.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Logtrade.Iol.Examples.OAuth.Web.Controllers;

public class HomeController(IolOAuthConnector oAuthConnector, ExampleRepository repo) : Controller
{
    [Route("")]
    public IActionResult Home()
    {
        return View(repo.GetAll());
    }

    [HttpGet, Route("assisted-connect")]
    public IActionResult AssistedConnect() => View(new AssistedConnectModel());

    [HttpPost, Route("assisted-connect")]
    public async Task<IActionResult> AssistedConnect(AssistedConnectModel model)
    {
        var connection = new ConnectionRequestModel()
        {
            ClientId = model.ClientId,
            RequestingPartyId = string.IsNullOrWhiteSpace(model.RequestingPartyId) ? model.RequestingPartyId : null,
            ConnectToId = model.ConnectToId,

        };

        if (!ModelState.IsValid) return View(model);

        var request = await oAuthConnector.MakeAssistedConnectionRequest(connection);

        model.State = request.Request?.State;
        model.ErrorCode = request.ErrorCode;
        model.ErrorMessage = request.ErrorMessage;

        return View(model);
    }

    [HttpGet, Route("manual-connect")]
    public IActionResult ManualConnect() => View(new ManualConnectModel());

    [HttpPost, Route("manual-connect")]
    public IActionResult ManualConnect(ManualConnectModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var connectionString = oAuthConnector.GenerateManualConnectionString(model.ClientId, model.RequestingPartyId);

        model.ConnectionString = connectionString;

        return View(model);
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
}