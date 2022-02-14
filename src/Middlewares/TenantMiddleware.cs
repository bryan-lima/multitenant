using EFCore.Multitenant.Extensions;
using EFCore.Multitenant.Provider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Multitenant.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            TenantData _tenant = httpContext.RequestServices.GetRequiredService<TenantData>();
            _tenant.TenantId = httpContext.GetTenantId();

            await _next(httpContext);
        }
    }
}
