using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class KHACHHANGs
    {
       public int MaKH {  get; set; }
       public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string CMND_CCCD { get; set; }
        public string LoaiKhachHang { get; set; }
        public DateTime NgayTao { get; set; }
    }
}