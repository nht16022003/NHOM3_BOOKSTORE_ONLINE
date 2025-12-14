using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class PHUONGTHUCTHANHTOANs
    {
        static string constr = ConfigurationManager.ConnectionStrings["QLWSACH"].ConnectionString;
        public int MAPHUONGTHUC { get; set; }
	   public string TENPHUONGTHUC { get; set; }

        public static List<PHUONGTHUCTHANHTOANs> layDanhSachPhuongThucThanhToan()
        {
            SqlConnection conn = new SqlConnection(constr);
            List<PHUONGTHUCTHANHTOANs> Phuongthucthanhtoan = new List<PHUONGTHUCTHANHTOANs>();
            SqlDataAdapter pttt = new SqlDataAdapter("Select * from PHUONGTHUCTHANHTOAN", conn);
            DataTable dataTable = new DataTable();
            pttt.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                PHUONGTHUCTHANHTOANs s = new PHUONGTHUCTHANHTOANs()
                {
                    MAPHUONGTHUC = Convert.ToInt32(row["MAPHUONGTHUC"]),
                    TENPHUONGTHUC = row["TENPHUONGTHUC"]?.ToString() ?? "",
                   
                };
                Phuongthucthanhtoan.Add(s);
            }

            return Phuongthucthanhtoan;
        }
    }
}