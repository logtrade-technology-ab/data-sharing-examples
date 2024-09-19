using Logtrade.Iol.Examples.OAuth.Core.Models.Iol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logtrade.Iol.Examples.OAuth.Core.Models;
public class ConnectionRequestResult
{
    public bool Succeeded { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public ConnectionRequest? Request { get; set; }
}
