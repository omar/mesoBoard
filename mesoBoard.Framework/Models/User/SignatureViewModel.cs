using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mesoBoard.Framework.Models
{
    public class SignatureViewModel
    {
        public string ParsedSignature { get; set; }

        public string Signature { get; set; }

        public bool? Preview { get; set; }

        public string PreviewParsedSignature { get; set; }
    }
}