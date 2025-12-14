using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOOKSTORE___ONLINE.Models.DB;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class NhapHangSach
    {
        public SACH Sach { get; set; }
        public int SoLuong { get; set; }
        public double TongTien { get; set; }
    }
}