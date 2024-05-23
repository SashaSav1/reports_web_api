namespace reports_web_api.Domain
{
    public class CouponCancellation
    {
        public int Id { get; set; }
        public Guid IdOrganization { get; set; }
        public int CouponId { get; set; }
        public DateTime CancellationDate { get; set; }
        public int IdPatient { get; set; }
        public Patient? Patient { get; set; }
    }
}
