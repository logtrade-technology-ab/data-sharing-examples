namespace Logtrade.Iol.Examples.OAuth.Core.Models.ExampleRepository;
public class ConnectionRecord
{
    public int ConnectionRecordId { get; set; } = 0;
    public string State { get; set; } = "";
    public ConnectionType ConnectionType { get; set; }
    public string ConnectionState { get; set; } = "connection_pending";
    public string IolOrganisationIdentity { get; set; } = "";
    public DateTimeOffset Started { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? Completed { get; set; }
}
