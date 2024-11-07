using Logtrade.Iol.Examples.OAuth.Core.Models.Iol;

namespace Logtrade.Iol.Examples.OAuth.Core.Models;
public class ConnectionRequestResult
{
    public bool Succeeded { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public ConnectionRequest? Request { get; set; }
}
