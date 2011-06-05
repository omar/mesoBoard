using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mesoBoard.Common;

namespace mesoBoard.Services
{
    public class BaseService
    {
        protected IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
