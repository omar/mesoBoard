using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mesoBoard.Framework.Models
{
    public interface IThreadEditor : IEditor
    {
        ThreadEditorViewModel ThreadEditor { get; set; }
    }
}