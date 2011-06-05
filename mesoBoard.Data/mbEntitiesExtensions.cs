using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mesoBoard.Common;

namespace mesoBoard.Data
{
    public partial class mbEntities : IUnitOfWork
    {
        public mbEntities(string connectionString) : base(connectionString)
        {

        }

        public void Commit()
        {
            this.SaveChanges();
        }
    }
}
