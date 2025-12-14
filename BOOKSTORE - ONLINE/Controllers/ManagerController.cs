using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.UI.WebControls;
using BOOKSTORE___ONLINE.Models.DB;
using BOOKSTORE___ONLINE.Models.Entities;

namespace BOOKSTORE___ONLINE.Controllers
{
    public class ManagerController : Controller
    {
        static string constr = ConfigurationManager.ConnectionStrings["QLWSACH"].ConnectionString;
        WebBS db = new WebBS();
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }
        //TUAN


        public ActionResult QLSP_NV()
        {

            ViewBag.AS = ANHSACHs.layAnhSach();
            var listsp = SACHs.layDanhSachSanPham();
            return View(listsp);

        }

        public ActionResult DanhMucAdminPartial()
        {
            return PartialView();

        }

        public ActionResult EditKhoNhanVien(int id)
        {
            var sp = SACHs.layDanhSachSanPham().FirstOrDefault(x => x.MASACH == id);
            if (sp == null)
            {
                ViewBag.Error = "Lỗi";
            }
            return View(sp);
        }

        [HttpPost]
        public ActionResult EditKhoNhanVien(SACHs model, HttpPostedFileBase fileImage, HttpPostedFileBase fileMoTa)
        {
            var sp = SACHs.layDanhSachSanPham().FirstOrDefault(x => x.MASACH == model.MASACH);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Cập nhật thông tin cơ bản
            sp.TENSACH = model.TENSACH;
            sp.MADANHMUC_CHILD = model.MADANHMUC_CHILD;
            sp.TACGIA = model.TACGIA;
            sp.NHAXUATBAN = model.NHAXUATBAN;
            sp.GIABAN = model.GIABAN;
            sp.NGAYCAPNHAT = DateTime.Now.Date;
            sp.MOTACHITIET = model.MOTACHITIET;
            sp.NGAYTHEMSANPHAM = model.NGAYTHEMSANPHAM;


            // =============================
            if (fileImage != null && fileImage.ContentLength > 0)
            {
                string path = "/Content/" + Path.GetFileName(fileImage.FileName);
                string serverPath = Server.MapPath("~" + path);
                fileImage.SaveAs(serverPath);
                sp.ANHSANPHAM = path;   // Lưu đường dẫn vào DB
            }


            //   UPLOAD FILE MÔ TẢ
            // =============================
            if (fileMoTa != null && fileMoTa.ContentLength > 0)
            {
                string path = "/Content/Mota/" + Path.GetFileName(fileMoTa.FileName);
                string serverPath = Server.MapPath("~" + path);
                fileMoTa.SaveAs(serverPath);
                sp.MOTAFILE = path;
            }





            // 👉 Lưu lại DB
            using (var conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = @"UPDATE SACH
                   SET TENSACH=@TENSACH,
                       MADANHMUC_CHILD=@MADANHMUC_CHILD,
                       TACGIA=@TACGIA,
                       NHAXUATBAN=@NHAXUATBAN,
                       GIABAN=@GIABAN,
                       NGAYCAPNHAT=@NGAYCAPNHAT,
                       MOTACHITIET=@MOTACHITIET,
                      
                       MOTAFILE=@MOTAFILE
                   WHERE MASACH=@MASACH";


                string sql_updateanh = @"UPDATE ANHSACH
                       SET LINKANH = @ANHSANPHAM
                       WHERE MASACH = @MASACH";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TENSACH", sp.TENSACH);
                    cmd.Parameters.AddWithValue("@MADANHMUC_CHILD", sp.MADANHMUC_CHILD);
                    cmd.Parameters.AddWithValue("@TACGIA", sp.TACGIA);
                    cmd.Parameters.AddWithValue("@NHAXUATBAN", sp.NHAXUATBAN);
                    cmd.Parameters.AddWithValue("@GIABAN", sp.GIABAN);
                    cmd.Parameters.AddWithValue("@NGAYCAPNHAT", sp.NGAYCAPNHAT);
                    cmd.Parameters.AddWithValue("@MOTACHITIET", sp.MOTACHITIET);
                    //cmd.Parameters.AddWithValue("@ANHSANPHAM", sp.ANHSANPHAM);
                    cmd.Parameters.AddWithValue("@MOTAFILE", sp.MOTAFILE);
                    cmd.Parameters.AddWithValue("@MASACH", sp.MASACH);

                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SqlCommand(sql_updateanh, conn))
                {
                    cmd.Parameters.AddWithValue("@ANHSANPHAM", sp.ANHSANPHAM);
                }
            }


            return RedirectToAction("QLSP_NV", "Manager");
        }

        public ActionResult XemDanhSachDonHang()
        {
            List<DONHANGVIEWMODEL> list = new List<DONHANGVIEWMODEL>();
            string query = @"
            SELECT DH.MADONHANG, DH.NgayDat, DH.TongTien, DH.DiaChiGiaoHang, DH.TRANGTHAI,
            S.TENSACH, CT.SOLUONG, CT.GIATAITHOIDIEMMUA
            FROM DONHANG DH
            LEFT JOIN CHITIETDONHANG CT ON DH.MADONHANG = CT.MADONHANG
            LEFT JOIN SACH S ON CT.MASACH = S.MASACH
            LEFT JOIN PHUONGTHUCTHANHTOAN PT ON DH.MAPHUONGPHUC = PT.MAPHUONGTHUC
            ORDER BY DH.MADONHANG ASC

        ";

            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new DONHANGVIEWMODEL
                    {
                        MADONHANG = Convert.ToInt32(row["MADONHANG"]),
                        NgayDat = Convert.ToDateTime(row["NgayDat"]),
                        TongTien = Convert.ToDecimal(row["TongTien"]),
                        DiaChiGiaoHang = row["DiaChiGiaoHang"].ToString(),
                        TRANGTHAI = row["TRANGTHAI"].ToString(),
                        TENSACH = row["TENSACH"] != DBNull.Value ? row["TENSACH"].ToString() : "",
                        SOLUONG = row["SOLUONG"] != DBNull.Value ? Convert.ToInt32(row["SOLUONG"]) : 0,
                        GIATAITHOIDIEMMUA = row["GIATAITHOIDIEMMUA"] != DBNull.Value ? Convert.ToDecimal(row["GIATAITHOIDIEMMUA"]) : 0
                    });
                }

            }

            return View(list);
        }

        public ActionResult XemDoanhThu()
        {
            List<string> labels = new List<string>();
            List<decimal> data = new List<decimal>();

            string query = @"
            SELECT MONTH(NgayDat) AS Thang, SUM(TongTien) AS DoanhThu
            FROM DONHANG
            GROUP BY MONTH(NgayDat)
            ORDER BY MONTH(NgayDat)
        ";

            using (SqlConnection conn = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    labels.Add("Tháng " + reader["Thang"].ToString());
                    data.Add(Convert.ToDecimal(reader["DoanhThu"]));
                }
            }

            ViewBag.Labels = labels;
            ViewBag.Data = data;

            return View();
        }


        public ActionResult Detail(int id)
        {

            return RedirectToAction("Detail", "Home");
        }

        public ActionResult themSanPham()
        {
            return View();
        }

        public ActionResult xoaSanPham(int id)
        {
            var sp = db.SACHes.ToList().FirstOrDefault(x => x.MASACH == id);
            return View(sp);

        }



        public ActionResult xoaSanPhamAction(int id)
        {
            var spdelete = db.SACHes.FirstOrDefault(x => x.MASACH == id);

            if (spdelete != null)
            {
                // Xóa các chi tiết đơn hàng liên quan
                var chiTietDH = db.CHITIETDONHANGs.Where(x => x.MASACH == id).ToList();
                foreach (var ct in chiTietDH)
                {
                    db.CHITIETDONHANGs.Remove(ct);
                }

                var chitietsach = db.CHITIETSACHes.Where(x => x.MASACH == id).ToList();
                foreach (var ct in chitietsach)
                {
                    db.CHITIETSACHes.Remove(ct);
                }

                db.SACHes.Remove(spdelete);
                db.SaveChanges();

                TempData["ThanhCong"] = $"Xóa sản phẩm {spdelete.TENSACH} thành công !";
            }

            return RedirectToAction("ThongBaoThanhCong", "Account");
        }



        public ActionResult suaSanPham()
        {
            return View();
        }

        public ActionResult NhapHang()
        {
            return View(db.NHACUNGCAPs.ToList());
        }

        public ActionResult dsSachNCC(int id)
        {

            return View(db.SACHes.ToList().Where(x => x.MANHACUNGCAP == id));
        }
        [HttpPost]
        public ActionResult timNCC(FormCollection form)
        {
            string ncc = form["timNcc"];

            var nccfind = db.NHACUNGCAPs
                .Where(x => x.TENNHACUNGCAP.Contains(ncc))
                .ToList();

            if (nccfind.Any())
            {
                return View("NhapHang", nccfind);
            }
            else
            {
                ViewBag.LoiTimNCC = "Không tìm thấy nhà cung cấp!";
                return View("NhapHang", new List<NHACUNGCAP>());

            }
        }

        public ActionResult chonSachNhapHang(int id)
        {
            return View(db.SACHes.ToList().FirstOrDefault(x=>x.MASACH == id));
        }

        [HttpPost]
        public ActionResult chonSachAction(FormCollection form) {
            int sl = int.Parse(form["soluong"]);
            double giaBan = sl * double.Parse(form["GIABAN"]);
            ViewBag.GIABAN = giaBan;

            int mancc = int.Parse(form["MANHACUNGCAP"]);
            int masach = int.Parse(form["MASACH"]);
            string tensach = form["TENSACH"].ToString();
            using (WebBS db = new WebBS())
            {
                {
                    NHAPHANG nh = new NHAPHANG()
                    {
                        MANHACUNGCAP = mancc,
                        MASACH = masach,
                        TENSACH = tensach,
                        SOLUONG = sl
                    };
                    db.NHAPHANGs.Add(nh);
                   SACH Sach = db.SACHes.ToList().FirstOrDefault(x=>x.MANHACUNGCAP == mancc && x.MASACH == masach);
                   Sach.SOLUONGTONKHO = (Sach.SOLUONGTONKHO ?? 0) + sl;

                    db.SaveChanges();
                };
                
                
            }
            return View(db.NHAPHANGs.ToList());
        }

        [HttpPost]
        public ActionResult traCuuTonKho(FormCollection form)
        {
            string tenSP = form["tenSP"];


            if (string.IsNullOrWhiteSpace(tenSP))
            {
                ViewBag.Loi = "Vui lòng nhập tên sản phẩm cần tra cứu!";
                return View("QLSP_NV", SACHs.layDanhSachSanPham());
            }


            using (WebBS db = new WebBS())
            {
                var danhSach = (
                    from s in db.SACHes
                    from k in db.KHOSACHes
                    where s.MASACH == k.MASACH
                          && s.TENSACH.Contains(tenSP)
                    select new ThongTinTonKho
                    {
                        TENKHO = k.TENKHO,
                        MASACH = s.MASACH,
                        TENSACH = s.TENSACH,
                        SOLUONGTONKHO = s.SOLUONGTONKHO ?? 0
                    }

                ).ToList();
                if(!danhSach.Any())
                {
                    TempData["Loi"] = "Không tìm thấy sản phẩm cần tra cứu !";
                    return RedirectToAction("QLSP_NV");
                }
                return View(danhSach);
            }
        }

        public ActionResult DanhMuc_NhanVienPartial()
        {
            return PartialView();
        }

        public ActionResult formQLXK()
        {
            /*
                public int MADONHANG { get; set; }
                public string TENNHANVIEN { get; set; }
         
                public string TENKHACHHANG { get; set; }

                public int MASACH {  get; set; }

                public int SOLUONG { get; set; }

                public decimal GIATAITHOIDIEMMUA { get; set; }

                public string DIACHI {  get; set; }

                public string TRANGTHAI {  get; set; }

                public decimal tongTien()
                {
                    return GIATAITHOIDIEMMUA * SOLUONG;
                }
             */
            using (WebBS db = new WebBS())
            {
                var danhSach = (
                    from dh in db.DONHANGs
                    from ct in db.CHITIETDONHANGs
                    from nv in db.USER_NV
                    from kh in db.KHACHHANGs
                    from s in db.SACHes
                    where dh.MADONHANG == ct.MADONHANG && dh.MAKH == kh.MaKH && nv.MANV == dh.MANV 
                    && s.MASACH == ct.MASACH

                    select new xuatKhoInFo()
                    {
                        MADONHANG = ct.MADONHANG,
                        TENNHANVIEN = nv.TENNHANVIEN,
                        TENKHACHHANG = kh.HoTen,
                        MASACH = ct.MASACH,
                        SOLUONG =(int) ct.SOLUONG,
                        TONKHO = (int) s.SOLUONGTONKHO,
                        GIATAITHOIDIEMMUA = (decimal) ct.GIATAITHOIDIEMMUA,
                     


                    }
                    ).ToList();
                return View(danhSach);

            }
          


        }

        public ActionResult xoaSPView(int id)
        {
            return View(db.SACHes.FirstOrDefault(x=>x.MASACH == id));
        }

        public ActionResult ThemSPVaoKho()
        {
            return View();
        }


        public ActionResult kHACH()
        {
            return View(db.KHACHHANGs.ToList());
        }


        public ActionResult xoaKH(int id)
        {
            return View(db.KHACHHANGs.FirstOrDefault(x=>x.MaKH == id));
        }


        public ActionResult EditKH(int id)
        {
            return View(db.KHACHHANGs.FirstOrDefault(x => x.MaKH == id));
        }


        public ActionResult ToChucKhuyenMai()
        {
            return View(db.CHUONGTRINHKHUYENMAIs.ToList());
        }

        public ActionResult chonCTKM(string id)
        {
            return View(db.VOUCHERs.Where(x => x.MACT == id).ToList());
        }

        public ActionResult cnKM(int id) { 
            return View(db.VOUCHERs.FirstOrDefault(x=>x.MaVoucher == id));
        }

