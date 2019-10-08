using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOQ.API.Services.DTO
{
    public class Item
    {
        public string href { get; set; }
        public List<Data> data { get; set; }
        public List<Link> links { get; set; }
    }
}
