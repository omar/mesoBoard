//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mesoBoard.Data
{
    using System;
    using System.Collections.Generic;

    public partial class Message
    {
        public int MessageID { get; set; }

        public Nullable<int> FromUserID { get; set; }

        public Nullable<int> ToUserID { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public bool IsRead { get; set; }

        public System.DateTime DateSent { get; set; }

        public bool ToDelete { get; set; }

        public bool FromDelete { get; set; }

        public virtual User FromUser { get; set; }

        public virtual User ToUser { get; set; }
    }
}