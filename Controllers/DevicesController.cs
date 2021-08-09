// Anthony Isensee
// 8/6/21

using AIM_Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// usings for ExampleList
using DataHandler;
using Microsoft.Extensions.Configuration;

namespace AIM_Inventory.Controllers
{
    public class DevicesController : Controller
    {

        // Summary:
        //  Displays a list of devices from a database (db version 0.1)
        // Returns:
        //  A view with a list of devices.
        public IActionResult Index()
        {
            // Create list.
            List<DeviceModel> devices = new List<DeviceModel>();

            // Creates an instance of our DataHandler.DataAccess class so that we may use it's functionality to populate a list with data.
            DataAccess _data = new DataAccess();

            // TODO: Figure out a way to incorporate the connection string from appsettings.json
            // Using the Microsoft.Extensions.Configuration class we create an instance of a config that will allow us to reference our appsettings.json to obtain our database connection string.
            //IConfiguration _config;
            
            // MySQL statement to execute. 
            string sqlStatement = "SELECT * FROM DEVICE;";
            devices = _data.LoadData<DeviceModel, dynamic>(sqlStatement, new { }, "Server=127.0.0.1;Port=3306;database=inventory_local_test;user id=local_user;password=password;");

            // Returns the default view (Index) with a list of devices.
            // Could also be specified as:
            return View("Index", devices);
            //cmdcreturn View(devices);

        }

        // Summary:
        //  Displays a field for creating a new device.
        public IActionResult Create()
        {
            return View();
        }

        // Summary:
        //  Stores an entry into the database when the asp-action "create" takes place (trigger from Create view is the "Create" button.
        // Parameters:
        //  model: a DeviceModel model that contains device information.
        // Returns:
        //  The "Index" view if the model is valid, which should include the new device. If the model is not valid, returns the current view.
        [HttpPost]  // This [HttpPost] attribute specifies that this is an action that will be performed *after* the Create page is loaded.
                    // This tag also allows us to have two "Create" methods in this class without errors. (Try removing the tag, I dare you!)
        public IActionResult Create(DeviceModel model)
        {
            // Here we check to make sure that the model follows all the attribute rules of "DeviceModel".
            // The "Create" view should have kept the user within the parameters, but an extra check is not a bad thing.
            if (ModelState.IsValid)
            {
                // Define the necessary sql string with parameters. (@ symbol is only there so string can wrap multiple lines.)
                string sql = @"insert into device (id, type, friendly_name, ip_address, serial_number, model_number, mac_address, operating_system, notes, date_purchase, date_retire) 
                                values (@id, @type, @friendly_name, @ip_address, @serial_number, @model_number, @mac_address, @operating_system, @notes, @date_purchase, @date_retire);";
                DataAccess _data = new DataAccess();
                
                // Use DataHandler's DataAccess class to save data to the specified database.
                _data.SaveData(
                    sql,                    // sql string
                    new { id = model.ID,    // model parameters
                        type = model.Type, 
                        friendly_name = model.Friendly_Name, 
                        ip_address = model.IP_Address,
                        serial_number = model.Serial_Number,
                        model_number = model.Model_Number,
                        mac_address = model.MAC_Address,
                        operating_system = model.Operating_System,
                        notes = model.Notes,
                        date_purchase = model.Date_Purchase,
                        date_retire = model.Date_Retire },   
                    "Server=127.0.0.1;Port=3306;database=inventory_local_test;user id=local_user;password=password;");  // connection string

                // Returns the "Index" view. The new device should be populated to the list.
                // Note that if we did not specify the "Index" view, the "Create" view would be returned instead.
                return RedirectToAction("Index");

            }

            return View();
        }

        public IActionResult Edit()
        {
            return View("Index");
        }

        public IActionResult Delete()
        {
            return View("Index");
        }

    }
}
