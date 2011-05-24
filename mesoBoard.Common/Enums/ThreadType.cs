using System.Linq;
using System.Collections.Generic;

namespace mesoBoard.Common
{
    public enum ThreadType
    {
        Regular = 1,
        Sticky = 2,
        Announcement = 3,
        GlobalAnnouncement = 4,
    }

    public class ThreadTypes
    {
        public static ThreadTypes Regular = new ThreadTypes("Regular", ThreadType.Regular);
        public static ThreadTypes Sticky = new ThreadTypes("Sticky", ThreadType.Sticky);
        public static ThreadTypes Announcement = new ThreadTypes("Announcement", ThreadType.Announcement);
        public static ThreadTypes GlobalAnnouncement = new ThreadTypes("Global Announcement", ThreadType.GlobalAnnouncement);

        public static IEnumerable<ThreadTypes> List
        {
            get
            {
                yield return Regular;
                yield return Sticky;
                yield return Announcement;
                yield return GlobalAnnouncement;                
            }
        }

        public static ThreadTypes Get(int value)
        {
            return List.FirstOrDefault(item => item.Value == value);
        }

        public static ThreadTypes Get(string name)
        {
            return List.FirstOrDefault(item => item.Name == name);
        }
        
        public string Name { get; set; }
        public int Value { get; set; }
        public ThreadType Type { get; set; }

        public ThreadTypes(string name, ThreadType type)
        {
            Name = name;
            Value = (int)type;
            Type = type;
        }
    }
}
