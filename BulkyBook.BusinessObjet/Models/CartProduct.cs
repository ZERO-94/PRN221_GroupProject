using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BulkyBook.BusinessObject.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        [Range(1,1000)]
        public int Total { get; set; } = 1;

        [JsonIgnore]
        public Product Product { get; set; }
    }
}
