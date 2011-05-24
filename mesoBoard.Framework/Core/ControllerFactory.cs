using System;
using System.Web.Mvc;
using System.Linq;
using mesoBoard.Common;
using Ninject;
using Ninject.Web.Mvc;

namespace mesoBoard.Framework.Core
{
    public class ControllerFactory : System.Web.Mvc.DefaultControllerFactory
    {
        IKernel _kernel;

        public ControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override Type GetControllerType(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            Type controllerType = base.GetControllerType(requestContext, controllerName);
            
            if (controllerType != null)
                return controllerType;

            var controllers = _kernel.GetAll<IController>().ToList();


            var controller =  _kernel.TryGet<IController>(controllerName + "Controller");
            
            if(controller == null)
                return base.GetControllerType(requestContext, controllerName);
            else if (controller is IPluginController)
            {
                IPluginController pluginController = (IPluginController)controller;

                if (requestContext.HttpContext.Items.Contains(HttpContextItemKeys.PluginFolder))
                    requestContext.HttpContext.Items.Remove(HttpContextItemKeys.PluginFolder);

                requestContext.HttpContext.Items.Add(HttpContextItemKeys.PluginFolder, pluginController.FolderName);
            }

            return controller.GetType();   
        }
    }
}