<<<<<<< Updated upstream

        public ActionResult XacNhanXuatKho(int id)
        {
            using (WebBS db = new WebBS())
            {
                var danhSach = (
                    from dh in db.DONHANGs
                    from ct in db.CHITIETDONHANGs
                    from nv in db.USER_NV
                    from kh in db.KHACHHANGs
                    from s in db.SACHes
                    where dh.MADONHANG == id && dh.MAKH == kh.MaKH && nv.MANV == dh.MANV
                    && s.MASACH == ct.MASACH

                    select new xuatKhoInFo()
                    {
                        MADONHANG = ct.MADONHANG,
                        TENNHANVIEN = nv.TENNHANVIEN,
                        TENKHACHHANG = kh.HoTen,
                        MASACH = ct.MASACH,
                        SOLUONG = (int)ct.SOLUONG,
                        TONKHO = (int)s.SOLUONGTONKHO,
                        GIATAITHOIDIEMMUA = (decimal)ct.GIATAITHOIDIEMMUA,



                    }
                    ).ToList();
                return View(danhSach);

            }


     
        }

        public ActionResult XuatKhoAction(int id)
        {
            //lấy chi tiết đơn hàng
            var chiTietDon = db.CHITIETDONHANGs.Where(x=>x.MADONHANG ==id);

            foreach(var ct in chiTietDon)
            {
                var sach = db.SACHes.FirstOrDefault(x=>x.MASACH == ct.MASACH);
                if (sach == null)
                    continue;

                if(sach.SOLUONGTONKHO < ct.SOLUONG)
                {
                    TempData["Error"] = $"Sách {sach.TENSACH} không đủ tồn kho!";
                    return RedirectToAction("XacNhanXuatKho", new { id });
                }

                //TRỪ TỒN KHO
                sach.SOLUONGTONKHO -= ct.SOLUONG;
            }    

            var donHang = db.DONHANGs.FirstOrDefault(x=>x.MADONHANG == id);
            if(donHang != null)
            {
                donHang.TRANGTHAI = "Đã xuất kho";
            }    

            db.SaveChanges();
            TempData["ThanhCong"] = "Xuất kho thành công !";
            return RedirectToAction("formQLXK");
=======
        public ActionResult lichSuMuaHang(int id)
        {
            using (WebBS db = new WebBS())
            {
                var dsls = (

                       from kh in db.KHACHHANGs
                       where kh.MaKH == id
                       from dh in db.DONHANGs.Where(d => d.MAKH == kh.MaKH)
                       from ct in db.CHITIETDONHANGs.Where(c => c.MADONHANG == dh.MADONHANG)
                       from s in db.SACHes.Where(sa => sa.MASACH == ct.MASACH)


                       select new ChiTietLichSu
                        {
                            MaKH = kh.MaKH,
                            TenKH = kh.HoTen,
                            MADONHANG = dh.MADONHANG,
                            TENSACH = s.TENSACH,
                            SOLUONG = (int)ct.SOLUONG,
                            NgayDat = (DateTime)dh.NgayDat,
                            GIATAITHOIDIEMMUA = (decimal) ct.GIATAITHOIDIEMMUA,
                            TRANGTHAI = dh.TRANGTHAI
                        }
                        ).ToList();
                return View(dsls);
            }    
                    
           
                   

>>>>>>> Stashed changes
        }
    }


    //CHI


    //KHOA

    //KIET

    //DAT
}