using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityToolShedMvc.Models
{
    public class Tool
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Warnings { get; set; }
        public int CategoriesId { get; set; }
        //public int CommunityPersonId { get; set; }
        public int AgeId { get; set;}
        public string Age { get; set; }
        public List<Tool> BorrowedTools { get; set; }
    }
}