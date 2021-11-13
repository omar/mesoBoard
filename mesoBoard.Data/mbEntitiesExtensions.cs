using mesoBoard.Common;

namespace mesoBoard.Data
{
    public partial class mbEntities : IUnitOfWork
    {

        public void Commit()
        {
            this.SaveChanges();
        }
    }
}