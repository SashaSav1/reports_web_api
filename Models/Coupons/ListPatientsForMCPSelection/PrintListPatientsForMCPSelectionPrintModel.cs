using reports_web_api.Domain;

namespace reports_web_api.Models.Coupons.ListPatientsForMCPSelection
{
    public class PrintListPatientsForMCPSelectionPrintModel
    {
        public User Doctor {  get; set; }
        public List<GroupedListPatientsForMCPSelection> GroupedListPatientsForMCPSelection { get; set; }
    }
}
