using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mesoBoard.Framework.Models
{
    public interface IPostEditor : IEditor
    {
        PostEditorViewModel PostEditor { get; set; }
    }
}