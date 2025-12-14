using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class GIOHANG_CHITIET
    {
       public int MaGioHangChiTiet { get; set; }
       public int MaGioHang {  get; set; }
       public int MaSach { get; set; }
        public int SoLuong { get; set; }
    }
}