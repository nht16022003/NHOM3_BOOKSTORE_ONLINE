using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace BOOKSTORE___ONLINE.Models.Entities
{
    public class Cart
    {
        public List<CartItem> list;
        public Cart()
        {
            list = new List<CartItem>();
        }
        public Cart(List<CartItem> _list)
        {
            list = _list;
        }
        public int SoLuongMH()
        {
            return list.Count;
        }
        public int TongSoLuongSP()
        {
            return list.Sum(x => x.SOLUONG);
        }
        public decimal TongThanhTien()
        {
            return list.Sum(x => x.ThanhTien);
        }
        public int Them(int id)
        {
            CartItem sp = list.Find(x => x.MASACH == id);
            try
            {
                if (sp != null)//đã tồn tại trong giỏ hàng
                {
                    sp.SOLUONG++;
                }
                else
                {
                    CartItem newItem = new CartItem(id);
                    list.Add(newItem);
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 1; //thêm thành công
        }
        public int Xoa(int id)
        {
            CartItem sp = list.Find(x => x.MASACH == id);
            try
            {
                if (sp != null)
                {
                    list.Remove(sp);
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 1; //xóa thành công
        }
        public int Giam(int id)
        {
            CartItem sp = list.Find(x => x.MASACH == id);
            try
            {
                if (sp != null)
                {
                    sp.SOLUONG--;
                    if (sp.SOLUONG <= 0)
                    {
                        list.Remove(sp);
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1; //xóa thành công
        }
    }
}