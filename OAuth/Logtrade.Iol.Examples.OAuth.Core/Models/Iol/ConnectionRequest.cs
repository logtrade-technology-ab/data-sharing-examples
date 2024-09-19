namespace Logtrade.Iol.Examples.OAuth.Core.Models.Iol;

public class ConnectionRequest
{
    public int ConnectionRequestId { get; set; }
    public string ApplicationClientId { get; set; } = null!;
    public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
    public string RequestedById { get; set; } = null!;
    public string? CompletedById { get; set; } = null;
    public DateTimeOffset? Completed { get; set; } = null;
    public string ConnectTo { get; set; } = null!;
    public string? OnBehalfOf { get; set; } = null;
    public string State { get; set; } = "";
    public ConnectionRequestState RequestState { get; set; } = ConnectionRequestState.Pending;
    public string ConnectionRequestKey { get; set; } = "";
}

public enum ConnectionRequestState
{
    Unknown,
    Pending,
    Accepted,
    Rejected
}
