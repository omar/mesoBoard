using System.Collections.Generic;
using System.Linq;

namespace mesoBoard.Data.Repositories
{
    public class StoredProcedures : IStoredProcedures
    {
        private mbEntities db;

        public StoredProcedures(mbEntities dataContext)
        {
            this.db = dataContext;
        }

        public IEnumerable<OnlineUser> Get_Inactive_OnlineUsers()
        {
            return this.db.Get_Inactive_OnlineUsers().AsEnumerable();
        }

        public IEnumerable<OnlineGuest> Get_Inactive_OnlineGuests()
        {
            return this.db.Get_Inactive_OnlineGuests().AsEnumerable();
        }
        
    }
}
