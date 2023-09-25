using System;
using System.Collections.Generic;

namespace ShopPhone.DataAccess;

public partial class Configuration
{
    public int Id { get; set; }

    public string Uri { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? DiscoveryService { get; set; }
}
