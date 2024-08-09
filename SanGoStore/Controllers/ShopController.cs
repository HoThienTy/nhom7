using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using SanGoStore.Models;
using PagedList;
using PagedList.Mvc;
namespace SanGoStore.Controllers
{
    public class ShopController : Controller
    {
        // Data context for SanGoStore
        dbSanGoStoreDataContext data = new dbSanGoStoreDataContext();

        // Retrieve a list of products (SanPham)
        private List<SanPham> LaySanPham(int count)
        {
            return data.SanPhams.OrderBy(sp => sp.MaSP).Take(count).ToList();
        }

        // Display products with pagination
        public ActionResult Index(string keyword, int? page)
        {
            int pageSize = 6;  // Number of products per page
            int pageNum = (page ?? 1);  // Default to the first page if no page number is specified

            // Query to filter products based on the search keyword
            var sanpham = data.SanPhams
            .Where(sp => string.IsNullOrEmpty(keyword) || sp.TenSP.Contains(keyword))
            .OrderBy(sp => sp.MaSP);

            ViewBag.Keyword = keyword;  // Pass the search keyword to the view

            return View(sanpham.ToPagedList(pageNum, pageSize));  // Use PagedList for pagination
        }

        // Display product categories
        public ActionResult LoaiSanPham()
        {
            var loaiSanPham = from lsp in data.Loais select lsp;  // Retrieve all categories
            return PartialView(loaiSanPham);
        }

        // Display products by category
        public ActionResult SanPhamTheoLoai(int id)
        {
            var sanpham = from sp in data.SanPhams where sp.MaLoai == id select sp;  // Retrieve products by category
            return View(sanpham);
        }

        // Display product details
        public ActionResult Details(int id)
        {
            var sanpham = from sp in data.SanPhams where sp.MaSP == id select sp;  // Retrieve a single product by ID
            return View(sanpham.Single());
        }

        
    }
}