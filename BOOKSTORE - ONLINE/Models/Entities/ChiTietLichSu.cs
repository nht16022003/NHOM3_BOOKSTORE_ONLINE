using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class ChiTietLichSu
    {
        public int MaKH {  get; set; }

        public string TenKH { get; set; }

        public int MADONHANG { get; set; }

        public string TENSACH { get; set; }

        public int SOLUONG {  get; set; }

        public Decimal GIATAITHOIDIEMMUA { get; set; }

        public Decimal tongTien => SOLUONG * GIATAITHOIDIEMMUA;

        public DateTime NgayDat { get; set; }

        public string TRANGTHAI {get; set; }
    }
}