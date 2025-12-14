using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using BOOKSTORE___ONLINE.Models.DB;
using BOOKSTORE___ONLINE.Models.Entities;

namespace BOOKSTORE___ONLINE.Controllers
{
    public class CartController : Controller
    {
        WebBS db = new WebBS();
        static string constr = ConfigurationManager.ConnectionStrings["QLWSACH"].ConnectionString;
        SqlConnection conn = new SqlConnection(constr);
        

        // GET: Cart
        public ActionResult Index()
        {
            Cart cart = (Cart)Session["Cart"];
            CartItem c = (CartItem)Session["CartItem"];
            if (cart == null)
            {
                cart = new Cart(); //khởi tạo lại giỏ hàng nếu chưa có 
            }
            ViewBag.AS = Models.Entities.ANHSACHs.layAnhSach();
            return View(cart);
        }

        public ActionResult AddToCart(int id) //Mã sách
        {
            if (Session["username"] == null)//chưa đăng nhập
            {
                return RedirectToAction("Login", "Account");
            }
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart(); //khởi tạo lại giỏ hàng nếu chưa có 
            }
            int result = cart.Them(id);
            if (result == 1)
            { //thêm thành công
                Session["Cart"] = cart;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Lỗi không thể thêm sản phẩm vào giỏ hàng. Vui lòng thử lại!";
            return RedirectToAction("Detail", "Home", new { id = id });
        }
        public ActionResult RemoveFromCart(int id) //Mã sách
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart(); //khởi tạo lại giỏ hàng nếu chưa có 
            }
            int result = cart.Xoa(id);
            if (result == 1)
            { //xóa thành công
                Session["Cart"] = cart;
            }
            else
            {
                ViewBag.Error = "Lỗi không thể xóa sản phẩm trong giỏ hàng. Vui lòng thử lại!";
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult UpdateSL(int id, int type)//1: tăng, 2: giảm
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart(); //khởi tạo lại giỏ hàng nếu chưa có 
            }
            var result = -1;
            if (type == 1)//tăng
            {
                result = cart.Them(id);
            }
            else
            {
                result = cart.Giam(id);
            }
            if (result == 1)
            { // thành công
                Session["Cart"] = cart;
            }
            else
            {
                ViewBag.Error = "Lỗi không thể cập nhật số lượng sản phẩm trong giỏ hàng. Vui lòng thử lại!";
            }
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult ThanhToan()
        {
            // Lấy giỏ hàng từ session
            Cart cart = (Cart)Session["Cart"];

            // Kiểm tra giỏ hàng có dữ liệu
            if (cart == null || cart.list.Count == 0)
            {
                ViewBag.Error = "Bạn chưa mua sản phẩm nào!";
                return RedirectToAction("Index", "Home");
            }

            // Lấy thông tin khách hàng từ session
            string mauser = Session["MAKH"].ToString();
        
            if (mauser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Tạo đối tượng đơn hàng
            BOOKSTORE___ONLINE.Models.Entities.DONHANGs donHang = new BOOKSTORE___ONLINE.Models.Entities.DONHANGs()
            {
                
                MaKH = int.Parse(Session["MAKH"].ToString()),
                NgayDat = DateTime.Now,
                TongTien = cart.TongThanhTien(),
                DiaChiGiaoHang = Session["DiaChi"].ToString()
            };

            

            // Lưu đơn hàng vào CSDL (nếu có context db)
            // db.DONHANGs.Add(donHang);
            // db.SaveChanges();

            // Lưu chi tiết đơn hàng
            // foreach (var item in cart.list)
            // {
            //     CHITIETDONHANG ct = new CHITIETDONHANG()
            //     {
            //         MaDH = donHang.MaDH,
            //         MaSP = item.MaSP,
            //         SoLuong = item.SoLuong,
            //         GiaBan = item.DonGia
            //     };
            //     db.CHITIETDONHANGs.Add(ct);
            // }
            // db.SaveChanges();

           

            // Truyền dữ liệu sang view
            ViewBag.KhachHang = donHang.MaKH;
            ViewBag.NgayDat = donHang.NgayDat;
            ViewBag.TongTien = donHang.TongTien;
            ViewBag.DiaChiGiaoHang = donHang.DiaChiGiaoHang;
            ViewBag.AS = Models.Entities.ANHSACHs.layAnhSach();
            ViewBag.PTTT = Models.Entities.PHUONGTHUCTHANHTOANs.layDanhSachPhuongThucThanhToan();
            return View(cart.list); // View sẽ nhận danh sách sản phẩm đã mua
        }


        //public ActionResult Payment(int ship)
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    KHACHHANG user = (KHACHHANG)Session["User"];
        //    if (cart == null || cart.SoLuongMH() == 0)
        //    {
        //        ViewBag.Error = "Không có sản phẩm nào trong giỏ hàng. Vui lòng thêm sản phẩm và thử lại!";
        //        return RedirectToAction("Index", "Cart");
        //    }
        //    DONHANG hd = new DONHANG()
        //    {
        //        MaKH = user.MaKH,
        //        //MaNV = 1,
        //        NgayDat = DateTime.Now,
        //        TongTien = cart.TongThanhTien() + ship,
        //        //TinhTrang = 1,
        //        DiaChiGiaoHang = user.DiaChi,
        //        //DaThanhToan = true
        //    };
        //    db.tblHoaDon.Add(hd);//lưu tạm
        //    db.SaveChanges();//tự động generate ra MAHD và gán lại vào biến "hd"
        //    foreach (var item in cart.list)
        //    {
        //        tblChiTietHoaDon ct = new tblChiTietHoaDon()
        //        {
        //            MaHD = hd.MaHD,
        //            MaSach = item.MaSP,
        //            SoLuong = item.SoLuong,
        //            GiaBan = item.DonGia
        //        };
        //        db.tblChiTietHoaDon.Add(ct);
        //    }
        //    db.SaveChanges();
        //    Session["Cart"] = new Cart();//clear giỏ hàng
        //    return RedirectToAction("ConfirmPayment", "Cart");
        //}
        public ActionResult ConfirmPayment()
        {
            return View();
        }


        public ActionResult ThanhToanNganHang()
        {
            return View();
        }

        public ActionResult ThanhToanMoMO()
        {
            return View();
        }

        public ActionResult MuaNgay(int id)
        {
            if (Session["username"]==null)
            {
                return RedirectToAction("Login", "Account");
            }
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart(); //khởi tạo lại giỏ hàng nếu chưa có 
            }
            int result = cart.Them(id);
            if (result == 1)
            { //thêm thành công
                Session["Cart"] = cart;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Lỗi không thể thêm sản phẩm vào giỏ hàng. Vui lòng thử lại!";
            return RedirectToAction("Detail", "Home", new { id = id });
           
        }

        [HttpPost]
        public ActionResult XacNhanThanhToan(FormCollection form)
        {
            // Lấy giỏ hàng từ session
            Cart cart = (Cart)Session["Cart"];
            //Lấy thông tin radio được chọn tên phương thức thanh toán
            string mapt = form["MaPTTT"];
            if(string.IsNullOrEmpty(mapt))
            {
                ViewBag.LoiPT = "Vui lòng chọn phương thức thanh toán !";
                return RedirectToAction("ThanhToan");
            }


            //Lưu đơn hàng mới
            int maPhuongThuc = int.Parse(mapt);
            Session["MAPT"] = maPhuongThuc;
           
            if(maPhuongThuc == 1)
            {
                return RedirectToAction("ThanhToanNganHang");
            }    

        

            return null;
           
        }


        [HttpPost]
        public ActionResult XACNHANTHANHTOAN_LUUDB()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null || cart.list.Count == 0)
            {
                ViewBag.Loi = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Cart");
            }

            int MaPTValues = Convert.ToInt32(Session["MAPT"]);

            
            DONHANG dh = new DONHANG()
            {
                MAKH = Convert.ToInt32(Session["MAKH"]),
                MANV = 1, 
                NgayDat = DateTime.Now,
                TongTien = cart.TongThanhTien(),
                DiaChiGiaoHang = Session["DiaChi"].ToString(),
                TRANGTHAI = "Đã đặt hàng",
                MAPHUONGPHUC = MaPTValues
            };

            db.DONHANGs.Add(dh);
            db.SaveChanges(); 

            // Thêm chi tiết tất cả sản phẩm
            foreach (var c in cart.list)
            {
                CHITIETDONHANG ct = new CHITIETDONHANG()
                {
                    MADONHANG = dh.MADONHANG,
                    MASACH = c.MASACH,
                    SOLUONG = c.SOLUONG,
                    GIATAITHOIDIEMMUA = c.GIABAN
                };
                db.CHITIETDONHANGs.Add(ct);
            }
            db.SaveChanges();

            // Clear giỏ hàng
            Session["Cart"] = new Cart();

            TempData["ThongBao"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "Home");
        }




    }
}