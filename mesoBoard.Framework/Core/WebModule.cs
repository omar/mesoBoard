using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Data.Repositories;
using Ninject.Modules;
using Ninject.Parameters;

namespace mesoBoard.Framework.Core
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            ConstructorArgument parameter = new ConstructorArgument("connectionString", Settings.EntityConnectionString);
            Bind<mbEntities>().ToSelf().InRequestScope().WithParameter(parameter);
            Bind<IStoredProcedures>().To<StoredProcedures>().InRequestScope();
            Bind(typeof(IRepository<>)).To(typeof(EntityRepository<>)).InRequestScope();
        }
    }
}