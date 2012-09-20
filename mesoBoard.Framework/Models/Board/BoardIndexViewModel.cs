using System.Collections.Generic;
using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class BoardIndexViewModel
    {
        public IEnumerable<ForumRow> Forums { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}