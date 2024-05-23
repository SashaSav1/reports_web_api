using reports_web_api.Domain;

namespace reports_web_api.Models.Coupons.AnalysisIssuanceCoupons
{
    public class AnalysisIssuanceCouponsPrintModel
    {
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public List<GroupedAnalysisIssuanceCoupons> GroupedAnalysisIssuanceCoupons { get; set; }
    }
}
