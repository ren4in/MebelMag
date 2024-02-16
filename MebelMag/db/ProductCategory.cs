using System;
using System.Collections.Generic;

namespace MebelMag;

public partial class ProductCategory
{
    public int IdProductCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
