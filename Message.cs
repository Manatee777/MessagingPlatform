using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MManatee.Models
{
    public class Message
    {
        public int MessageID { get; set; }
        public string Subject { get; set; }

        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public string OwnedBy { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateTime { get; set; }

        public bool ReadState { get; set; }

        public virtual Type Type { get; set; }
        public int TypeID { get; set; }

    }
}