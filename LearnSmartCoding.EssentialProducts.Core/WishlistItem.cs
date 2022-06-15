using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearnSmartCoding.EssentialProducts.Core
{
    public class WishlistItem
    {
        public long Id { get; set; }
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }
        [MaxLength(200)]
        public string OwnerADObjectId { get; set; }
    }
}
