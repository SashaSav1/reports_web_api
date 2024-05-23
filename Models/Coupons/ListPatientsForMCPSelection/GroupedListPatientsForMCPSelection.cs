using reports_web_api.Domain;

namespace reports_web_api.Models.Coupons.ListPatientsForMCPSelection
{
    public class GroupedListPatientsForMCPSelection
    {
        public int Number { get; set; }
        public string TimeStart { get; set; }
        public int NumberCard { get; set; }
        public Patient? Patient { get; set; }
        public string Note { get; set; }

    }
}
