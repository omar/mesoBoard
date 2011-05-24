using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace mesoBoard.Services
{
    public class ThreadImageServices 
    {
        public ThreadImageServices()
        {
        }

        public string[] GetThreadImages()
        {
            string[] validExtensions = new string[] { ".gif", ".png", ".jpg", ".jpeg" };
            DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~/Images/ThreadImages"));
            return di.GetFiles().Select(x => x.Name).Where(x=> validExtensions.Contains(Path.GetExtension(x))).OrderBy(x => x).ToArray();
        }
       
    }
}