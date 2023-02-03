using BulkyBook.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BusinessObject.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartProduct>? Products { get; set; }
    }
}
