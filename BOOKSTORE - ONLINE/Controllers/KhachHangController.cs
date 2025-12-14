using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOOKSTORE___ONLINE.Models.Entities;
using BOOKSTORE___ONLINE.Models.DB;

namespace BOOKSTORE___ONLINE.Controllers
{
    public class KhachHangController : Controller
    {
        static string constr = ConfigurationManager.ConnectionStrings["QLWSACH"].ConnectionString;
 
        BOOKSTORE___ONLINE.Models.Entities.SACHs S = new BOOKSTORE___ONLINE.Models.Entities.SACHs();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View();
        }
        //TUAN
        public ActionResult TimKiemTheoDanhMucChild(int id, string dm)
        {

            var listSach = SACHs.layDanhSachSanPham();
            ViewBag.AS = layAnhSach();
            var lstk = listSach
                 .Where(x =>
                     x.MADANHMUC == id &&
                     x.MADANHMUC_CHILD.Trim().ToUpper() == dm.Trim().ToUpper()
                 )
                 .ToList();

            return View("Index",lstk);

        }

        public List<BOOKSTORE___ONLINE.Models.Entities.ANHSACHs> layAnhSach()
        {
            SqlConnection conn = new SqlConnection(constr);
            SqlDataAdapter asa = new SqlDataAdapter("SELECT * FROM ANHSACH", conn);
            DataTable dataTable = new DataTable();
            List<BOOKSTORE___ONLINE.Models.Entities.ANHSACHs> listSach = new List<BOOKSTORE___ONLINE.Models.Entities.ANHSACHs>();
            asa.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                ANHSACHs anhSach = new ANHSACHs()
                {
                    MASACH = (int)row["MASACH"],
                    LINKANH = row["LINKANH"].ToString()
                };
                listSach.Add(anhSach);
            }

