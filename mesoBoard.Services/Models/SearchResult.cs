
namespace mesoBoard.Services
{
    public class SearchResult
    {
        public string ThreadTitle { get; set; }
        public int PostID { get; set; }
        public int ThreadID { get; set; }
        public string Text { get; set; }

        public SearchResult(string title, int threadID, string text, int postID)
        {
            this.ThreadTitle = title;
            this.ThreadID = threadID;
            this.PostID = postID;
            this.Text = text;
        }

        public SearchResult()
        {
        }
    }
}
