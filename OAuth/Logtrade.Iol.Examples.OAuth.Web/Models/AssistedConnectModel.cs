using System.ComponentModel.DataAnnotations;

namespace Logtrade.Iol.Examples.OAuth.Web.Models;

public class AssistedConnectModel
{
    [Required]
    public string ClientId { get; set; } = "";
    [Required]
    public string ConnectToId { get; set; } = "";
    [Required]
    public string SysAdminToken { get; set; } = "";
    public string? RequestingPartyId { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? State { get; set; }
}
