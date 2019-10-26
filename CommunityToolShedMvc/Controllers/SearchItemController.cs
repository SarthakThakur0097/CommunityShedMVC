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
    public class SearchItemController : Controller
    {
        // GET: SearchItem
        public ActionResult Index(int communityId)
        {
            var cp = ((CustomPrincipal)User);
            var viewModel = new SearchViewModel();
            viewModel.PopulateSelectLists();
            
            viewModel.Tools = DatabaseHelper.Retrieve<Tool>(@"
                      SELECT i.id, i.[Name], c.Type, i.Warnings, ag.Age
                      FROM Item i 
                      JOIN Categories c ON i.CategoriesId = c.id                 
                      JOIN Person p ON p.Id = i.PersonId
                      JOIN AgeRange ag ON i.AgeId = ag.Id
                      WHERE p.Id = @PersonId AND i.CommunityId = @CommunityId
                         ",
                        new SqlParameter("@PersonId", cp.Person.Id),
                        new SqlParameter("@CommunityId", communityId))
                        ;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(int communityId, SearchViewModel viewModel)
        {
            var cp = ((CustomPrincipal)User);
            string toolName = viewModel.ItemName;
            if(toolName==null)
            {
                toolName = "";
            }
            int CategoryId = viewModel.CategoryId;
            int rangeId = viewModel.AgeId;

            viewModel.PopulateSelectLists();

            if (toolName.Length == 0 && CategoryId == 0 && rangeId == 0)
            {
                viewModel.Tools = DatabaseHelper.Retrieve<Tool>(@"
                      SELECT i.id, i.[Name], c.Type, i.Warnings, ag.Age
                      FROM Item i JOIN Community cm ON i.CommunityId = cm.Id 
                      JOIN Categories c ON i.Id = c.id
                      
                      JOIN Person p ON p.Id = i.PersonId
                      JOIN AgeRange ag ON i.AgeId = ag.Id
                      WHERE p.Id = @PersonId AND cp.CommunityId = @CommunityId
                         ",
                        new SqlParameter("@PersonId", cp.Person.Id),
                        new SqlParameter("@CommunityId", communityId))
                        ;
            }
            else if (toolName.Length > 1 && (CategoryId == 0) && (rangeId == 0))
            {
                viewModel.Tools = DatabaseHelper.Retrieve<Tool>(@"
                    SELECT i.id, i.[Name],c.[Type], c.Id ,i.CategoriesId, p.FirstName + ' ' + p.LastName AS FullName, ag.Age
                    FROM Item i JOIN Community cm ON i.CommunityId = cm.Id
                    JOIN Categories c ON i.CategoriesId = c.Id
                    JOIN Person p ON i.PersonId = p.id
                    JOIN AgeRange ag ON i.AgeId = ag.Id
                    WHERE i.[Name] = @Name",
                       new System.Data.SqlClient.SqlParameter("@name", toolName));
            }

            else if(CategoryId!= 0 && toolName.Length == 0)
            {
                viewModel.Tools = DatabaseHelper.Retrieve<Tool>(@"
                    SELECT  i.id AS itemId,i.[Name], i.CategoriesId, c.[Type], c.Id,  p.FirstName + ' ' + p.LastNAME AS FullName, ag.Age
                    FROM Item i JOIN Categories c ON i.CategoriesId = c.Id
                    JOIN Community cm ON i.CommunityId = cm.Id
                    JOIN Person p ON i.PersonId = p.id
                    JOIN AgeRange ag ON i.AgeId = ag.Id
                    WHERE c.Id = @CategoryId",
                    new System.Data.SqlClient.SqlParameter("@CategoryId", CategoryId));
            }
            else if(rangeId!= 0 && toolName.Length == 0)
            {
                viewModel.Tools = DatabaseHelper.Retrieve<Tool>(@"
                    SELECT  i.id AS itemId, i.[Name], i.CategoriesId, c.[Type], c.Id,  p.FirstName + ' ' + p.LastNAME AS FullName, ag.Age
                    FROM Item i JOIN Categories c ON i.CategoriesId = c.Id
                    JOIN AgeRange ag ON ag.Id = i.AgeId
                    JOIN Community cm ON i.CommunityId = cm.Id
                    JOIN Person p ON i.PersonId = p.id
                    WHERE ag.Id = @selectedAgeRange",
                    new System.Data.SqlClient.SqlParameter("@selectedAgeRange", rangeId));
            }
            else if(toolName.Length > 1 && CategoryId != 0)
            {
                viewModel.Tools = DatabaseHelper.Retrieve<Tool>(@"
                     SELECT i.id AS itemId, i.[Name], i.CategoriesId, c.[Type], c.Id,  p.FirstName + ' ' + p.LastNAME AS FullName, ag.Age
                    FROM Item i JOIN Categories c ON i.CategoriesId = c.Id
                    JOIN AgeRange ag ON ag.Id = i.AgeId
                    JOIN Community cm ON i.CommunityId = cm.Id
                    JOIN Person p ON i.PersonId = p.id
                    WHERE i.[Name] = @Name AND c.Id = @toolType",
                    new System.Data.SqlClient.SqlParameter("@name", toolName),
                    new System.Data.SqlClient.SqlParameter("@toolType", CategoryId));
            }
            else if(toolName.Length > 1 && rangeId!= 0)
            {

                viewModel.Tools = DatabaseHelper.Retrieve<Tool>(@"SELECT i.id AS itemId, i.[Name], i.CategoriesId, c.[Type], c.Id,  p.FirstName + ' ' + p.LastNAME AS FullName, ag.Age
                   FROM Item i JOIN Categories c ON i.CategoriesId = c.Id
                    JOIN AgeRange ag ON ag.Id = i.AgeId
                    Item i JOIN Community cm ON i.CommunityId = cm.Id
                    JOIN Person p ON i.PersonId = p.id
                    WHERE i.[Name] = @Name AND WHERE ag.Id = @selectedAgeRange",
                    new System.Data.SqlClient.SqlParameter("@name", toolName),
                    new System.Data.SqlClient.SqlParameter("@selectedAgeRange", rangeId)
                    );

            }
            else if(toolName.Length > 1 && CategoryId != 0 && rangeId != 0)
            {
                viewModel.Tools = DatabaseHelper.Retrieve<Tool>(@"
                    SELECT  i.id AS itemId, i.[Name], i.CategoriesId, c.[Type], c.Id,  p.FirstName + ' ' + p.LastNAME AS FullName, ag.Age
                    FROM Item i JOIN Community cm ON i.CommunityId = cm.Id
                    JOIN Categories c ON i.CategoriesId = c.Id
                    JOIN AgeRange ag ON ag.Id = i.AgeId
                    
                    WHERE i.[Name] = @Name AND WHERE c.Id = @toolType AND WHERE ag.Id = @selectedAgeRange",
                    new System.Data.SqlClient.SqlParameter("@name", toolName),
                    new System.Data.SqlClient.SqlParameter("@toolType", CategoryId),
                    new System.Data.SqlClient.SqlParameter("@selectedAgeRange", rangeId)
                    );
            }
            return View(viewModel);
        }
    }
}