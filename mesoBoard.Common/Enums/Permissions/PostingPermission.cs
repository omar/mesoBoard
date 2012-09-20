using System.Collections.Generic;
using System.Linq;

namespace mesoBoard.Common
{
    public enum PostingPermissionValue
    {
        None = 0,
        Reply,
        Thread,
        Sticky,
        Announcments
    }

    public class PostingPermission : PermissionBase<PostingPermissionValue>
    {
        public PostingPermission(PostingPermissionValue value, string name) : base(value, name) { }
    }

    public class PostingPermissions : PermissionCollection<PostingPermissionValue, PostingPermission>
    {
        public static PostingPermission None = new PostingPermission(PostingPermissionValue.None, "None");
        public static PostingPermission Reply = new PostingPermission(PostingPermissionValue.Reply, "Reply");
        public static PostingPermission Thread = new PostingPermission(PostingPermissionValue.Thread, "Thread");
        public static PostingPermission Sticky = new PostingPermission(PostingPermissionValue.Sticky, "Sticky");
        public static PostingPermission Announcments = new PostingPermission(PostingPermissionValue.Announcments, "Announcments");

        public new static IEnumerable<PostingPermission> List
        {
            get
            {
                yield return None;
                yield return Reply;
                yield return Thread;
                yield return Sticky;
                yield return Announcments;
            }
        }

        public static PermissionCollection<PostingPermissionValue, PostingPermission> Class =
             new PermissionCollection<PostingPermissionValue, PostingPermission>(List.ToList());
    }
}