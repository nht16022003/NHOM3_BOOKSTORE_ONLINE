using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
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
            return View(db.SACHes.ToList().FirstOrDefault(x => x.MASACH == id));
        }

        [HttpPost]
        public ActionResult chonSachAction(FormCollection form)
        {
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
                    SACH Sach = db.SACHes.ToList().FirstOrDefault(x => x.MANHACUNGCAP == mancc && x.MASACH == masach);
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
                if (!danhSach.Any())
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

        //public ActionResult formQLXK()
        //{
        //    using (WebBS db = new WebBS())
        //    {
        //        var danhSach = (
        //            from dh in db.DONHANGs
        //            from ct in db.CHITIETDONHANGs
        //            from kh in db.KHACHHANGs
        //            where dh.MADONHANG == ct.MADONHANG && dh.MAKH == kh.MaKH

        //            select new CHITIETDONHANG
        //            {
        //                MADONHANG = ct.MADONHANG

        //            }
        //    return View(db.DONHANGs.ToList());
        //    }


        //}

        public ActionResult xoaSPView(int id)
        {
            return View(db.SACHes.FirstOrDefault(x => x.MASACH == id));
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
            return View(db.KHACHHANGs.FirstOrDefault(x => x.MaKH == id));
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

        public ActionResult cnKM(int id)
        {
            return View(db.VOUCHERs.FirstOrDefault(x => x.MaVoucher == id));
        }

        public ActionResult ThemSanPhamMoiVaoKho()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemAction(
    FormCollection form,
    HttpPostedFileBase AnhSach,
    HttpPostedFileBase MOTAFILE)
        {
            int MADANHMUC = int.Parse(form["MADANHMUC"]);
            int SOLUONGTONKHO = int.Parse(form["SOLUONGTONKHO"]);
            int MANHACUNGCAP = int.Parse(form["MANHACUNGCAP"]);
            decimal GIABAN = decimal.Parse(form["GIABAN"]);

            string TENSACH = form["TENSACH"] ?? "";
            string MADANHMUC_CHILD = form["MADANHMUC_CHILD"] ?? "";
            string TACGIA = form["TACGIA"] ?? "";
            string NHAXUATBAN = form["NHAXUATBAN"] ?? "";
            string MOTACHITIET = form["MOTACHITIET"] ?? "";

            if (string.IsNullOrEmpty(TENSACH))
            {
                TempData["Error"] = "Tên sách không được để trống";
                return RedirectToAction("QLSP_NV");
            }

            string anhPath = null;
            string motaPath = null;
            int maSachMoi;

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                // ===== INSERT SÁCH + LẤY IDENTITY =====
                string insertSach = @"
            INSERT INTO SACH
            (MADANHMUC, MADANHMUC_CHILD, TENSACH, TACGIA, NHAXUATBAN,
             GIABAN, SOLUONGTONKHO, MANHACUNGCAP,
             MOTACHITIET, NGAYTHEMSANPHAM, NGAYCAPNHAT)
            OUTPUT INSERTED.MASACH
            VALUES
            (@MADANHMUC, @MADANHMUC_CHILD, @TENSACH, @TACGIA, @NHAXUATBAN,
             @GIABAN, @SOLUONGTONKHO, @MANHACUNGCAP,
             @MOTACHITIET, GETDATE(), GETDATE())";

                using (SqlCommand cmd = new SqlCommand(insertSach, conn))
                {
                    cmd.Parameters.AddWithValue("@MADANHMUC", MADANHMUC);
                    cmd.Parameters.AddWithValue("@MADANHMUC_CHILD", MADANHMUC_CHILD);
                    cmd.Parameters.AddWithValue("@TENSACH", TENSACH);
                    cmd.Parameters.AddWithValue("@TACGIA", TACGIA);
                    cmd.Parameters.AddWithValue("@NHAXUATBAN", NHAXUATBAN);
                    cmd.Parameters.AddWithValue("@GIABAN", GIABAN);
                    cmd.Parameters.AddWithValue("@SOLUONGTONKHO", SOLUONGTONKHO);
                    cmd.Parameters.AddWithValue("@MANHACUNGCAP", MANHACUNGCAP);
                    cmd.Parameters.AddWithValue("@MOTACHITIET", MOTACHITIET);

                    maSachMoi = (int)cmd.ExecuteScalar();
                }

                // ===== UPLOAD ẢNH =====
                if (AnhSach != null && AnhSach.ContentLength > 0)
                {
                    string folder = Server.MapPath("~/Content/AnhSach");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = Path.GetFileName(AnhSach.FileName);
                    AnhSach.SaveAs(Path.Combine(folder, fileName));
                    anhPath = "/Content/AnhSach/" + fileName;

                    string insertAnh = "INSERT INTO ANHSACH(MASACH, LINKANH) VALUES(@MASACH,@LINKANH)";
                    using (SqlCommand cmd = new SqlCommand(insertAnh, conn))
                    {
                        cmd.Parameters.AddWithValue("@MASACH", maSachMoi);
                        cmd.Parameters.AddWithValue("@LINKANH", anhPath);
                        cmd.ExecuteNonQuery();
                    }
                }

                // ===== UPLOAD FILE MÔ TẢ =====
                if (MOTAFILE != null && MOTAFILE.ContentLength > 0)
                {
                    string folder = Server.MapPath("~/Content/Mota");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = Path.GetFileName(MOTAFILE.FileName);
                    MOTAFILE.SaveAs(Path.Combine(folder, fileName));
                    motaPath = "/Content/Mota/" + fileName;

                    string updateMota = "UPDATE SACH SET MOTAFILE=@MOTAFILE WHERE MASACH=@MASACH";
                    using (SqlCommand cmd = new SqlCommand(updateMota, conn))
                    {
                        cmd.Parameters.AddWithValue("@MOTAFILE", motaPath);
                        cmd.Parameters.AddWithValue("@MASACH", maSachMoi);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("QLSP_NV");
        }
    }
        //CHI


        //KHOA

        //KIET

        //DAT
    }