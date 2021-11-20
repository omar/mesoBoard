using mesoBoard.Common;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Framework.Core
{
    [AllowOffline]
    [PermissionAuthorize(SpecialPermissionValue.Administrator)]
    [Area("Admin")]
    public class BaseAdminController : BaseController
    {
        
    }
}
