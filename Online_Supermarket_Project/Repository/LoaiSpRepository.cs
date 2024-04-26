using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;

namespace Online_Supermarket_Project.Repository
{
    public class LoaiSpRepository : ILoaiSpRepository
    {
        private readonly MyAppDbContext _appDbContext;
        public LoaiSpRepository(MyAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Category Add(Category loaiSp)
        {
            _appDbContext.Category.Add(loaiSp);
            _appDbContext.SaveChanges();
            return loaiSp;
        }

        public Category Delete(string maloaiSp)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetAllLoaiSp()
        {
            return _appDbContext.Category;
        }

        public Category GetLoaiSp(string maloaiSp)
        {
            return _appDbContext.Category.Find(maloaiSp);
        }

        public Category Update(Category loaiSp)
        {
            _appDbContext.Update(loaiSp);
            _appDbContext.SaveChanges();
            return loaiSp;
        }
    }
}