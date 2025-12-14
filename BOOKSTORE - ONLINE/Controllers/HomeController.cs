using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BOOKSTORE___ONLINE.Models.Entities;

namespace BOOKSTORE___ONLINE.Controllers
{
    public class HomeController : Controller
    {
        static string constr = ConfigurationManager.ConnectionStrings["QLWSACH"].ConnectionString;
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(constr);
            List<SACHs> listSach = new List<SACHs>();
            SqlDataAdapter sach = new SqlDataAdapter("Select * from SACH", conn);
            DataTable dataTable = new DataTable();
            sach.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows) {
                SACHs s = new SACHs()
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
            ViewBag.AS = ANHSACHs.layAnhSach();

            return View(listSach);
        }

        

        private List<DANHMUC_CHILDs> layDanhMucChile()
        {
            SqlConnection conn = new SqlConnection(constr);
            SqlDataAdapter s = new SqlDataAdapter("SELECT * FROM DANHMUC_CHILD", conn);
            DataTable dataTable = new DataTable();
            List<DANHMUC_CHILDs> ldmc = new List<DANHMUC_CHILDs>();
            s.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
            {
                DANHMUC_CHILDs dANHMUC = new DANHMUC_CHILDs()
                {
                    MADANHMUC = (int)row["MADANHMUC"],
                    MADANHMUC_CHILD = row["MADANHMUC_CHILD"].ToString(),
                    TENDANHMUC = row["TENDANHMUC"].ToString()
                };
                ldmc.Add(dANHMUC);
            }    

            
            return ldmc;
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DanhMucPartial()
        {
            SqlConnection conn = new SqlConnection(constr);
            List<DANHMUCs> listDanhMuc = new List<DANHMUCs>();
            SqlDataAdapter danhmuc = new SqlDataAdapter("Select * from DANHMUC", conn);
            DataTable dttable = new DataTable();
            danhmuc.Fill(dttable);

            foreach(DataRow row in dttable.Rows)
            {
                DANHMUCs dm = new DANHMUCs()
                {
                 
                    MADANHMUC = (int)row["MADANHMUC"],
                    TENDANHMUC = row["TENDANHMUC"].ToString()
                  

                };
                listDanhMuc.Add(dm);
            }

            ViewBag.DanhMucChild = layDanhMucChile();

            return PartialView("DanhMucPartial", listDanhMuc);
        }

        public ActionResult TacGia()
        {
            SqlConnection conn = new SqlConnection(constr);
            List<SACHs> listSach = new List<SACHs>();
            SqlDataAdapter sach = new SqlDataAdapter("Select * from SACH", conn);
            DataTable dataTable = new DataTable();
            sach.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                SACHs s = new SACHs()
                {
                   
                    TACGIA = row["TACGIA"].ToString(),
                   
                };
                listSach.Add(s);
            }

            
            var distg = listSach.GroupBy(x => x.TACGIA)
                                .Select(g => g.First())
                                .ToList();

            ViewBag.Tacgia = distg;
            return View();
        }


        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult DetailAction(int id)
        {
            
            var sp = SACHs.layDanhSachSanPham().FirstOrDefault(x=>x.MASACH == id);
            ViewBag.AS = ANHSACHs.layAnhSach();

            var sanphamtuongtu = SACHs.layDanhSachSanPham()
                          .Where(x => x.MADANHMUC == sp.MADANHMUC && x.MASACH != id)
                          .ToList();
            ViewBag.SanPhamTuongTu = sanphamtuongtu;
            string path = Server.MapPath("~" + sp.MOTAFILE);

            ViewBag.HienThiCT = System.IO.File.ReadAllText(path);
            if (Session["role"] != null && Session["role"].ToString().Trim() == "KhachHang")
            {
                ViewBag.DiaChi = Session["DiaChi"].ToString();
            }    
            return View("Detail", sp);
        }

      
        public ActionResult EditKhoNhanVien(int id)
        {
            var sp = SACHs.layDanhSachSanPham().FirstOrDefault(x => x.MASACH == id);
            return View(sp);
        }
    }
}