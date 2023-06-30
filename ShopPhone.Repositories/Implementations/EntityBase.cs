using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Repositories.Implementations;
public class EntityBase
{
    public int Id { get; set; }
    public bool Status { get; set; }

    protected EntityBase()
    {
        Status = true;
    }
}