using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using CommunityToolShedMvc.Security;
using CommunityToolShedMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMvc.Controllers
{
    [Authorize]
    public class ToolsController : Controller
    {


        // GET: Tools


        public ActionResult Detail(int communityid, int id)
        {
            var viewModel = new DetailViewModel();


            viewModel.Tool = DatabaseHelper.RetrieveSingle<Tool>(@"
                      SELECT i.id, i.[Name], c.Type, i.Warnings, ag.Age
                      FROM Item i JOIN Community cm ON i.CommunityId = cm.Id 
                      JOIN Categories c ON i.Id = c.id

                      JOIN Person p ON p.Id = i.PersonId
                      JOIN AgeRange ag ON i.AgeId = ag.Id
                      WHERE i.id = @itemId 
                         ",
                        new SqlParameter("@itemId", id));

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Detail(int communityId, DetailViewModel viewModel, int id)
        {
            //var viewModel = new DetailViewModel();
            var cp = ((CustomPrincipal)User);
            DateTime requestedOndate = DateTime.UtcNow;
            int? idDB = DatabaseHelper.Insert(@"
                insert into ItemLoan (ItemId, FromPersonId, ToPersonId, BorrowedOn)
                values (@ItemId, @FromPersonId, @ToPersonId, @BorrowedOn);",
               new SqlParameter("@ItemId", id),
               new SqlParameter("@FromPersonId", cp.Person.Id),
               new SqlParameter("@ToPersonId", cp.Person.Id),
               new SqlParameter("@BorrowedOn", requestedOndate)
               );
            ;
            return View(viewModel);
        }

        public ActionResult Add(int communityId)
        {
            var viewModel = new AddItemViewModel();
            viewModel.PopulateSelectLists();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add(int communityId, AddItemViewModel viewModel)
        {

            viewModel.PopulateSelectLists();
            var cp = ((CustomPrincipal)User);
            //string toolName = viewModel.ItemName;
            //string warning = viewModel.Tool.Warnings;

            //int CategoryId = viewModel.CategoryId;
            //int rangeId = viewModel.AgeId;



            int? id = DatabaseHelper.Insert(@"
                insert into Item (Name, CategoriesId, AgeId, Warnings, CommunityId, PersonId)
                values (@Name, @CategoriesId, @AgeId, @Warnings, @CommunityId, @PersonId);",
               new SqlParameter("@Name", viewModel.Tool.Name),
               new SqlParameter("@CategoriesId", viewModel.Tool.CategoriesId),
               new SqlParameter("@AgeId", viewModel.Tool.AgeId),
               new SqlParameter("@Warnings", viewModel.Tool.Warnings),
               new SqlParameter("@CommunityId", communityId),
               new SqlParameter("@PersonId", cp.Person.Id)
               
              );


            ;
            return RedirectToAction("Index", "SearchItem", communityId);
        }
    }
}