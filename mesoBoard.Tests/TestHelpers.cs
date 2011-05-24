using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Linq.Expressions;

namespace mesoBoard.Tests
{
    public static class TestHelpers
    {
        public static Expression<Func<T, bool>> RepositoryIsAny<T>() where T : class
        {
            return It.IsAny<Expression<Func<T, bool>>>();
        }
    }
}
