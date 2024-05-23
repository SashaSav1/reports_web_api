using reports_web_api.Domain;
using System;

namespace reports_web_api.Service
{
    public class CouponService
    {
        public List<User> doctors;
        public List<Patient> patients;
        public List<User> users;
        public List<Coupon> coupons;
        public List<CouponCancellation> couponCancellations;
        private Random random;

        public CouponService()
        {
            random = new Random();
            InitializeDoctors();
            InitializePatients();
            InitializeUsers();
            InitializeCoupons();
            InitializeCouponCancellations();
        }

        private void InitializeDoctors()
        {
            doctors = new List<User>();

            for (int i = 0; i < 100; i++)
            {
                doctors.Add(new User
                {
                    Id = i + 1,
                    UserId = Guid.NewGuid(),
                    UserFio = $"Doctor {i + 1}",
                    PostId = random.Next(1, 10),
                    Post = $"Post {random.Next(1, 10)}"
                });
            }
        }

        private void InitializePatients()
        {
            patients = new List<Patient>();
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
        }

        private void InitializeUsers()
        {
            users = new List<User>();

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
        }

        private void InitializeCoupons()
        {
            coupons = new List<Coupon>();
            DateOnly startDate = new DateOnly(2024, 1, 1);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Today);
            int rangeInDays = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days;

            for (int i = 0; i < 10000; i++)
            {
                var doctor = doctors[random.Next(doctors.Count)];
                var patient = random.Next(0, 2) == 0 ? null : patients[random.Next(patients.Count)];
                var userOrdered = random.Next(0, 2) == 0 ? null : users[random.Next(users.Count)];
                var userGiven = random.Next(0, 2) == 0 ? null : users[random.Next(users.Count)];
                var IdCouponType = random.Next(1, 10);
                var IdDiagnosticsType = random.Next(0, 2) == 0 ? (int?)null : random.Next(1, 10);
                var IdStatus = random.Next(1, 5);
                var IdWhereOrdered = random.Next(1, 5);

                coupons.Add(new Coupon
                {
                    Id = i + 1,
                    ActualId = random.Next(1, 10000),
                    IdOrganization = Guid.NewGuid(),
                    Date = startDate.AddDays(random.Next(rangeInDays)),
                    TimeStart = new TimeOnly(random.Next(0, 24), random.Next(0, 60)),
                    TimeEnd = new TimeOnly(random.Next(0, 24), random.Next(0, 60)),
                    CouponNumber = $"CN{random.Next(1000, 9999)}",
                    IdCouponType = IdCouponType,
                    CouponType = $"Type {IdCouponType}",
                    IdDiagnosticsType = IdDiagnosticsType,
                    DiagnosticsType = IdDiagnosticsType != null ? $"Diagnostics {IdDiagnosticsType}" : null,
                    IdStatus = IdStatus,
                    Status = $"Status {IdStatus}",
                    Note = random.Next(0, 2) == 0 ? null : $"Note {random.Next(1, 100)}",
                    IdDepartment = Guid.NewGuid(),
                    Department = $"Department {random.Next(1, 10)}",
                    IdWhereOrdered = IdWhereOrdered,
                    WhereOrdered = $"Where {IdWhereOrdered}",
                    IdDoctor = doctor.Id,
                    IdPatient = patient?.Id,
                    IdUserOrdered = userOrdered?.Id,
                    IdUserGiven = userGiven?.Id,
                    DateOrdered = random.Next(0, 2) == 0 ? (DateTime?)null : DateTime.Now.AddDays(-random.Next(0, 100)),
                    DateGiven = random.Next(0, 2) == 0 ? (DateTime?)null : DateTime.Now.AddDays(-random.Next(0, 100)),
                    Doctor = doctor,
                    Patient = patient,
                    UserOrdered = userOrdered,
                    UserGiven = userGiven
                });
            }
        }

        private void InitializeCouponCancellations()
        {
            couponCancellations = new List<CouponCancellation>();

            DateTime startDate = new DateTime(2024, 1, 1);
            DateTime endDate = DateTime.Today;
            int rangeInDays = (endDate - startDate).Days;

            for (int i = 0; i < 4000; i++)
            {
                var patient = patients[random.Next(patients.Count)];

                couponCancellations.Add(new CouponCancellation
                {
                    Id = i + 1,
                    IdOrganization = Guid.NewGuid(),
                    CouponId = random.Next(1, 10000),
                    CancellationDate = startDate.AddDays(random.Next(rangeInDays)),
                    IdPatient = patient.Id,
                    Patient = patient
                });
            }
        }

        public List<Coupon> GetAllCoupons()
        {
            return coupons;
        }

        public List<CouponCancellation> GetAllCouponCancellations()
        {
            return couponCancellations;
        }

        public List<User> GetAllDoctors()
        {
            return doctors;
        }

        public List<Patient> GetAllPatients()
        {
            return patients;
        }

        public List<User> GetAllUsers()
        {
            return users;
        }
    }

}
