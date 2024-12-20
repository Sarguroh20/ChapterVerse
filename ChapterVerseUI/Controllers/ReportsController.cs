using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChapterVerseUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ReportsController : Controller
    {
        private readonly IReportRepo _reportRepo;

        public ReportsController(IReportRepo reportRepo)
        {
            _reportRepo = reportRepo;
        }
        public async Task<ActionResult> TopFiveSellingBooks(DateTime? sDate = null, DateTime? eDate = null)
        {
            try
            {
                DateTime startDate = sDate ?? DateTime.UtcNow.AddDays(-7);
                DateTime endDate = eDate ?? DateTime.UtcNow;
                var topFiveSellingBooks = await _reportRepo.GetTopNSellingBooksByDate(startDate, endDate);
                var vm = new TopNSoldBooksVm(startDate, endDate, topFiveSellingBooks);
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Something went wrong";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
