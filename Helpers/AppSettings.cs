namespace AuthorizationService.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int InvalidationMinutes { get; set; }
    }
}