namespace reports_web_api.Domain
{
    public class Patient
    {
        public int Id { get; set; }
        public int ActualId { get; set; }
        public string? PatientFio { get; set; }
        public DateOnly? BirtDate { get; set; }
        public string? Address { get; set; }
        public string? CardNumber { get; set; }
    }
}
