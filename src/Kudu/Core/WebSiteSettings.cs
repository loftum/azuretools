namespace Kudu.Core
{
    public class WebSiteSettings
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public string GetUserName() => $"${Name}";
        public string GetApiBase() => $"https://{Name}.scm.azurewebsites.net/api";

        public override string ToString()
        {
            return Name;
        }
    }
}