using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class TAIKHOAN_ADMIN
    {
        public int MaTK {  get; set; }

        public int MAADMIN { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; }
    }
}