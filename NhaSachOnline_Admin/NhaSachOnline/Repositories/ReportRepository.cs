using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NhaSachOnline.Data;
using NhaSachOnline.Models;

namespace NhaSachOnline.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<Book>> LayThongTinBanTheoNgay(DateTime ngayBatDau, DateTime ngayKetThuc);
    }
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ResultBookModel>> LayThongTinBanTheoNgay(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            var ngaybatDau = new SqlParameter("@ngayBatDau", ngayBatDau);
            var ngayketThuc = new SqlParameter("@ngayKetThuc", ngayKetThuc);
            var result = await _dbContext.Database.SqlQueryRaw<ResultBookModel>
                ("exec Usp_GetResultBookByDate @ngaybatDau, @ngayketThuc", ngayBatDau, ngayKetThuc).ToListAsync();
            return result;
        }

        Task<IEnumerable<Book>> IReportRepository.LayThongTinBanTheoNgay(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            throw new NotImplementedException();
        }
    }
}
