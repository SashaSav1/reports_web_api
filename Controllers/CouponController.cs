using FastReport.Export.Pdf;
using FastReport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using reports_web_api.Domain;
using reports_web_api.Service;
using System.Xml.Linq;
using reports_web_api.Models.Coupons.FreeCoupons;
using reports_web_api.Models.Coupons.ListPatientsForMCPSelection;
using reports_web_api.Models.Coupons.AnalysisIssuanceCoupons;
using System.Linq;
using reports_web_api.Models.Coupons.ListOrderedPatientCoupons;
using reports_web_api.Models.Coupons.FreeCouponsByDoctor;

namespace reports_web_api.Controllers
{
    /// <summary>
    ///  Контроллер для печати всех шаблонов талонов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        CouponService _couponService = new CouponService();

        /// <summary>
        ///  Справка о наличии свободных талонов в разрезе специальностей медицинских работников
        /// </summary>
        [HttpGet("printFreeCoupons")]
        public async Task<ActionResult> PrintFreeCoupons(
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] Guid[] departmentId,
            [FromQuery] int[] postId,
            [FromQuery] int[] statusId,
            [FromQuery] int[] doctorId)
        {
            #region process reportData
            DateOnly DateFrom = dateFrom.HasValue ? DateOnly.FromDateTime(dateFrom.Value) : new DateOnly(2000, 1, 1);
            DateOnly DateTo = dateTo.HasValue ? DateOnly.FromDateTime(dateTo.Value) : DateOnly.FromDateTime(DateTime.Today);

            var Coupons = _couponService.GetAllCoupons()
                .Where(m => m.Date >= DateFrom && m.Date <= DateTo)
                .Where(m => departmentId.Length == 0 || departmentId.Contains(m.IdDepartment))
                .Where(m => postId.Length == 0 || postId.Contains(m.Doctor.PostId))
                .Where(m => doctorId.Length == 0 || doctorId.Contains(m.IdDoctor))
                .Where(m => statusId.Length == 0 || statusId.Contains(m.IdStatus))
                .OrderBy(m => m.Date)
                .ThenBy(m => m.Department)
                .ToList();

            var groupedData = Coupons
                .GroupBy(m => new { m.Date, m.DiagnosticsType, m.Doctor.Post })
                .Select(group => new GroupedFreeCoupons
                {
                    Post = group.Key.Post,
                    Date = group.Key.Date,
                    TypeResearch = group.Key.DiagnosticsType,
                    Amount = group.Count() 
                })
                .ToList();

            var reportData = new FreeCouponsPrintModel
            {
                DateFrom = DateFrom,
                DateTo = DateTo,
                CouponsType = string.Join(", ", Coupons
                .Where(m => statusId.Contains(m.IdStatus))
                .Select(m => m.Status)
                .Distinct()),
            };
            #endregion

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "fr", "FreeCoupons.frx");

            using (var report = new Report())
            {
                report.Load(reportPath);
                report.Dictionary.RegisterBusinessObject(new[] { reportData }, "ReportData", 10, true);
                report.Dictionary.RegisterBusinessObject(groupedData, "MatrixGroupedFreeCoupons", 10, true);
                report.Report.Save(reportPath);
                report.Prepare();

                var stream = new MemoryStream();

                using (var pdfExport = new PDFExport())
                {
                    pdfExport.Export(report, stream);
                    await stream.FlushAsync();
                }

                stream.Position = 0; // Reset the position to the beginning of the stream

                var fileName = "FreeCoupons.pdf";

                return File(stream, "application/pdf", fileName);

            }
        }

        /// <summary>
        ///  Список пациентов для подбора МКП, записавшихся на приём или вызвавших врача на дом
        /// </summary>
        [HttpGet("printListPatientsForMCPSelection")] //В параметры должен передоваться тип посещения, но в моделе его нет. Так же в моделе нет номера карты
        public async Task<ActionResult> PrintListPatientsForMCPSelection(
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] Guid[] departmentId,
            [FromQuery] int[] DoctorId)
        {
            #region process reportData
            DateOnly DateFrom = dateFrom.HasValue ? DateOnly.FromDateTime(dateFrom.Value) : new DateOnly(2000, 1, 1);
            DateOnly DateTo = dateTo.HasValue ? DateOnly.FromDateTime(dateTo.Value) : DateOnly.FromDateTime(DateTime.Today);

            var Coupons = _couponService.GetAllCoupons()
                .Where(m => m.Date >= DateFrom && m.Date <= DateTo)
                .Where(m => departmentId.Length == 0 || departmentId.Contains(m.IdDepartment))
                .Where(m => DoctorId.Length == 0 || DoctorId.Contains(m.IdDoctor))
                .Where(m => m.Patient != null)
                //.Where(m => VisitTypeId.Length == 0 || couponTypeId.Contains(m.VisitTypeId)) // Фильтрация по типу посещения
                .OrderBy(m => m.Date)
                .ThenBy(m => m.IdDoctor)
                .ToList();

            int SerialNumber = 1;

            var reportData = Coupons
                .GroupBy(m => m.Doctor)
                .Select(group => new PrintListPatientsForMCPSelectionPrintModel
                {
                    Doctor = group.Key,
                    GroupedListPatientsForMCPSelection = group
                        .OrderBy(m => m.TimeStart)
                        .Select((coupon, index) => new GroupedListPatientsForMCPSelection
                        {
                            Number = SerialNumber++,
                            TimeStart = coupon.TimeStart.ToString("HH:mm"),
                            NumberCard = 1,
                            Patient = coupon.Patient,
                            Note = coupon.Note
                            
                        })
                        .ToList()
                })
                .ToList();

            var rangeDates = new RangeDates
            {
                DateFrom = DateFrom,
                DateTo = DateTo,
            };
            #endregion

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "fr", "ListPatientsForMCPSelection.frx");

            using (var report = new Report())
            {
                report.Load(reportPath);
                report.Dictionary.RegisterBusinessObject(reportData, "ReportData", 10, true);
                report.Dictionary.RegisterBusinessObject(new[] { rangeDates }, "rangeDates", 10, true);
                report.Report.Save(reportPath);
                report.Prepare();

                var stream = new MemoryStream();

                using (var pdfExport = new PDFExport())
                {
                    pdfExport.Export(report, stream);
                    await stream.FlushAsync();
                }

                stream.Position = 0; // Reset the position to the beginning of the stream

                var fileName = "ListPatientsForMCPSelection.pdf";

                return File(stream, "application/pdf", fileName);

            }
        }

        /// <summary>
        ///  Анализ выдачи талонов
        /// </summary>
        [HttpGet("printAnalysisIssuanceCoupons")]
        public async Task<ActionResult> PrintAnalysisIssuanceCoupons(
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] Guid[] departmentId,
            [FromQuery] int[] doctorId)
        {
            #region process reportData
            DateOnly DateFrom = dateFrom.HasValue ? DateOnly.FromDateTime(dateFrom.Value) : new DateOnly(2000, 1, 1);
            DateOnly DateTo = dateTo.HasValue ? DateOnly.FromDateTime(dateTo.Value) : DateOnly.FromDateTime(DateTime.Today);

            var cancellations = _couponService.GetAllCouponCancellations();

            var Coupons = _couponService.GetAllCoupons()
                .Where(m => m.Date >= DateFrom && m.Date <= DateTo)
                .Where(m => departmentId.Length == 0 || departmentId.Contains(m.IdDepartment))
                .Where(m => doctorId.Length == 0 || doctorId.Contains(m.IdDoctor))
                .Where(m => m.IdUserGiven != null)
                .OrderBy(m => m.Date)
                .ThenBy(m => m.IdDoctor)
                .ToList();

            var groupedData = Coupons
                .GroupBy(m => m.Doctor)
                .Select(doctorGroup => new GroupedAnalysisIssuanceCoupons
                {
                    Doctor = doctorGroup.Key,
                    AnalysisIssuanceCoupons = doctorGroup
                        .GroupBy(coupon => new { coupon.Date, coupon.UserGiven })
                        .Select(dateGroup => new AnalysisIssuanceCoupons
                        {
                            UserGiven = dateGroup.Key.UserGiven,
                            Date = dateGroup.Key.Date,
                            Amount = dateGroup.Count(), // Общее количество купонов в каждой группе
                            AmountGiven = dateGroup.Count(c => c.DateGiven.HasValue), // Количество купонов с непустым DateGiven
                            AmountCancellation = dateGroup.Count(c => cancellations.Any(cancel => cancel.CouponId == c.Id))
                        })
                        .ToList()
                })
                .ToList();

            var reportData = new AnalysisIssuanceCouponsPrintModel
            {
                DateFrom = DateFrom,
                DateTo = DateTo,
                GroupedAnalysisIssuanceCoupons = groupedData
            };
            #endregion

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "fr", "AnalysisIssuanceCoupons.frx");

            using (var report = new Report())
            {
                report.Load(reportPath);
                report.Dictionary.RegisterBusinessObject(new[] { reportData }, "ReportData", 10, true);
                report.Report.Save(reportPath);
                report.Prepare();

                var stream = new MemoryStream();

                using (var pdfExport = new PDFExport())
                {
                    pdfExport.Export(report, stream);
                    await stream.FlushAsync();
                }

                stream.Position = 0; // Reset the position to the beginning of the stream

                var fileName = "AnalysisIssuanceCoupons.pdf";

                return File(stream, "application/pdf", fileName);

            }
        }

        /// <summary>
        ///  Просмотр списка заказанных талонов пациента, с учётом отмены ранее заказанных талонов
        /// </summary>
        [HttpGet("printListOrderedPatientCoupons")]
        public async Task<ActionResult> PrintListOrderedPatientCoupons(
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] int patientId)
        {
            #region process reportData
            DateOnly DateFrom = dateFrom.HasValue ? DateOnly.FromDateTime(dateFrom.Value) : new DateOnly(2000, 1, 1);
            DateOnly DateTo = dateTo.HasValue ? DateOnly.FromDateTime(dateTo.Value) : DateOnly.FromDateTime(DateTime.Today);

            var Coupons = _couponService.GetAllCoupons()
                .Where(m => m.Date >= DateFrom && m.Date <= DateTo)
                .Where(m => m.IdPatient == patientId)
                .OrderBy(m => m.Date)
                .ThenBy(m => m.TimeStart)
                .ToList();

            var reportData = Coupons
                .GroupBy(m => m.Patient)
                .Select(patientGroup => new ListOrderedPatientCouponsPrintModel
                {
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    Patient = patientGroup.Key,
                    ListOrderedPatientCoupons = patientGroup
                        .OrderBy(coupon => coupon.Date)
                        .ThenBy(coupon => coupon.TimeStart)
                        .Select(coupon => new ListOrderedPatientCoupons
                        {
                            Date = coupon.Date,
                            TimeStart = coupon.TimeStart,
                            Doctor = coupon.Doctor,
                            isGivenCoupon = coupon.DateGiven.HasValue,
                            isExaminationPerformed = false
                        })
                        .ToList()
                })
                .ToList();
            #endregion

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "fr", "ListOrderedPatientCoupons.frx");

            using (var report = new Report())
            {
                report.Load(reportPath);
                report.Dictionary.RegisterBusinessObject(reportData, "ReportData", 10, true);
                report.Report.Save(reportPath);
                report.Prepare();

                var stream = new MemoryStream();

                using (var pdfExport = new PDFExport())
                {
                    pdfExport.Export(report, stream);
                    await stream.FlushAsync();
                }

                stream.Position = 0; // Reset the position to the beginning of the stream

                var fileName = "ListOrderedPatientCoupons.pdf";

                return File(stream, "application/pdf", fileName);

            }
        }

        /// <summary>
        ///  Формирование сводного отчёта об оставшихся свободных талонах.
        /// </summary>
        [HttpGet("printFreeCouponsByDoctor")]
        public async Task<ActionResult> PrintFreeCouponsByDoctor(
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] int doctorId,
            [FromQuery] int[] statusId)
        {
            #region process reportData
            DateOnly DateFrom = dateFrom.HasValue ? DateOnly.FromDateTime(dateFrom.Value) : new DateOnly(2000, 1, 1);
            DateOnly DateTo = dateTo.HasValue ? DateOnly.FromDateTime(dateTo.Value) : DateOnly.FromDateTime(DateTime.Today);

            var Coupons = _couponService.GetAllCoupons()
                .Where(m => m.Date >= DateFrom && m.Date <= DateTo)
                .Where(m => m.IdDoctor == doctorId)
                .Where(m => statusId.Length == 0 || statusId.Contains(m.IdStatus))
                .OrderBy(m => m.Date)
                .ThenBy(m => m.TimeStart)
                .ToList();

            var reportData = Coupons
                .GroupBy(m => m.Doctor)
                .Select(doctorGroup => new FreeCouponsByDoctorPrintModel
                {
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    Doctor = doctorGroup.Key,
                    GroupedFreeCouponsByDoctor = doctorGroup
                        .GroupBy(coupon => coupon.Date)
                        .Select(dateGroup => new GroupedFreeCouponsByDoctor
                        {
                            Date = dateGroup.Key,
                            freeCouponsByDoctors = dateGroup
                                .Select(coupon => new FreeCouponsByDoctor
                                {
                                    ActualId = coupon.ActualId,
                                    CouponNumber = coupon.CouponNumber,
                                    TimeStart = coupon.Date.ToString() + " " + coupon.TimeStart.ToString(),
                                    WhereOrder = "",
                                    DateOrdered = coupon.DateOrdered,
                                    UserOrdered = coupon.UserOrdered,
                                    DateGiven = coupon.DateGiven,
                                    UserGiven = coupon.UserGiven
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();
            #endregion

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "fr", "FreeCouponsByDoctor.frx");

            using (var report = new Report())
            {
                report.Load(reportPath);
                report.Dictionary.RegisterBusinessObject(reportData, "ReportData", 10, true);
                report.Report.Save(reportPath);
                report.Prepare();

                var stream = new MemoryStream();

                using (var pdfExport = new PDFExport())
                {
                    pdfExport.Export(report, stream);
                    await stream.FlushAsync();
                }

                stream.Position = 0; // Reset the position to the beginning of the stream

                var fileName = "FreeCouponsByDoctor.pdf";

                return File(stream, "application/pdf", fileName);

            }
        }

    }
}
