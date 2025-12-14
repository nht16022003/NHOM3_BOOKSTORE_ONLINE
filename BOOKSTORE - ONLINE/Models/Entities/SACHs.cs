using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class SACHs
    {
        static string constr = ConfigurationManager.ConnectionStrings["QLWSACH"].ConnectionString;
       



        public static List<BOOKSTORE___ONLINE.Models.Entities.SACHs> layDanhSachSanPham()
        {

            SqlConnection conn = new SqlConnection(constr);
            List<SACHs> SACHs = new List<SACHs>();
            SqlDataAdapter sach = new SqlDataAdapter("Select * from SACH", conn);
            DataTable dataTable = new DataTable();
            sach.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
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
                    NGAYCAPNHAT = row["NGAYCAPNHAT"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["NGAYCAPNHAT"]) : null,
                    MOTAFILE = row["MOTAFILE"]?.ToString() ?? ""
                };
                SACHs.Add(s);
            }
            return SACHs;
        }

       

        public int MASACH { get; set; }
        public int MADANHMUC { get; set; }
        public string MADANHMUC_CHILD { get; set; }  // từ int? thành string
                                                     // nullable vì có thể NULL
        public string TENSACH { get; set; }
        public string TACGIA { get; set; }
        public string NHAXUATBAN { get; set; }
        public decimal? GIABAN { get; set; }       // nullable
        public int? SOLUONGTONKHO { get; set; }    // nullable
        public string MOTACHITIET { get; set; }
        public DateTime NGAYTHEMSANPHAM { get; set; }
        public DateTime? NGAYCAPNHAT { get; set; } // nullable
        public string ANHSANPHAM { get; set; }

        public string MOTAFILE { get; set; }

        public string DiaChiGiaoHang {  get; set; }

        public int MANHACUNGCAP { get; set; }
    }

}