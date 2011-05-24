using System.Web.Mvc;
using mesoBoard.Common;

namespace mesoBoard.Framework.Core
{
    
    [ValidateInput(false)]
    [AllowOffline]
    [PermissionAuthorize(SpecialPermissionValue.Administrator)]
    public class BaseAdminController : BaseController
    {
        
    }
}
