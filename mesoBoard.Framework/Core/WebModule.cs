using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Data.Repositories;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject;
using Ninject.Web.Mvc;
using Ninject.Web.Mvc.FilterBindingSyntax;
using System.Data.Entity;
using Ninject.Syntax;
using System.Web.Mvc;
using mesoBoard.Services;
using System;
using System.Web;

namespace mesoBoard.Framework.Core
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            BindMvcServices();
            BindDataAccess();
            BindIndicatorAttributes();

            Bind<User>().ToMethod(context => (User)HttpContext.Current.Items[HttpContextItemKeys.CurrentUser]);
            Bind<Theme>().ToMethod(context => (Theme)HttpContext.Current.Items[HttpContextItemKeys.CurrentTheme]);
        }
        
        private void BindMvcServices()
        {
            Bind<IControllerFactory>().To<DefaultControllerFactory>();
            Bind<IViewEngine>().To<ViewEngine>();
        }
        
        private void BindDataAccess()
        {
            ConstructorArgument parameter = new ConstructorArgument("connectionString", Settings.EntityConnectionString);
            Bind<mbEntities>().ToSelf().InRequestScope().WithParameter(parameter);

            Bind<IStoredProcedures>().To<StoredProcedures>().InRequestScope();
            Bind(typeof(IRepository<>)).To(typeof(EntityRepository<>)).InRequestScope();
            Bind<IUnitOfWork>().ToMethod(item => item.Kernel.Get<mbEntities>());
            Bind<DbContext>().ToMethod(item => item.Kernel.Get<mbEntities>());
        }

        private void BindIndicatorAttributes()
        {
            TrackActivityAttribute.Bind(this.Kernel);
            PermissionAuthorizeAttribute.Bind(this.Kernel);
        }
    }
}