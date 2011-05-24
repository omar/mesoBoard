using System.Linq;
using System.Collections.Generic;

namespace mesoBoard.Common
{
    public enum SpecialPermissionValue
    {
        None = 0,
        Moderator,
        Administrator
    }

    public class SpecialPermission : PermissionBase<SpecialPermissionValue>
    {
        public SpecialPermission(SpecialPermissionValue value, string name):base(value, name){}
    }

    public class SpecialPermissions
    {
        public static SpecialPermission None =  new SpecialPermission(SpecialPermissionValue.None, "None");
        public static SpecialPermission Moderator = new SpecialPermission(SpecialPermissionValue.Moderator, "Moderator");
        public static SpecialPermission Administrator = new SpecialPermission(SpecialPermissionValue.Administrator, "Administrator");

        public static IEnumerable<SpecialPermission> List
        {
            get
            {
                yield return None;
                yield return Moderator;
                yield return Administrator;
            }
        }

        public static PermissionCollection<SpecialPermissionValue, SpecialPermission> Class =
             new PermissionCollection<SpecialPermissionValue, SpecialPermission>(List.ToList());
    }
}
