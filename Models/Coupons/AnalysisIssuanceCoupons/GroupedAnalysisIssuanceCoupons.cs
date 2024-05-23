using reports_web_api.Domain;

namespace reports_web_api.Models.Coupons.AnalysisIssuanceCoupons
{
    public class GroupedAnalysisIssuanceCoupons
    {
        public User Doctor { get; set; }
        public List<AnalysisIssuanceCoupons> AnalysisIssuanceCoupons { get; set; }
    }
}
