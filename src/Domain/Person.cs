using EFCore.Multitenant.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Multitenant.Domain
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
    }
}
