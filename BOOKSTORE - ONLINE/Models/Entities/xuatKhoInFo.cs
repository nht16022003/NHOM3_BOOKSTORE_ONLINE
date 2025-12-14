using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class xuatKhoInFo
    {
        public int MADONHANG { get; set; }
        public string TENNHANVIEN { get; set; }
         
        public string TENKHACHHANG { get; set; }

        public int MASACH {  get; set; }

        public int SOLUONG { get; set; }

        public decimal GIATAITHOIDIEMMUA { get; set; }

        public string DIACHI {  get; set; }

        public string TRANGTHAI {  get; set; }

        public int TONKHO {  get; set; }

        public decimal TONGTIEN {
            get { return tongTien(); }
        }

        public decimal tongTien()
        {
            return GIATAITHOIDIEMMUA * SOLUONG;
        }

    }
}