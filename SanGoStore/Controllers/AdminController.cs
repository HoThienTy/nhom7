using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SanGoStore.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace SanGoStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbSanGoStoreDataContext db = new dbSanGoStoreDataContext();

        public ActionResult Index()
        {
            return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["TaiKhoanAdmin"] = ad;
                    return RedirectToAction("DonDatHang", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult SanPham(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.SanPhams.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ThemSanPham()
        {
            ViewBag.MaLoai = new SelectList(db.Loais.OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSanPham(SanPham sanpham, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(db.Loais.OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh đại diện";
                return View();
            }

            if (ModelState.IsValid)
            {
                var filename = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/Assets/Images/"), filename);
                if (!System.IO.File.Exists(path))
                {
                    fileupload.SaveAs(path);
                }
                sanpham.AnhDD = filename;
                db.SanPhams.InsertOnSubmit(sanpham);
                db.SubmitChanges();
                return RedirectToAction("SanPham");
            }
            return View();
        }

        public ActionResult ChiTietSanPham(int id)
        {
            var sanpham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        public ActionResult XoaSanPham(int id)
        {
            var sanpham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        [HttpPost, ActionName("XoaSanPham")]
        public ActionResult XacNhanXoa(int id)
        {
            var sanpham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SanPhams.DeleteOnSubmit(sanpham);
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }

        public ActionResult SuaSanPham(int id)
        {
            var sanpham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoai = new SelectList(db.Loais.OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sanpham.MaLoai);
            return View(sanpham);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSanPham(SanPham sanpham)
        {
            var item = db.SanPhams.SingleOrDefault(n => n.MaSP == sanpham.MaSP);
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            item.TenSP = sanpham.TenSP;
            item.GiaBan = sanpham.GiaBan;
            item.NoiDung = sanpham.NoiDung;
            item.SoLuongTon = sanpham.SoLuongTon;
            item.MaLoai = sanpham.MaLoai;
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }
        public ActionResult Loai(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.Loais.ToList().OrderBy(n => n.MaLoai).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemLoai()
        {
            return View();
        }
        public ActionResult SuaLoai(int id)
        {
            Loai item = db.Loais.SingleOrDefault(n => n.MaLoai == id);
            return View(item);
        }
        [HttpPost]
        public ActionResult SuaLoai(Loai loai)
        {
            Loai itemm = db.Loais.SingleOrDefault(n => n.MaLoai == loai.MaLoai);
            itemm.TenLoai = loai.TenLoai;
            db.SubmitChanges();
            return RedirectToAction("Loai");
        }
        public ActionResult ChiTietLoai(int id)
        {
            Loai item = db.Loais.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = item.MaLoai;
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(item);
        }
        public ActionResult XoaLoai(int id)
        {
            Loai item = db.Loais.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = item.MaLoai;
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(item);
        }
        [HttpPost, ActionName("XoaLoai")]
        public ActionResult XacNhanXoa1(int id)
        {
            Loai item = db.Loais.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = item.MaLoai;
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Loais.DeleteOnSubmit(item);
            db.SubmitChanges();
            return RedirectToAction("Loai");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemLoai(Loai item)
        {
            db.Loais.InsertOnSubmit(item);
            db.SubmitChanges();
            return RedirectToAction("Loai");
        }
        public ActionResult KhachHang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.KhachHangs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult XoaKH(int id)
        {
            KhachHang item = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = item.MaKH;
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(item);
        }
        [HttpPost, ActionName("XoaKH")]
        public ActionResult XacNhanXoa2(int id)
        {
            KhachHang item = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = item.MaKH;
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KhachHangs.DeleteOnSubmit(item);
            db.SubmitChanges();
            return RedirectToAction("KhachHang");
        }
        public ActionResult ChiTietKH(int id)
        {
            KhachHang item = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = item.MaKH;
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(item);
        }
        public ActionResult SuaKH(int id)
        {
            KhachHang item = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            return View(item);
        }
        [HttpPost]
        public ActionResult SuaKH(KhachHang kh)
        {
            KhachHang itemm = db.KhachHangs.SingleOrDefault(n => n.MaKH == kh.MaKH);
            itemm.HoTen = kh.HoTen;
            itemm.TaiKhoan = kh.TaiKhoan;
            itemm.MatKhau = kh.MatKhau;
            itemm.Email = kh.Email;
            itemm.DiaChiKH = kh.DiaChiKH;
            itemm.DienThoaiKH = kh.DienThoaiKH;
            itemm.NgaySinh = kh.NgaySinh;
            db.SubmitChanges();
            return RedirectToAction("KhachHang");
        }
        public ActionResult DonDatHang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.DonDatHangs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SuaDDH(int id)
        {
            DonDatHang item = db.DonDatHangs.SingleOrDefault(n => n.MaDonHang == id);
            return View(item);
        }
        [HttpPost]
        public ActionResult SuaDDH(DonDatHang ddh)
        {
            DonDatHang itemm = db.DonDatHangs.SingleOrDefault(n => n.MaDonHang == ddh.MaDonHang);
            if (itemm.TinhTrangGiaohang == null)
            {
                itemm.TinhTrangGiaohang = ddh.TinhTrangGiaohang;
                db.SubmitChanges();
            }
            else
            itemm.TinhTrangGiaohang = ddh.TinhTrangGiaohang;
            if (itemm.DaThanhToan == null)
            {
                itemm.DaThanhToan = ddh.DaThanhToan;
                db.SubmitChanges();
            }    
            else
            itemm.DaThanhToan = ddh.DaThanhToan;
            db.SubmitChanges();
            return RedirectToAction("DonDatHang");
        }
        public ActionResult ChiTietDH(int id)
        {
            ChiTietDatHang item = db.ChiTietDatHangs.FirstOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = item.MaDonHang;
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(item);
        }
        public ActionResult XoaDDH(int id)
        {
            DonDatHang item = db.DonDatHangs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = item.MaDonHang;
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(item);
        }
        [HttpPost, ActionName("XoaDDH")]
        public ActionResult XacNhanXoa3(int id)
        {
            DonDatHang item = db.DonDatHangs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = item.MaDonHang;
            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DonDatHangs.DeleteOnSubmit(item);
            db.SubmitChanges();
            return RedirectToAction("DonDatHang");
        }
    }
}
