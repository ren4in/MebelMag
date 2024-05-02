using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MebelMag;

public partial class Customer
{
    [JsonIgnore]

    public int? IdCustomer { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool ShouldSerializeIdUser()
    {
        // Возвращаем true, если IdUser не равен null (для обновления)
        // Возвращаем false, если IdUser равен null (для создания)
        return IdCustomer.HasValue;
    }
    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string? EMail { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
