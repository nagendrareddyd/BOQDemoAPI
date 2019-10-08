using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOQ.API.Services.DTO
{
    public class Collection
    {
        public string version { get; set; }
        public List<object> links { get; set; }
        public List<Item> items { get; set; }
        public List<object> queries { get; set; }
        public Template template { get; set; }
    }
}