            return listSach;
        }

        public ActionResult TimKiemTheoKhoangGiaDuoi50()
        {
            ViewBag.Duoi50 = null;
            ViewBag.Tu50den100 = null;
            ViewBag.Tu100den300 = null;
            ViewBag.Tren300 = null;

            SqlConnection conn = new SqlConnection(constr);
            List<SACH> listSach = new List<SACH>();
            SqlDataAdapter sach = new SqlDataAdapter("Select * from SACH", conn);
            DataTable dataTable = new DataTable();
            sach.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                SACH s = new SACH()
                {
                    MASACH = Convert.ToInt32(row["MASACH"]),
                    MADANHMUC = Convert.ToInt32(row["MADANHMUC"]),
                    MADANHMUC_CHILD = row["MADANHMUC_CHILD"] != DBNull.Value ? row["MADANHMUC_CHILD"].ToString().Trim() : null,
                    TENSACH = row["TENSACH"]?.ToString() ?? "",
                    TACGIA = row["TACGIA"]?.ToString() ?? "-",
                    NHAXUATBAN = row["NHAXUATBAN"]?.ToString() ?? "-",
                    GIABAN = row["GIABAN"] != DBNull.Value ? Convert.ToDecimal(row["GIABAN"]) : (decimal?)null,
                    SOLUONGTONKHO = row["SOLUONGTONKHO"] != DBNull.Value ? Convert.ToInt32(row["SOLUONGTONKHO"]) : (int?)null,
                    MOTACHITIET = row["MOTACHITIET"]?.ToString() ?? "-",
                    NGAYTHEMSANPHAM = row["NGAYTHEMSANPHAM"] != DBNull.Value ? Convert.ToDateTime(row["NGAYTHEMSANPHAM"]) : DateTime.MinValue,
                    NGAYCAPNHAT = row["NGAYCAPNHAT"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["NGAYCAPNHAT"]) : null
                };
                listSach.Add(s);
            }

            var Duoi50 = listSach.Where(x => x.GIABAN < 50000).ToList();
            ViewBag.AS = layAnhSach();
            return View("Index", Duoi50);
        }

        public ActionResult TimKiemTheoKhoangGiaTu50Den100()
        {
            ViewBag.Duoi50 = null;
            ViewBag.Tu50den100 = null;
            ViewBag.Tu100den300 = null;
            ViewBag.Tren300 = null;

            SqlConnection conn = new SqlConnection(constr);
            List<SACH> listSach = new List<SACH>();
            SqlDataAdapter sach = new SqlDataAdapter("Select * from SACH", conn);
            DataTable dataTable = new DataTable();
            sach.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                SACH s = new SACH()
                {
                    MASACH = Convert.ToInt32(row["MASACH"]),
                    MADANHMUC = Convert.ToInt32(row["MADANHMUC"]),
                    MADANHMUC_CHILD = row["MADANHMUC_CHILD"] != DBNull.Value ? row["MADANHMUC_CHILD"].ToString().Trim() : null,
                    TENSACH = row["TENSACH"]?.ToString() ?? "",
                    TACGIA = row["TACGIA"]?.ToString() ?? "-",
                    NHAXUATBAN = row["NHAXUATBAN"]?.ToString() ?? "-",
                    GIABAN = row["GIABAN"] != DBNull.Value ? Convert.ToDecimal(row["GIABAN"]) : (decimal?)null,
                    SOLUONGTONKHO = row["SOLUONGTONKHO"] != DBNull.Value ? Convert.ToInt32(row["SOLUONGTONKHO"]) : (int?)null,
                    MOTACHITIET = row["MOTACHITIET"]?.ToString() ?? "-",
                    NGAYTHEMSANPHAM = row["NGAYTHEMSANPHAM"] != DBNull.Value ? Convert.ToDateTime(row["NGAYTHEMSANPHAM"]) : DateTime.MinValue,
                    NGAYCAPNHAT = row["NGAYCAPNHAT"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["NGAYCAPNHAT"]) : null
                };
                listSach.Add(s);
            }

           
            var Tu50den100 = listSach.Where(X => X.GIABAN >= 50000 && X.GIABAN <= 100000).ToList();
            ViewBag.AS = layAnhSach();
            return View("Index", Tu50den100);
        }

        public ActionResult TimKiemTheoKhoangTu100Den300()
        {
            ViewBag.Duoi50 = null;
            ViewBag.Tu50den100 = null;
            ViewBag.Tu100den300 = null;
            ViewBag.Tren300 = null;

            SqlConnection conn = new SqlConnection(constr);
            List<SACH> listSach = new List<SACH>();
            SqlDataAdapter sach = new SqlDataAdapter("Select * from SACH", conn);
            DataTable dataTable = new DataTable();
            sach.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                SACH s = new SACH()
                {
                    MASACH = Convert.ToInt32(row["MASACH"]),
                    MADANHMUC = Convert.ToInt32(row["MADANHMUC"]),
                    MADANHMUC_CHILD = row["MADANHMUC_CHILD"] != DBNull.Value ? row["MADANHMUC_CHILD"].ToString().Trim() : null,
                    TENSACH = row["TENSACH"]?.ToString() ?? "",
                    TACGIA = row["TACGIA"]?.ToString() ?? "-",
                    NHAXUATBAN = row["NHAXUATBAN"]?.ToString() ?? "-",
                    GIABAN = row["GIABAN"] != DBNull.Value ? Convert.ToDecimal(row["GIABAN"]) : (decimal?)null,
                    SOLUONGTONKHO = row["SOLUONGTONKHO"] != DBNull.Value ? Convert.ToInt32(row["SOLUONGTONKHO"]) : (int?)null,
                    MOTACHITIET = row["MOTACHITIET"]?.ToString() ?? "-",
                    NGAYTHEMSANPHAM = row["NGAYTHEMSANPHAM"] != DBNull.Value ? Convert.ToDateTime(row["NGAYTHEMSANPHAM"]) : DateTime.MinValue,
                    NGAYCAPNHAT = row["NGAYCAPNHAT"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["NGAYCAPNHAT"]) : null
                };
                listSach.Add(s);
            }

            
            var Tu100den300 = listSach.Where(X => X.GIABAN >= 100000 && X.GIABAN <= 300000).ToList();
            ViewBag.AS = layAnhSach();
            return View("Index", Tu100den300);
        }

        public ActionResult TimKiemTheoKhoangGiaTren300()
        {
            ViewBag.Duoi50 = null;
            ViewBag.Tu50den100 = null;
            ViewBag.Tu100den300 = null;
            ViewBag.Tren300 = null;

            SqlConnection conn = new SqlConnection(constr);
            List<SACH> listSach = new List<SACH>();
            SqlDataAdapter sach = new SqlDataAdapter("Select * from SACH", conn);
            DataTable dataTable = new DataTable();
            sach.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                SACH s = new SACH()
                {
                    MASACH = Convert.ToInt32(row["MASACH"]),
                    MADANHMUC = Convert.ToInt32(row["MADANHMUC"]),
                    MADANHMUC_CHILD = row["MADANHMUC_CHILD"] != DBNull.Value ? row["MADANHMUC_CHILD"].ToString().Trim() : null,
                    TENSACH = row["TENSACH"]?.ToString() ?? "",
                    TACGIA = row["TACGIA"]?.ToString() ?? "-",
                    NHAXUATBAN = row["NHAXUATBAN"]?.ToString() ?? "-",
                    GIABAN = row["GIABAN"] != DBNull.Value ? Convert.ToDecimal(row["GIABAN"]) : (decimal?)null,
                    SOLUONGTONKHO = row["SOLUONGTONKHO"] != DBNull.Value ? Convert.ToInt32(row["SOLUONGTONKHO"]) : (int?)null,
                    MOTACHITIET = row["MOTACHITIET"]?.ToString() ?? "-",
                    NGAYTHEMSANPHAM = row["NGAYTHEMSANPHAM"] != DBNull.Value ? Convert.ToDateTime(row["NGAYTHEMSANPHAM"]) : DateTime.MinValue,
                    NGAYCAPNHAT = row["NGAYCAPNHAT"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["NGAYCAPNHAT"]) : null
                };
                listSach.Add(s);
            }

            
            var Tren300 = listSach.Where( X => X.GIABAN > 300000).ToList();
            ViewBag.AS = layAnhSach();
            return View("Index", Tren300);
        }

        public ActionResult TimKiemTheoTuKhoa(string keyword, FormCollection form)
        {
            keyword = form["keyword"];
            List<SACHs> s = SACHs.layDanhSachSanPham();
            var findrs = s.Where(x=>x.TENSACH.ToLower().Contains(keyword.ToLower())).ToList();
            ViewBag.AS = layAnhSach();
            return View("Index", findrs);
        }

      

        public ActionResult Detail()
        {
            return View();
        }

        private WebBS db = new WebBS();

        public ActionResult XemDonHang()
        {
            int makh = Convert.ToInt32(Session["MAKH"]);

            DataTable dt = new DataTable();

            string query = @"
     SELECT DH.MADONHANG, DH.NgayDat, DH.TongTien,DH.DiaChiGiaoHang,DH.TRANGTHAI, PT.TENPHUONGTHUC, CT.SOLUONG,S.TENSACH, S.GIABAN FROM DONHANG DH, CHITIETDONHANG CT, SACH S, PHUONGTHUCTHANHTOAN PT
       WHERE DH.MADONHANG =  CT.MADONHANG AND CT.MASACH = S.MASACH AND DH.MAPHUONGPHUC = PT.MAPHUONGTHUC AND MAKH = @MAKH
    ";

            using (SqlConnection conn = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@MAKH", makh);
                adapter.Fill(dt);
            }

            // Chuyển DataTable sang List<DonHangViewModel>
            List<DONHANGVIEWMODEL> list = new List<DONHANGVIEWMODEL>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DONHANGVIEWMODEL
                {
                    MADONHANG = Convert.ToInt32(row["MADONHANG"]),
                    NgayDat = Convert.ToDateTime(row["NgayDat"]),
                    TongTien = Convert.ToDecimal(row["TongTien"]),
                    DiaChiGiaoHang = row["DiaChiGiaoHang"].ToString(),
                    TRANGTHAI = row["TRANGTHAI"].ToString(),
                    TENSACH = row["TENSACH"].ToString(),
                    SOLUONG = Convert.ToInt32(row["SOLUONG"]),
                    GIATAITHOIDIEMMUA = Convert.ToDecimal(row["GIABAN"])
                });
            }

            return View(list);
        }

        //CHI

        //KHOA

        //KIET

        //DAT
    }
}