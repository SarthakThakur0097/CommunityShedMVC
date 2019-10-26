using CommunityToolShedMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace CommunityToolShedMvc.Security
{
    public class CustomPrincipal: IPrincipal
    { 
        private CustomIdentity identity;
        private Person person;
        
        public CustomPrincipal(CustomIdentity identity, Person person)
        {
            this.identity = identity;
            this.person = person;
        }

        public Person Person
        {
            get
            { return person; }
        }

        //public CustomIdentity CustomIdentity
        //{
        //    get
        //    {
        //        return identity;
        //    }
        //}

        public IIdentity Identity
        {
            get
            {
                return identity;
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public bool IsInRole(int communityId, string role)
        {
            bool roleFound = false;
            foreach (var communityRole in person.Roles)
            {
                if (communityRole.RoleName == role && communityRole.CommunityId == communityId)
                {
                    roleFound = true;
                    break;
                }
            }

            return roleFound;
        }


    }

}