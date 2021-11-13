using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Data.Repositories;
using mesoBoard.Framework.Core.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace mesoBoard.Framework.Core
{
    public static class SetupDependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddScoped<DbContext, mbEntities>();
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
            services.AddScoped<IUnitOfWork, mbEntities>();
            return services;
        }

        public static IServiceCollection AddWebComponents(this IServiceCollection services)
        {
            services.AddScoped<User>(sp => {
                var factory = sp.GetService<UserFactory>();
                return factory.GetUserAsync().Result;
            });

            services.AddScoped<Theme>(sp => {
                var factory = sp.GetService<ThemeFactory>();
                return factory.GetTheme();
            });
            return services;
        }
    }
}