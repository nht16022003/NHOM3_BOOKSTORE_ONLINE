using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class DONHANGs
    {
       public int MADONHANG {  get; set; }

       public int MANV {  get; set; }

       public int MaKH { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public string MAPHUONGPHUC { get; set; }
        public string DiaChiGiaoHang { get; set; }

        public string TRANGTHAI { get; set; }

      
    }
}