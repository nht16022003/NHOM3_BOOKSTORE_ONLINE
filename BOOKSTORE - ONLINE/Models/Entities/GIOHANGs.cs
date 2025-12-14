using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class GIOHANGs
    {
        public int MaGioHang {  get; set; }
        public int MaKH { get; set; }
        public DateTime NgayTao { get; set; }
        public string TrangThai { get; set; }
    }
}