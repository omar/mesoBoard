using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class CategoryServices : BaseService
    {
        private IRepository<Category> _categoryRepository;

        public CategoryServices(
            IRepository<Category> categoryRepository,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _categoryRepository = categoryRepository;
        }

        public Category CreateCategory(string name, string description)
        {
            int order = GetLastOrder();
            Category category = new Category()
            {
                Name = name,
                Description = description,
                Order = order + 1
            };

            _categoryRepository.Add(category);
            _unitOfWork.Commit();
            return category;
        }

        public int GetLastOrder()
        {
            Category lastCategory = _categoryRepository.Get().OrderByDescending(item => item.Order).FirstOrDefault();
            if (lastCategory == null)
                return 0;
            else
                return lastCategory.Order;
        }

        public IEnumerable<Category> GetViewableCategories(User currentUser)
        {
            IEnumerable<Category> AllCategories = _categoryRepository.Get();

            return null;
            // return AllCategories.Where(x => x.Forums.Count != 0 && x.Forums.Any(y => y.CanView(currentUser))).OrderBy(x => x.Order);
        }
    }
}