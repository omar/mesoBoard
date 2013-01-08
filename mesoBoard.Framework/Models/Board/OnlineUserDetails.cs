using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class OnlineUserDetails
    {
        public OnlineUser OnlineUser { get; set; }

        public Role DefaultRole { get; set; }

        public string Color
        {
            get
            {
                if (DefaultRole == null || DefaultRole.Rank == null)
                    return string.Empty;

                return DefaultRole.Rank.Color;
            }
        }
    }
}