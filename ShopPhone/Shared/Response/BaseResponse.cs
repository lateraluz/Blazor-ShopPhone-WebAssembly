using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Response;

public class BaseResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

public class BaseResponseGeneric<TData> : BaseResponse
{
    public TData? Data { get; set; }
}

