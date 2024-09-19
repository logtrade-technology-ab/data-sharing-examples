using System.ComponentModel.DataAnnotations;

namespace Logtrade.Iol.Examples.OAuth.Core.Models.Iol;

public class RequestConnection
{
    /// <summary>
    /// The ClientId of the application to connect
    /// </summary>
    [Required]
    public string ClientId { get; set; } = "";
    /// <summary>
    /// The IolOrganisationId of the account for the application to connect to
    /// </summary>
    [Required]
    public string ConnectToId { get; set; } = "";
    /// <summary>
    /// Optional IolOrganisationId that the application is connecting on behalf of
    /// </summary>
    public string OnBehalfOf { get; set; } = "";
    /// <summary>
    /// Optional state to help the application owner to track the request
    /// </summary>
    public string State { get; set; } = "";
}