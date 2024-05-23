using reports_web_api.Domain;

namespace reports_web_api.Models.Coupons.AnalysisIssuanceCoupons
{
    public class AnalysisIssuanceCoupons
    {
        public User? UserGiven { get; set; }
        public DateOnly Date { get; set; }
        public int Amount { get; set; }
        public int AmountGiven { get; set; }
        public int AmountCancellation { get; set; }
    }
}
