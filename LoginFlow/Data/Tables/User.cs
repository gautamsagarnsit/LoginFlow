namespace LoginFlow.Data.Tables
{
    public class User
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
