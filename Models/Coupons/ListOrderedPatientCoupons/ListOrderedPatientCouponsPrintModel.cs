using reports_web_api.Domain;

namespace reports_web_api.Models.Coupons.ListOrderedPatientCoupons
{
    public class ListOrderedPatientCouponsPrintModel
    {
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public Patient Patient { get; set; }
        public List<ListOrderedPatientCoupons> ListOrderedPatientCoupons { get; set; }
    }
}
