using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Multitenant.Domain.Abstract
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string TenantId { get; set; }
    }
}
