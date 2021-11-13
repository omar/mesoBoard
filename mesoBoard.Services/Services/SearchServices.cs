using System.Collections.Generic;
using mesoBoard.Common;
using mesoBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace mesoBoard.Services
{
    public class SearchServices : BaseService
    {
        private mbEntities _dataContext;

        public SearchServices(
            mbEntities dataContext,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _dataContext = dataContext;
        }

        // TODO: Add more functionality to search

        public IEnumerable<Post> SearchPosts(string keywords)
        {
            string searchQuery = @"SELECT DISTINCT * FROM Posts
                                   WHERE FREETEXT(Posts.TextOnly, {0})";

            var results = _dataContext.Posts.FromSqlRaw(searchQuery, keywords);
            return results;
        }
    }
}