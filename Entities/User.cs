namespace AuthorizationService.Entities
{
    public class User
    {
        public int accountId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string token { get; set; }
    }
}