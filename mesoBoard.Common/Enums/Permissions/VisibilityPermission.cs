using System.Collections.Generic;
using System.Linq;

namespace mesoBoard.Common
{
    public enum VisibilityPermissionValue
    {
        Hidden = 0,
        Visible
    }

    public class VisibilityPermission : PermissionBase<VisibilityPermissionValue>
    {
        public VisibilityPermission(VisibilityPermissionValue value, string name) : base(value, name) { }
    }

    public class VisibilityPermissions : PermissionCollection<VisibilityPermissionValue, VisibilityPermission>
    {
        public static VisibilityPermission Hidden = new VisibilityPermission(VisibilityPermissionValue.Hidden, "Hidden");
        public static VisibilityPermission Visible = new VisibilityPermission(VisibilityPermissionValue.Visible, "Visible");

        public new static IEnumerable<VisibilityPermission> List
        {
            get
            {
                yield return Hidden;
                yield return Visible;
            }
        }

        public static PermissionCollection<VisibilityPermissionValue, VisibilityPermission> Class =
             new PermissionCollection<VisibilityPermissionValue, VisibilityPermission>(List.ToList());
    }
}