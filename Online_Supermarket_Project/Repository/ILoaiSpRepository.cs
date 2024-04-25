using Microsoft.AspNetCore.Mvc;
using Online_Supermarket_Project.Models;
namespace Online_Supermarket_Project.Repository
{
    public interface ILoaiSpRepository
    {
        Category Add(Category loaiSp);
        Category Update(Category loaiSp);
        Category Delete(String maloaiSp);
        Category GetLoaiSp(String maloaiSp);
        IEnumerable<Category> GetAllLoaiSp();
    }
}