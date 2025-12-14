using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class ANHSACHs
    {
        static string constr = ConfigurationManager.ConnectionStrings["QLWSACH"].ConnectionString;
        public int MASACH {  get; set; }
        public string LINKANH { get; set; }

        public static List<ANHSACHs> layAnhSach()
        {
            SqlConnection conn = new SqlConnection(constr);
            SqlDataAdapter asa = new SqlDataAdapter("SELECT * FROM ANHSACH", conn);
            DataTable dataTable = new DataTable();
            List<ANHSACHs> listSach = new List<ANHSACHs>();
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
    }
}