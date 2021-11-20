using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mesoBoard.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class RoleViewModel
    {
        public int RoleID { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsGroup { get; set; }

        public byte SpecialPermissions { get; set; }

        public SelectList SpecialPermissionsList
        {
            get
            {
                return new SelectList(mesoBoard.Common.SpecialPermissions.List, "Value", "Name");
            }
        }

        public int? RankID { get; set; }

        public IEnumerable<Rank> Ranks { get; set; }

        public SelectList RanksList
        {
            get
            {
                return new SelectList(Ranks, "RankID", "Title");
            }
        }

        public IEnumerable<User> Users { get; set; }
    }
}