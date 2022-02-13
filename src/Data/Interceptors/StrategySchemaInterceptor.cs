﻿using EFCore.Multitenant.Provider;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Multitenant.Data.Interceptors
{
    public class StrategySchemaInterceptor : DbCommandInterceptor
    {
        private readonly TenantData _tenant;

        public StrategySchemaInterceptor(TenantData tenant)
        {
            _tenant = tenant;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            ReplaceSchema(command);

            return base.ReaderExecuting(command, eventData, result);
        }

        private void ReplaceSchema(DbCommand command)
        {
            //FROM PRODUCTS -> FROM [tenant-1].PRODUCTS 
            command.CommandText = command.CommandText.Replace("FROM ", $" FROM {_tenant.TenantId}.")
                                                     .Replace("JOIN ", $" JOIN {_tenant.TenantId}.");
        }
    }
}
