using System.Collections.Generic;

namespace mesoBoard.Data
{
    public interface IStoredProcedures
    {
        IEnumerable<OnlineGuest> Get_Inactive_OnlineGuests();
        IEnumerable<OnlineUser> Get_Inactive_OnlineUsers();
    }
}
