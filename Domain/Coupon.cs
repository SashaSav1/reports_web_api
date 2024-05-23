namespace reports_web_api.Domain
{
    public class Coupon
    {
        public int Id { get; set; }
        public int ActualId { get; set; }
        public Guid IdOrganization { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly TimeStart { get; set; }
        public TimeOnly TimeEnd { get; set; }
        public required string CouponNumber { get; set; }
        public int IdCouponType { get; set; }
        public required string CouponType { get; set; }
        public int? IdDiagnosticsType { get; set; }
        public string? DiagnosticsType { get; set; }
        public int IdStatus { get; set; }
        public required string Status { get; set; }
        public string? Note { get; set; }
        public Guid IdDepartment { get; set; }
        public required string Department { get; set; }
        public int IdWhereOrdered { get; set; }
        public required string WhereOrdered { get; set; }

        /// <summary> 
        /// Id для навигации, не из оперативной базы 
        /// </summary> 
        public int IdDoctor { get; set; }
        /// <summary> 
        /// Id для навигации, не из оперативной базы 
        /// </summary> 
        public int? IdPatient { get; set; }
        /// <summary> 
        /// Id для навигации, не из оперативной базы 
        /// </summary> 
        public int? IdUserOrdered { get; set; }
        /// <summary> 
        /// Id для навигации, не из оперативной базы 
        /// </summary> 
        public int? IdUserGiven { get; set; }

        public DateTime? DateOrdered { get; set; }
        public DateTime? DateGiven { get; set; }
        public required User Doctor { get; set; }
        public Patient? Patient { get; set; }
        public User? UserOrdered { get; set; }
        public User? UserGiven { get; set; }
    }
}
