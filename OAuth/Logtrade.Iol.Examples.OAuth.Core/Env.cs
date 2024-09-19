namespace Logtrade.Iol.Examples.OAuth.Core
{
    public static class Env
    {
        public static Settings Settings { get; set; } = new Settings();
    }

    public class Settings
    {
        public string IolDomain { get; set; } = "https://iol.logtrade.org";
    }
}
