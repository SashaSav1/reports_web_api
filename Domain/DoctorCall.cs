namespace reports_web_api.Domain
{
    public class DoctorCall
    {
        public int Id { get; set; }
        public int ActualId { get; set; }
        public int IdMedicArea { get; set; }
        public required string MedicArea { get; set; }
        public DateTime CallDate { get; set; }
        public int IdCallType { get; set; }
        public required string CallType { get; set; }
        public required string Addresss { get; set; }
        public int? FrontDoor { get; set; }
        public int? Floor { get; set; }
        public required string PhoneNumber { get; set; }
        public required string MobileNumber { get; set; }
        public string? FrontDoorCode { get; set; }
        public required string Reason { get; set; }
        public required string Note { get; set; }
        public string? Diagnosis { get; set; }
        public string? Result { get; set; }
        public DateTime? CompetionDate { get; set; }
        public DateTime? TransmitDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? IdDoctorMedicArea { get; set; }
        public string? DoctorMedicArea { get; set; }
        public int IdPatient { get; set; }
        public int? IdTransmittedUser { get; set; }
        public int? IdDoctorUser { get; set; }
        public int IdStatus { get; set; }
        public required string Status { get; set; }

        public required Patient Patient { get; set; }
        public required User? TransmittedUser { get; set; }
        public required User? DoctorUser { get; set; }
    }
}
