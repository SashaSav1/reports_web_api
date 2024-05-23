using reports_web_api.Domain;

namespace reports_web_api.Service
{
    public class DoctorCallService
    {
        public List<User> users = new List<User>();
        public List<Patient> patients = new List<Patient>();
        public List<DoctorCall> doctorCalls = new List<DoctorCall>();

        private Random random = new Random();

        public DoctorCallService()
        {
            for (int i = 0; i < 100; i++)
            {
                users.Add(new User
                {
                    Id = i + 1,
                    UserId = Guid.NewGuid(),
                    UserFio = $"User {i + 1}",
                    PostId = random.Next(1, 10),
                    Post = $"Post {random.Next(1, 10)}"
                });
            }

            DateOnly startDate = new DateOnly(1950, 1, 1);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Today);
            int rangeInDays = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days;

            for (int i = 0; i < 100; i++)
            {
                patients.Add(new Patient
                {
                    Id = i + 1,
                    ActualId = random.Next(1, 10000),
                    PatientFio = $"Patient {i + 1}",
                    BirtDate = startDate.AddDays(random.Next(rangeInDays)),
                    Address = $"Address {random.Next(1, 100)}",
                    CardNumber = $"Card {random.Next(1000, 9999)}"
                });
            }

            for (int i = 0; i < 1000; i++)
            {
                var patient = patients[random.Next(patients.Count)];
                var transmittedUser = random.Next(0, 2) == 0 ? null : users[random.Next(users.Count)];
                var doctorUser = random.Next(0, 2) == 0 ? null : users[random.Next(users.Count)];
                var IdCallType = random.Next(1, 10);
                var IdStatus = random.Next(1, 5);

                doctorCalls.Add(new DoctorCall
                {
                    Id = i + 1,
                    ActualId = random.Next(1, 10000),
                    IdMedicArea = random.Next(1, 10),
                    MedicArea = $"MedicArea {random.Next(1, 10)}",
                    CallDate = DateTime.Now.AddDays(-random.Next(0, 100)),
                    IdCallType = IdCallType,
                    CallType = $"CallType {IdCallType}",
                    Addresss = $"Address {random.Next(1, 100)}",
                    FrontDoor = random.Next(0, 2) == 0 ? (int?)null : random.Next(1, 10),
                    Floor = random.Next(0, 2) == 0 ? (int?)null : random.Next(1, 10),
                    PhoneNumber = $"Phone {random.Next(100000, 999999)}",
                    MobileNumber = $"Mobile {random.Next(100000, 999999)}",
                    FrontDoorCode = random.Next(0, 2) == 0 ? null : $"Code {random.Next(1000, 9999)}",
                    Reason = $"Reason {random.Next(1, 10)}",
                    Note = $"Note {random.Next(1, 10)}",
                    Diagnosis = random.Next(0, 2) == 0 ? null : $"Diagnosis {random.Next(1, 10)}",
                    Result = random.Next(0, 2) == 0 ? null : $"Result {random.Next(1, 10)}",
                    CompetionDate = random.Next(0, 2) == 0 ? (DateTime?)null : DateTime.Now.AddDays(-random.Next(0, 100)),
                    TransmitDate = random.Next(0, 2) == 0 ? (DateTime?)null : DateTime.Now.AddDays(-random.Next(0, 100)),
                    CancelDate = random.Next(0, 2) == 0 ? (DateTime?)null : DateTime.Now.AddDays(-random.Next(0, 100)),
                    IdDoctorMedicArea = random.Next(0, 2) == 0 ? (int?)null : random.Next(1, 10),
                    DoctorMedicArea = random.Next(0, 2) == 0 ? null : $"DoctorMedicArea {random.Next(1, 10)}",
                    IdPatient = patient.Id,
                    IdTransmittedUser = transmittedUser?.Id,
                    IdDoctorUser = doctorUser?.Id,
                    IdStatus = IdStatus,
                    Status = $"Status {IdStatus}",
                    Patient = patient,
                    TransmittedUser = transmittedUser,
                    DoctorUser = doctorUser
                });
            }
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public List<Patient> GetAllPatients()
        {
            return patients;
        }

        public List<DoctorCall> GetAllDoctorCalls()
        {
            return doctorCalls;
        }
    }
}
