using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ChapterVerseUI.Repositories
{
    public class ReportRepo : IReportRepo
    {
        private readonly ApplicationDbContext _context;

        public ReportRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TopNSoldBookModel>> GetTopNSellingBooksByDate(DateTime startDate, DateTime endDate)
        {
            var startDateParam = new SqlParameter("@startDate", startDate);
            var endDateParam = new SqlParameter("@endDate", endDate);
            var topFiveSoldBooks = await _context.Database.SqlQueryRaw<TopNSoldBookModel>("exec Usp_GetTopNSellingBooksByDate @startDate,@endDate", startDateParam, endDateParam).ToListAsync();
            return topFiveSoldBooks;
        }
    }

    public interface IReportRepo
    {
        Task<IEnumerable<TopNSoldBookModel>> GetTopNSellingBooksByDate(DateTime startDate, DateTime endDate);
    }
}
