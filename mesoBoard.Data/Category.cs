using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Category
    {
        public Category()
        {
            Forums = new HashSet<Forum>();
        }

        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public virtual ICollection<Forum> Forums { get; set; }
    }
}
