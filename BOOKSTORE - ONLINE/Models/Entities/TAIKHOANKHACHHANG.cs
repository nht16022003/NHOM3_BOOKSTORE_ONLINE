using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class TAIKHOANKHACHHANG
    {
        public int MaTK {  get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; }
        public int MaKH { get; set; }
    }
}