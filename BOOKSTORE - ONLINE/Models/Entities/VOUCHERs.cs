using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class VOUCHERs
    {
       public int MaVoucher {  get; set; }
        public string TenVoucher { get; set; }
        public decimal GiamGia { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string  TrangThai { get; set; }
    }
}