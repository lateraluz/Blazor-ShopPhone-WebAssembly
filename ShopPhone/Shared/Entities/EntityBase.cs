using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Entities;

public  class EntityBase
{        
    public bool Status { get; set; }

    protected EntityBase()
    {
        Status = true;
    }
}
