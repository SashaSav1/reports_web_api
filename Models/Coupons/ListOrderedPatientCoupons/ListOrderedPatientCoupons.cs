using reports_web_api.Domain;

namespace reports_web_api.Models.Coupons.ListOrderedPatientCoupons
{
    public class ListOrderedPatientCoupons
    {
        public DateOnly Date { get; set; }
        public TimeOnly TimeStart { get; set; }
        public User Doctor { get; set; }
        public bool isGivenCoupon { get; set; }
        public bool isExaminationPerformed { get; set; }
    }
}
