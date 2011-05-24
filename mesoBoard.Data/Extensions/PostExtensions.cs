
namespace mesoBoard.Data
{
    public partial class Post
    {
        public bool IsReported
        {
            get
            {
                return this.ReportedPost != null;
            }
        }
    }
}
