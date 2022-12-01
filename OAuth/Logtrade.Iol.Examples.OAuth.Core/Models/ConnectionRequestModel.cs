namespace Logtrade.Iol.Examples.OAuth.Core.Models
{
    public class ConnectionRequestModel
    {
        /// <summary>
        /// The client id of your application
        /// </summary>
        public string ClientId { get; set; } = "";
        /// <summary>
        /// A random, unique string to identify each request (e.g. GUID)
        /// </summary>
        public string State { get; set; } = "";
        /// <summary>
        /// The name of the connection you want to appear in the client's connection list
        /// </summary>
        public string ConnectionName { get; set; } = "";
        /// <summary>
        /// The Iol Account id of the requesting party if you're conencting two IoL organisations together
        /// </summary>
        /// 
        public string RequestingPartyId { get; set; } = "";
        /// <summary>
        /// A list of scopes, use simple strings or ScopeObject
        /// </summary>
        public List<string> ScopeList { get; set; } = new List<string>();
        /// <summary>
        /// A list of scopes, use this for adding filters and exclusions to the data
        /// </summary>
        public List<ScopeModel> ScopeObjects { get; set; } = new List<ScopeModel>();
    }

    public class ScopeModel
    {
        public string Scope { get; set; } = "";
        public List<KeyValuePair<string, string>> Filtered { get; set; } = new List<KeyValuePair<string, string>>();
        public List<KeyValuePair<string, string>> Excluded { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
