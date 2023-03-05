using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BusinessObject.ViewModels
{
    public class PaginationViewModel<T>
    {
        public int Total { get; set; }
        public int TotalPage { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
