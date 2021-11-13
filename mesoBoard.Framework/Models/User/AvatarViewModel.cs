using mesoBoard.Data;
using Microsoft.AspNetCore.Http;

namespace mesoBoard.Framework.Models
{
    public class AvatarViewModel
    {
        public User CurrentUser { get; set; }

        public AvatarType AvatarType { get; set; }

        public int WidthMax { get; set; }

        public int HeightMax { get; set; }

        public string Url { get; set; }

        public IFormFile Image { get; set; }
    }
}