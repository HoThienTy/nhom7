using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SanGoStore.Models;
using System.Web.Security; // Thêm thư viện này để sử dụng FormsAuthentication

namespace SanGoStore.Controllers
{
    public class NguoiDungController : Controller
    {
        // Data context for SanGoStore
        dbSanGoStoreDataContext data = new dbSanGoStoreDataContext();

        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        // GET: NguoiDung/DangKy
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        // POST: NguoiDung/DangKy
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            var matkhaunhaplai = collection["MatKhauNhapLai"];
            var diachi = collection["DiaChi"];
            var email = collection["Email"];
            var dienthoai = collection["DienThoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);

            // Validation
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi3"] = "Phải nhập mật khẩu";
            }
            else if (string.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (matkhau != matkhaunhaplai)
            {
                ViewData["loi4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["loi5"] = "Địa chỉ không được bỏ trống";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["loi6"] = "Email không được bỏ trống";
            }
            else if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["loi7"] = "Phải nhập điện thoại";
            }
            else
            {
                // Create new customer
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiaChiKH = diachi;
                kh.DienThoaiKH = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);

                data.KhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }

            return this.DangKy();
        }

        // GET: NguoiDung/DangNhap
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        // POST: NguoiDung/DangNhap
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    FormsAuthentication.SetAuthCookie(tendn, false); // Set the auth cookie
                    Session["TaiKhoan"] = kh; // Save user info in session (optional)
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Thongbao = "TÊN ĐĂNG NHẬP HOẶC MẬT KHẨU KHÔNG ĐÚNG";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            // Xóa thông tin đăng nhập của người dùng
            FormsAuthentication.SignOut();

            // Chuyển hướng người dùng về trang chủ hoặc trang đăng nhập
            return RedirectToAction("Index", "Home");
        }
    }
}