namespace Logtrade.Iol.Examples.OAuth.Core.Models;

public class ConnectionRequestModel
{
    /// <summary>
    /// The client id of your application
    /// </summary>
    public string ClientId { get; set; } = "";
    /// <summary>
    /// A random, unique string to identify each request (e.g. a GUID)
    /// </summary>
    public string State { get; set; } = "";
    /// <summary>
    /// The (optional) name of the connection you want to appear in the client's connection list
    /// </summary>
    public string ConnectionName { get; set; } = "";
    /// <summary>
    /// The Iol Organisation Identity of the requesting party
    /// For a full explanation see 
    /// </summary>
    /// 
    public string? RequestingPartyId { get; set; } = null;
    public string ConnectToId { get; set; } = "";
}
