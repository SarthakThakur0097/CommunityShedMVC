using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using CommunityToolShedMvc.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public CustomPrincipal CustomUser
        {
            get
            {
                return (CustomPrincipal)User;
            }
        }

        public ActionResult Add()
        {

        }
        public ActionResult Index()
        {
            bool isMember = CustomUser.IsInRole(1, "Member");
            bool isApprover = CustomUser.IsInRole(1, "Approver");
            List<Communites> Communities = DatabaseHelper.Retrieve<Communites>(@"
                      SELECT c.Id, c.[Name], c.Availability
                      FROM Community c
                         ")
                        ;
            //List<Tool> tools =  DatabaseHelper.Retrieve<Tool>(@"
            //          SELECT R.RoleName, CP.CommunityId
            //          FROM CommunityPerson CP
            //          JOIN Community C ON CP.CommunityId = C.Id
            //          JOIN Person P ON CP.PersonId = P.Id
            //          JOIN [Role] R ON CP.RoleId = R.Id
            //          WHERE P.Id = @PersonId
            //             ",
            //            new SqlParameter("@PersonId", ))
            //            ;
            return View(Communities);
        }

        
    }
}