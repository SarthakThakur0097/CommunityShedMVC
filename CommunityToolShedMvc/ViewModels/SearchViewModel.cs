using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMvc.ViewModels
{
    public class SearchViewModel
    {
       
        public string ItemName { get; set; }
        public int CategoryId { get; set; }

        public SelectList CategorySelectList {
            get
            {
                return new SelectList(Categories, "Id", "Type");
            }
        }
        public SelectList AgeSelectList
        {
            get
            {
                return new SelectList(Ages, "Id", "Age");
            }
        }

        public List<Category> Categories { get; set; }
        public List<Tool> Tools { get; set; }

        public int AgeId { get; set; }
        public List<AgeRange> Ages { get; set; }

        public void PopulateSelectLists()
        {
            Categories = DatabaseHelper.Retrieve<Category>(@"
                    SELECT c.Id, c.Type
                    FROM Categories c
                    ORDER BY Type
                    ");
            Ages = DatabaseHelper.Retrieve<AgeRange>(@"
                SELECT a.Id, a.Age
                FROM AgeRange a
                   ");
        }
    }
}