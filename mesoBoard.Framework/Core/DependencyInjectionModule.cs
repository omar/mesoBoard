using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Data.Repositories;
using mesoBoard.Framework.Core.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace mesoBoard.Framework.Core
{
    public static class DependencyInjectionModule
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddDbContext<mbEntities>(options => options.UseSqlServer(Settings.ConnectionString));
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
            services.AddScoped<IUnitOfWork>(sp => sp.GetService<mbEntities>());
            System.Console.WriteLine("Added IUnitOfWork");
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