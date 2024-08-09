using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SanGoStore.Models;

namespace SanGoStore.Controllers
{
    public class HomeController : Controller
    {
        // Data context for SanGoStore
        dbSanGoStoreDataContext data = new dbSanGoStoreDataContext();

        // Method to retrieve a list of products
        private List<SanPham> LaySanPham(int count)
        {
            return data.SanPhams.OrderByDescending(sp => sp.MaSP).Take(count).ToList();
        }

        // GET: Home/Index
        public ActionResult Index()
        {
            var sanpham = LaySanPham(8); // Retrieve top 8 products
            return View(sanpham);
        }

        // GET: Home/DanhMucSanPham
        public ActionResult DanhMucSanPham()
        {
            var loai = from l in data.Loais select l; // Retrieve product categories
            return PartialView(loai);
        }

        // GET: Home/Details
        public ActionResult Details(int id)
        {
            var sanpham = from sp in data.SanPhams where sp.MaSP == id select sp;
            return View(sanpham.Single());
        }
    }
}