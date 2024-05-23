using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace reports_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallingMedicalProfessionalController : ControllerBase
    {
        //[HttpGet("callingMedicalProfessional")]
        //public async Task<ActionResult> CallingMedicalProfessional(DateTime? DateFrom, DateTime? DateTo, bool? isToday, bool? isPreviousMonth,
        //    bool? isCurrentMonth, int[]? Quarter, bool? isFirstYear, bool? isSecondYear, bool? isNineMonth, bool? isYear, )
        //{
        //    var reportData = _couponService.GetCoupon();

        //    var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "fr", "Coupon.frx");

        //    using (var report = new Report())
        //    {
        //        report.Load(reportPath);
        //        report.Dictionary.RegisterBusinessObject(reportData, "ReportData", 10, true);
        //        report.Report.Save(reportPath);
        //        report.Prepare();

        //        var stream = new MemoryStream();

        //        using (var pdfExport = new PDFExport())
        //        {
        //            pdfExport.Export(report, stream);
        //            await stream.FlushAsync();
        //        }

        //        stream.Position = 0; // Reset the position to the beginning of the stream

        //        var fileName = "Coupon.pdf";

        //        return File(stream, "application/pdf", fileName);
        //    }
        //}

    }
}
