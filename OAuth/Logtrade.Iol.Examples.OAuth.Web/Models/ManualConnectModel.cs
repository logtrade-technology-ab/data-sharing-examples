using System.ComponentModel.DataAnnotations;

namespace Logtrade.Iol.Examples.OAuth.Web.Models;

public class ManualConnectModel
{
    [Required]
    public string ClientId { get; set; } = "";
    public string? RequestingPartyId { get; set; }

    public string? State { get; set; }
    public string? ConnectionString { get; set; }
}
