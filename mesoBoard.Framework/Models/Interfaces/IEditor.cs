using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mesoBoard.Framework.Models
{
    public interface IEditor
    {
        EditorType EditorType { get; set; }
    }
}