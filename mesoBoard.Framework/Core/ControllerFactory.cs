using System;
using System.Web.Mvc;
using System.Linq;
using mesoBoard.Common;
using Ninject;

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
                requestContext.HttpContext.Items[HttpContextItemKeys.PluginFolder] = pluginController.FolderName;
                    requestContext.HttpContext.Items.Remove(HttpContextItemKeys.PluginFolder);
            }

            return controller.GetType();   
        }
    }
}