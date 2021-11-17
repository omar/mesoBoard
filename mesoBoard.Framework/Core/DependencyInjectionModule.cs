using System;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Data.Repositories;
using mesoBoard.Framework.Core.IoC;
using mesoBoard.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace mesoBoard.Framework.Core
{
    public static class DependencyInjectionModule
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddDbContext<mesoBoardContext>(options => 
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(Settings.ConnectionString)
            );
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
            services.AddScoped<IUnitOfWork>(sp => sp.GetService<mesoBoardContext>());
            return services;
        }

        public static IServiceCollection AddWebComponents(this IServiceCollection services)
        {
            services.AddScoped<ThemeFactory>();
            services.AddScoped<UserFactory>();
            services.AddScoped<TrackActivityFilter>();

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

        public static IServiceCollection AddConcreteTypes(this IServiceCollection services)
        {
            AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.StartsWith("mesoBoard.Services"))
                .SelectMany(a => a.GetTypes())
                .Where(t => 
                        t.Namespace != null 
                    && t.Namespace.StartsWith("mesoBoard.") 
                    && !t.IsAbstract 
                    && t.IsPublic
                )
                .Select(t => services.AddScoped(t))
                .ToList();

            return services;
        }
    }
}