using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Entities;


public class AppConfig
{
    public Storageconfiguration StorageConfiguration { get; set; } = null!;

    public Jwt Jwt { get; set; } = default!;

    public SmtpConfiguration SmtpConfiguration { get; set; } = default!;

    public string WebAppUrl { get; set; } = null!;
}

public class Storageconfiguration
{
    public string PublicUrl { get; set; } = null!;
    public string Path { get; set; } = null!;
}

public class Jwt
{
    public string SecretKey { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
}

public class SmtpConfiguration
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Server { get; set; } = default!;
    public int PortNumber { get; set; }
    public string FromName { get; set; } = default!;
    public bool EnableSsl { get; set; }
}