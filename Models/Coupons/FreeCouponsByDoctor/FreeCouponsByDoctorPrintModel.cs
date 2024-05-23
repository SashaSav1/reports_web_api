using reports_web_api.Domain;

namespace reports_web_api.Models.Coupons.FreeCouponsByDoctor
{
    public class FreeCouponsByDoctorPrintModel
    {
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public User Doctor { get; set; }
        public List<GroupedFreeCouponsByDoctor> GroupedFreeCouponsByDoctor { get; set; }
    }
}
