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
    // This controller implements Crud operation Based on Json Value
    public class ContactsController : Controller
    {
        // Fields that are going to use throughout the ContactController
        private readonly AppDbContext appDbContext;
        private readonly INotyfService _notyfService;

        // Constructor
        public ContactsController(AppDbContext appDbContext, INotyfService notyfService)
        {
            this.appDbContext = appDbContext;
            _notyfService = notyfService;
        }
        
        // Index action
        // This action includes a view that our json value is going to demonstrate in this view 
        public IActionResult Index()
        {
            return View();
        }

        // This action load all data that exist in Contact table
        // Ther return type of this action is a Json value
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

            // If the Contact model is not valid execute the if clause
            if (!ModelState.IsValid)
            {
                // Show error using ToastNotification 
                _notyfService.Error("There are some errors in your inputs. Please check all fields");
                var errorList = (from item in ModelState
                                 where item.Value.Errors.Any()
                                 select item.Value.Errors[0].ErrorMessage
                    ).ToList();

                // Set error message to the Message variable
                Message = "There are some errors in your inputs. Please check all fields";

                // Return data and messages through the Json 
                return Json(new { Success = false, data = Message ,ErrorList=errorList});
            }

            // If contact id is 0 then we should add to the conact
            if(contact.Id==0)
            {
                // Set successful message to the Message variable
                Message = "New contact has been added.";

                // Add contact object to the table
                appDbContext.Contacts.Add(contact);

                // Demonstrate message using ToastNotification
                _notyfService.Success(Message);
            }

            // If the id of contact is not 0 then we should update the contact value
            else
            {
                // Set successful message to the Message variable
                Message = "New contact has been updated.";

                // Attach contact values to the contactObject variable
                var contactObject=appDbContext.Contacts.Attach(contact);

                // Update the state of contactObject variable
                contactObject.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                // Demonstrate message using ToastNotification
                _notyfService.Warning(Message);
            }
            
            // Save all changes to the database
            appDbContext.SaveChanges();

            // Return Json value and pass the message variable it
            return Json(new { Success=true,data=Message});
        }

        // This action set all values to the controls to make prepration to edit Contact
        // This action is going to execute the Edit contact
        // If there is a contact in the database then pass all values in contact object to the json value
        // Then json value can show all values in the related form control
        public JsonResult EditContact(int contactId)
        {
            var model = appDbContext.Contacts.FirstOrDefault(x=>x.Id==contactId);
            return Json(new { Success=true,data=model});
        }

        // This action delete a contact based on the contactId
        public JsonResult DeleteContact(int contactId)
        {
            // Retrieve the contact from the Context
            var model = appDbContext.Contacts.FirstOrDefault(x => x.Id == contactId);

            // Remove the contact the we have retrieved from context 
            appDbContext.Contacts.Remove(model);

            // Save all changes to the database
            appDbContext.SaveChanges();

            // Demonstrate message using ToastNotification
            _notyfService.Information("Data has been deleted successfully.");

            // Return Json value and pass the message variable it
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
