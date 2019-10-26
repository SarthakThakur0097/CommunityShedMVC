using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityToolShedMvc.ViewModels
{
    public class AddCommunityViewModel
    {
        public int Id
        {
            get;set;
        }
        public string Name
        {
            get;set;
        }
        public string Availability
        {
            get;
            set;
        }
    }
}