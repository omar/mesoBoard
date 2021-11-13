using System.IO;
using System.Linq;
using mesoBoard.Common;

namespace mesoBoard.Services
{
    public class ThreadImageServices : BaseService
    {
        public ThreadImageServices(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public string[] GetThreadImages()
        {
            string[] validExtensions = new string[] { ".gif", ".png", ".jpg", ".jpeg" };
            DirectoryInfo di = new DirectoryInfo("~/Images/ThreadImages");
            return di.GetFiles().Select(x => x.Name).Where(x => validExtensions.Contains(Path.GetExtension(x))).OrderBy(x => x).ToArray();
        }
    }
}