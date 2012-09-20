using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;

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
                                   WHERE FREETEXT(Posts.TextOnly, @keyword)";

            var parameter = new SqlParameter() { ParameterName = "keyword", Value = keywords };
            var results = _dataContext.Posts.SqlQuery(searchQuery, parameter);
            return results;
        }
    }
}