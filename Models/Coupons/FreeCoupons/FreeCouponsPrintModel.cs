namespace reports_web_api.Models.Coupons.FreeCoupons
{
    public class FreeCouponsPrintModel
    {
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public string CouponsType { get; set; }
    }
}
