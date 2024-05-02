using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MebelMag;

public partial class Purchase
{
    [JsonIgnore]
    public int? IdPurchase { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool ShouldSerializeIdPurchase()
    {
        // Возвращаем true, если IdUser не равен null (для обновления)
        // Возвращаем false, если IdUser равен null (для создания)
        return IdPurchase.HasValue;
    }
    public int? IdCustomer { get; set; }

    public DateTime PurchaseDateTime { get; set; }

    public bool Delivery { get; set; }

    public string? Street { get; set; }

    public string? House { get; set; }

    public string? Housing { get; set; }

    public int? Apartment { get; set; }

    public string Status { get; set; } = null!;

    public string? Comment { get; set; }

    public int? IdUser { get; set; }

    public virtual Customer? IdCustomerNavigation { get; set; } = null!;

    public virtual User? IdUserNavigation { get; set; } = null!;

    public virtual ICollection<M2mProductPurchase> M2mProductPurchases { get; set; } = new List<M2mProductPurchase>();
}
