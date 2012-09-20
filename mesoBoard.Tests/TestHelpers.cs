using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using mesoBoard.Common;
using Moq;

namespace mesoBoard.Tests
{
    public static class TestHelpers
    {
        public static Mock<IRepository<T>> MockRepository<T>(List<T> list) where T : class
        {
            var repository = new Mock<IRepository<T>>();

            repository
                .Setup(x => x.Get())
                .Returns(list.AsQueryable());

            repository
                .Setup(x => x.Add(It.IsAny<T>()))
                .Callback((T item) => list.Add(item));

            repository
                .Setup(x => x.Delete(It.IsAny<int>()))
                .Callback((int id) => list.RemoveAt(id));

            repository
                .Setup(x => x.First(It.IsAny<Expression<Func<T, bool>>>()))
                .Returns((Expression<Func<T, bool>> predicate) => list.AsQueryable().FirstOrDefault(predicate));

            repository
                .Setup(x => x.Get(It.IsAny<int>()))
                .Returns((int id) => list[id]);

            repository
                .Setup(x => x.Where(It.IsAny<Expression<Func<T, bool>>>()))
                .Returns((Func<T, bool> predicate) => list.Where(predicate).AsQueryable());

            return repository;
        }

        public static Mock<IUnitOfWork> MockUnitOfWork()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.Commit());
            return unitOfWork;
        }
    }
}