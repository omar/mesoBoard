using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public static class Misc
    {
        public static void ParseBBCodeScriptFile(Theme theme)
        {
            string validHighlighters = SiteConfig.SyntaxHighlighting.Value;
            string[] highlighters = validHighlighters.ToLower().Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] options = SiteConfig.SyntaxHighlighting.Options.ToLower().Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] tohide = options.Except(highlighters).ToArray();
            
            string originalFile;

            string path = HostingEnvironment.MapPath(("~/Themes/" + theme.FolderName + "/Content/Scripts/bbcode-set.js"));

            using(FileStream file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using(StreamReader reader = new StreamReader(file))
            {
                originalFile = reader.ReadToEnd();
            }

            string[] splitText = originalFile.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<string> found = new List<string>();

            foreach (string h in tohide)
            {
                found.AddRange(splitText.Where(x => x.Contains(h)).ToList());
            }

            found = found.Distinct().ToList();

            foreach (string s in found)
            {
                originalFile = originalFile.Replace(s, "// " + s);
            }

            string newPath = HostingEnvironment.MapPath(("~/Themes/" + theme.FolderName + "/Content/Scripts/bbcode-set-parsed.js"));
            using (StreamWriter writeit = File.CreateText(newPath))
            {
                writeit.Write(originalFile);
            }
        }
      
    }
}
