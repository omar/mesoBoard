using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace mesoBoard.Common
{
    public enum AttachmentPermissionValue
    {
        None = 0,
        Download,
        Upload
    }

    public class AttachmentPermission : PermissionBase<AttachmentPermissionValue>
    {
        public AttachmentPermission(AttachmentPermissionValue value, string name) : base(value, name) { }
    }

    public class AttachmentPermissions : PermissionCollection<AttachmentPermissionValue, AttachmentPermission>
    {
        public static AttachmentPermission None = new AttachmentPermission(AttachmentPermissionValue.None, "None");
        public static AttachmentPermission Download = new AttachmentPermission(AttachmentPermissionValue.Download, "Download");
        public static AttachmentPermission Upload = new AttachmentPermission(AttachmentPermissionValue.Upload, "Upload");

        public new static IEnumerable<AttachmentPermission> List
        {
            get
            {
                yield return None;
                yield return Download;
                yield return Upload;
            }
        }

        public static PermissionCollection<AttachmentPermissionValue, AttachmentPermission> Class =
            new PermissionCollection<AttachmentPermissionValue, AttachmentPermission>(List.ToList());
    }
}