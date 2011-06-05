using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mesoBoard.Common
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
