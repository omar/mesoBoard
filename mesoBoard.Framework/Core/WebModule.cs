using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Data.Repositories;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject;
using System.Data.Entity;

namespace mesoBoard.Framework.Core
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            ConstructorArgument parameter = new ConstructorArgument("connectionString", Settings.EntityConnectionString);
            Bind<mbEntities>().ToSelf().InRequestScope().WithParameter(parameter);

            Bind<IUnitOfWork>().ToMethod(item => item.Kernel.Get<mbEntities>());
            Bind<DbContext>().ToMethod(item => item.Kernel.Get<mbEntities>());

            Bind<IStoredProcedures>().To<StoredProcedures>().InRequestScope();
            Bind(typeof(IRepository<>)).To(typeof(EntityRepository<>)).InRequestScope();
        }
    }
}