using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Multitenant.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetTenantId(this HttpContext httpContext)
        {
            //3 maneiras de fazer:

            //Usando QueryString
            //Exeplo de rota: desenvolvedor.io/product?tenantId=tenant-1
            //var tenant = httpContext.Request.QueryString.Value.Split("/", StringSplitOptions.RemoveEmptyEntries)[0];

            //Usando Header
            //var tenant = httpContext.Request.Headers["tenant-id"];

            //Usando Path
            //Exeplo de rota: desenvolvedor.io/tenant-1/product -> " " / "tenant-1" / "product"
            var tenant = httpContext.Request.Path.Value.Split("/", StringSplitOptions.RemoveEmptyEntries)[0];

            return tenant;
        }
    }
}
