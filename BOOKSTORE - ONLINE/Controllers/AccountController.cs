using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Ajax.Utilities;
using BOOKSTORE___ONLINE.Models.DB;
using System.Runtime.InteropServices;

namespace BOOKSTORE___ONLINE.Controllers
{

    public class AccountController : Controller
    {
        static string constr = ConfigurationManager.ConnectionStrings["QLWSACH"].ConnectionString;
        SqlConnection conn = new SqlConnection(constr);
        WebBS db = new WebBS();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        //TUAN
        
        //CHI

        //KHOA
        public ActionResult Login() //KH
        {
            ViewBag.ShowSidebar = false;
            return View();
        }

        public ActionResult CreateThongTinKhachHang()
        {
            ViewBag.ShowSidebar = false;
            return View();
        }

        [HttpPost]
        public ActionResult CreateThongTinKhachHang(FormCollection form)
        {
            /*
             public int MaKH {  get; set; }
             public string HoTen { get; set; }
             public DateTime NgaySinh { get; set; }
             public string GioiTinh { get; set; }
             public string SDT { get; set; }
             public string Email { get; set; }
             public string DiaChi { get; set; }
             public string CMND_CCCD { get; set; }
             public string LoaiKhachHang { get; set; }
             public DateTime NgayTao { get; set; }
             */

            string hoten = form["HoTen"];
            DateTime ngaysinh = DateTime.Parse(form["NgaySinh"]);
            string gioitinh = form["GioiTinh"];
            string sodt = form["SDT"];
            string email = form["Email"];
            string diachi = form["DiaChi"];
            string CMND = form["CMND_CCCD"];

            KHACHHANG KH = new KHACHHANG()
            {
                HoTen = hoten,
                NgaySinh = ngaysinh,
                GioiTinh = gioitinh,
                SDT = sodt,
                Email = email,
                DiaChi = diachi,
                CMND_CCCD = CMND,
                
            };

        
            db.KHACHHANGs.Add(KH);
            db.SaveChanges();
            TempData["MAKH"] = KH.MaKH;

            return RedirectToAction("Dangky");
            
        }

        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangkyaction(FormCollection form)
        {
            string username = form["TenDangNhap"];
            string password = form["MatKhau"];
            string makh = TempData["MAKH"].ToString();
            TAIKHOANKHACHHANG tkkh = new TAIKHOANKHACHHANG()
            {
                TenDangNhap = username,
                MatKhau = password,
                MaKH = int.Parse(makh),
                VaiTro = "KhachHang",
            };


            db.TAIKHOANKHACHHANGs.Add(tkkh);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        public ActionResult LoginNV() //KH
        {
            ViewBag.ShowSidebar = false;
            return View();
        }

        public ActionResult LoginAD() //KH
        {
            ViewBag.ShowSidebar = false;
            return View();
        }

        public ActionResult Error()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult LoginActionKH(FormCollection form)
        {
            string username = form["tenDangNhap"];
            string password = form["matKhau"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin!";
                return RedirectToAction("Login");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {

                    string sql = @"SELECT * FROM TAIKHOANKHACHHANG tk, KHACHHANG kh 
                                     WHERE kh.MaKH = tk.MaKH and TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@TenDangNhap", username);
                        cmd.Parameters.AddWithValue("@MatKhau", password);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Lưu thông tin cần thiết vào Session
                                Session["username"] = reader["TenDangNhap"].ToString();
                                Session["role"] = reader["VaiTro"].ToString().Trim();
                                Session["MAKH"] = reader["MAKH"].ToString();
                                Session["HoTen"] = reader["HoTen"].ToString();
                                Session["DiaChi"] = reader["DiaChi"].ToString();
                               
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["Error"] = "Sai tên đăng nhập hoặc mật khẩu!";
                                return RedirectToAction("Error");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi đăng nhập!";
                return RedirectToAction("Error");
            }
        }

        [HttpPost]

        public ActionResult LoginActionNV(FormCollection form)
        {
            string username = form["tenDangNhap"];
            string password = form["matKhau"];
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login");
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    string sql = @"SELECT * FROM TAIKHOAN WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                    using(SqlCommand cmd = new SqlCommand(sql,conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDangNhap", username);
                        cmd.Parameters.AddWithValue ("@MatKhau", password);
                        conn.Open();
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                Session["username"] = reader["TenDangNhap"].ToString();
                                Session["role"] = reader["VaiTro"].ToString().Trim();
                                Session["MANV"] = reader["MANV"].ToString();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["Error"] = "Sai tên đăng nhập hoặc mật khẩu!";
                                return RedirectToAction("Error");
                            }
                        }    
                    }    
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi đăng nhập!";
                return RedirectToAction("Error");
            }
        }

        [HttpPost]

        public ActionResult LoginActionAD(FormCollection form)
        {
            string username = form["tenDangNhap"];
            string password = form["matKhau"];
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login");
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    string sql = @"SELECT * FROM TAIKHOAN_ADMIN WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDangNhap", username);
                        cmd.Parameters.AddWithValue("@MatKhau", password);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Session["username"] = reader["TenDangNhap"].ToString();
                                Session["role"] = reader["VaiTro"].ToString().Trim();
                                Session["MAADMIN"] = reader["MAADMIN"].ToString();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["Error"] = "Sai tên đăng nhập hoặc mật khẩu!";
                                return RedirectToAction("Error");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi đăng nhập!";
                return RedirectToAction("Error");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login","Account");
        }


        public ActionResult ThongBaoThanhCong()
        {

            ViewBag.ThongBao = "Thực hiện thành công";
            return View();
                    
        }

        //KIET

        //DAT
    }
}