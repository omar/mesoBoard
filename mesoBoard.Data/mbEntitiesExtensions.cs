using mesoBoard.Common;

namespace mesoBoard.Data
{
    public partial class mesoBoardContext : IUnitOfWork
    {

        public void Commit()
        {
            this.SaveChanges();
        }
    }
}