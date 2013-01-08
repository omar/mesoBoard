using System.Collections.Generic;
using System.Linq;

namespace mesoBoard.Common
{
    public enum PollingPermissionValue
    {
        None = 0,
        Vote,
        Create
    }

    public class PollingPermission : PermissionBase<PollingPermissionValue>
    {
        public PollingPermission(PollingPermissionValue value, string name) : base(value, name) { }
    }

    public class PollingPermissions : PermissionCollection<PollingPermissionValue, PollingPermission>
    {
        public static PollingPermission None = new PollingPermission(PollingPermissionValue.None, "None");
        public static PollingPermission Vote = new PollingPermission(PollingPermissionValue.Vote, "Vote");
        public static PollingPermission Create = new PollingPermission(PollingPermissionValue.Create, "Create");

        public new static IEnumerable<PollingPermission> List
        {
            get
            {
                yield return None;
                yield return Vote;
                yield return Create;
            }
        }

        public static PermissionCollection<PollingPermissionValue, PollingPermission> Class =
             new PermissionCollection<PollingPermissionValue, PollingPermission>(List.ToList());
    }
}