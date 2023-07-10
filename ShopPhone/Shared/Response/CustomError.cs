using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ShopPhone.Shared.Response;

public class CustomeError
{
    public Errors? Lista { get; set; }
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? TraceId { get; set; }
}

public class Errors
{
    public string[]? Data { get; set; }
}

