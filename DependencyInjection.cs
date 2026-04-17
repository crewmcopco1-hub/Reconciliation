using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDSReconciliation.Repositories.SQL;

namespace VDSReconciliation.Infrastructure
{
    public static class DependencyInjection
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<DbConnectionFactory>();

            services.AddScoped<IDetailRepository, DetailRepository>();
            services.AddScoped<IUserAppAuthorityRepository, UserAppAuthorityRepository>();
            services.AddScoped<IMasterRepository, MasterRepository>();
        }
    }

}
