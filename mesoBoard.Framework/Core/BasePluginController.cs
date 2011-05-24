using mesoBoard.Common;

namespace mesoBoard.Framework.Core
{
    public abstract class BasePluginController : BaseController, IPluginController
    {
        public abstract string Name
        {
            get;
        }

        public abstract string FolderName
        {
            get;
        }
    }
}