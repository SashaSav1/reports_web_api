namespace reports_web_api.Domain
{
    public class User
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public required string UserFio { get; set; }
        public int PostId { get; set; }
        public required string Post { get; set; }
    }
}
