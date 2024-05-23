using reports_web_api.Domain;

namespace reports_web_api.Models.Coupons.FreeCouponsByDoctor
{
    public class FreeCouponsByDoctor
    {
        public int ActualId { get; set; }
        public string CouponNumber { get; set; }
        public string TimeStart { get; set; }
        public string WhereOrder { get; set; }
        public DateTime? DateOrdered { get; set; }
        public User? UserOrdered { get; set; }
        public DateTime? DateGiven { get; set; }
        public User? UserGiven { get; set; }
    }
}
