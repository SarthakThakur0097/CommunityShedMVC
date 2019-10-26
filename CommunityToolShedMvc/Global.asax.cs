using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using CommunityToolShedMvc.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CommunityToolShedMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest()
        {
            IPrincipal user = HttpContext.Current.User;
            if(user.Identity.IsAuthenticated && user.Identity.AuthenticationType == "Forms")
            {
           
                FormsIdentity formsIdenttiy = (FormsIdentity)user.Identity;
                FormsAuthenticationTicket ticket = formsIdenttiy.Ticket;

                CustomIdentity customIdentity = new CustomIdentity(ticket);

                string currentUserEmail = ticket.Name;

                Person person = DatabaseHelper.RetrieveSingle<Person>(@"
                    SELECT Id, FirstName, LastName, Email
                    FROM Person
                    WHERE Email = @Email",
                    new SqlParameter("@Email", currentUserEmail)
                  );
                    person.Roles = DatabaseHelper.Retrieve<CommunityRole>(@"
                     SELECT R.RoleName, CP.CommunityId
                     FROM CommunityPerson CP
                     JOIN Community C ON CP.CommunityId = C.Id
                     JOIN Person P ON CP.PersonId = P.Id
                     JOIN [Role] R ON CP.RoleId = R.Id
                     WHERE P.Id = @PersonId
                        ",
                        new SqlParameter("@PersonId", person.Id))
                        ;
                CustomPrincipal customPrincipal = new CustomPrincipal(customIdentity, person);
                HttpContext.Current.User = customPrincipal;
                Thread.CurrentPrincipal = customPrincipal;

            }
        }
    }
}
