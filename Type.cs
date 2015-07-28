using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MManatee.Models
{
    public class Type
    {
        public int TypeID { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set;}
    }
}