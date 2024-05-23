namespace reports_web_api.Models.Coupons.FreeCouponsByDoctor
{
    public class GroupedFreeCouponsByDoctor
    {
        public DateOnly Date {  get; set; }
        public List<FreeCouponsByDoctor> freeCouponsByDoctors {  get; set; }
    }
}
