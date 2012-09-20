using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class ForumPermissionViewModel
    {
        public int ForumPermissionID { get; set; }

        [Required(ErrorMessage = "Select a forum.")]
        public int ForumID { get; set; }

        public IEnumerable<Forum> Forums { get; set; }

        public SelectList ForumsList
        {
            get
            {
                return new SelectList(Forums, "ForumID", "Name");
            }
        }

        [Required(ErrorMessage = "Select a role.")]
        public int RoleID { get; set; }

        [Required]
        public int Visibility { get; set; }

        [Required]
        public int Posting { get; set; }

        [Required]
        public int Attachments { get; set; }

        [Required]
        public int Polling { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        public SelectList RolesList
        {
            get
            {
                return new SelectList(Roles, "RoleID", "Name");
            }
        }

        public SelectList AttachmentPermissionsList { get { return new SelectList(AttachmentPermissions.List, "Value", "Name"); } }

        public SelectList PollingPermissionsList { get { return new SelectList(PollingPermissions.List, "Value", "Name"); } }

        public SelectList PostingPermissionsList { get { return new SelectList(PostingPermissions.List, "Value", "Name"); } }

        public SelectList VisibilityPermissionList { get { return new SelectList(VisibilityPermissions.List, "Value", "Name"); } }
    }
}