using AspNetCoreHero.ToastNotification.Abstractions;
using CRUDOperationUsingJqueryAjax.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace CRUDOperationUsingJqueryAjax.Controllers
{
    // This is a ContactController 
    // This controller implement Crud operation Based on Json Value
    public class ContactsController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly INotyfService _notyfService;
        public ContactsController(AppDbContext appDbContext, INotyfService notyfService)
        {
            this.appDbContext = appDbContext;
            _notyfService = notyfService;
        }
        
        // Index action
        public IActionResult Index()
        {
            return View();
        }

        // This action load all data that exist in Contact table
        public IActionResult Load()
        {

            List<Contact> model = appDbContext.Contacts.ToList();
            
            return Json(new { Success = true, data = model });
        }
        

        // This action add or update all data in Contact table
        // We get all values using FromBody
        [HttpPost]
        public JsonResult AddUpdate([FromBody] Contact contact)
        {
            string Message = "";
            if (!ModelState.IsValid)
            {
                _notyfService.Error("There are some errors in your inputs. Please check all fields");
                var errorList = (from item in ModelState
                                 where item.Value.Errors.Any()
                                 select item.Value.Errors[0].ErrorMessage
                    ).ToList();
                Message = "There are some errors in your inputs. Please check all fields";
                return Json(new { Success = false, data = Message ,ErrorList=errorList});
            }
            if(contact.Id==0)
            {
                Message = "New contact has been added.";
                appDbContext.Contacts.Add(contact);
                _notyfService.Success(Message);
            }
            else
            {
                Message = "New contact has been updated.";
                var contactObject=appDbContext.Contacts.Attach(contact);
                contactObject.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _notyfService.Warning(Message);
            }
            
            appDbContext.SaveChanges();
            return Json(new { Success=true,data=Message});
        }

        // This action set all values to the controls to make prepration to edit Contact
        public JsonResult EditContact(int contactId)
        {
            var model = appDbContext.Contacts.FirstOrDefault(x=>x.Id==contactId);
            return Json(new { Success=true,data=model});
        }

        // This action delete a contact based on the contactId
        public JsonResult DeleteContact(int contactId)
        {
            var model = appDbContext.Contacts.FirstOrDefault(x => x.Id == contactId);
            appDbContext.Contacts.Remove(model);
            appDbContext.SaveChanges();
            _notyfService.Information("Data has been deleted successfully.");

            return Json(new { Success = "true", data = model});
        }

        // This action load a partial view to set Rate system
        public IActionResult _RateToApp()
        {
            return PartialView("_RateToApp");
        }

        // This action save the FeedBack to the database
        public JsonResult FeedBack([FromBody] FeedBack feedBack)
        {
            var Message = "Your feedback has been sent successfully.";
            appDbContext.FeedBacks.Add(feedBack);
            appDbContext.SaveChanges();
            return Json(new { Success=true,data=Message});
        }

    }
}
