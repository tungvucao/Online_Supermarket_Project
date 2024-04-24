using Online_Supermarket_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Online_Supermarket_Project.Repository;
namespace Online_Supermarket_Project.ViewComponents
{
    public class LoaiSpMenuViewComponent : ViewComponent
    {
        private readonly ILoaiSpRepository _loaiSp;
        public LoaiSpMenuViewComponent(ILoaiSpRepository loaiSpRepository)
        {
            _loaiSp = loaiSpRepository;
        }
        public IViewComponentResult Invoke()
        {
            var loaisp = _loaiSp.GetAllLoaiSp().OrderBy(x => x.CateName);
            return View(loaisp);
        }
    }
}
