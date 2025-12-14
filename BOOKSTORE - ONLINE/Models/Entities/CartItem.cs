using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class CartItem
    {
        static string constr = ConfigurationManager.ConnectionStrings["QLWSACH"].ConnectionString;


        public int MASACH {  get; set; }

        public string TENSACH { get; set; }

        public string ANHDAIDIEN { get; set; }

        public int SOLUONG { get; set; }

        public decimal GIABAN   { get; set; }

        public decimal ThanhTien
        {
            get
            {
                return SOLUONG * GIABAN;
            }
        }

        public CartItem(int id)
        {
            SACHs sach = SACHs.layDanhSachSanPham().FirstOrDefault(x => x.MASACH == id);
            if (sach != null)
            {
                MASACH = sach.MASACH;
                TENSACH = sach.TENSACH;
                ANHDAIDIEN = sach.ANHSANPHAM;
                SOLUONG = 1;
                GIABAN = sach.GIABAN.Value;
            }
        }
    }
}