using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class DONHANGVIEWMODEL
    {
        public int MADONHANG { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string TRANGTHAI { get; set; }

        public string TENSACH { get; set; }
        public int SOLUONG { get; set; }
        public decimal GIATAITHOIDIEMMUA { get; set; }
    }
}