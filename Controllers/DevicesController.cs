// Anthony Isensee
// 8/6/21

using AIM_Inventory.Models;
using DataHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace AIM_Inventory.Controllers
{
    public class DevicesController : Controller
    {
        // Summary:
        //  This constructor allows us to pull information from our appsettings.json file, useful for application settings and database connection strings.
        public DevicesController(IConfiguration config)
        {
            _config = config;
        }
        
        // Variable for constructor to set whenever controller is created.
        private readonly IConfiguration _config;

        // Summary:
        //  Displays a list of devices from a database
        // Returns:
        //  A view with a list of devices.
        public IActionResult Index()
        {            
            // Initialize the SQL statement that is to be executed.
            string sqlStatement = "select * from device;";

            // Retrieve the database connection string from the appsettings.json file.
            string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

            // Initialize a list to store populated device models inside of.
            List<DeviceModel> devices = new List<DeviceModel>();

            // Create an instance of our DataAccess class so that we may use its functionality to populate a list with data.
            DataAccess _data = new DataAccess();

            // Populate the list of devices using our DataAccess logic.
            devices = _data.LoadData<DeviceModel, dynamic>(sqlStatement, new { }, connectionString);

            // Finally, return the default view (Index) to the user with a populated list of devices.
            return View("Index", devices);
        }

        // Summary:
        //  Displays a field for creating a new device.
        public IActionResult Create()
        {
            // Note that the "Create" view is implied since this is the "Create" method.
            // Thus, the "Create" view is returned automatically without it having been specified.
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
                string sqlStatement = @"insert into device (id, type, friendly_name, ip_address, serial_number, model_number, mac_address, operating_system, notes, date_purchase, date_retire) 
                                        values (@id, @type, @friendly_name, @ip_address, @serial_number, @model_number, @mac_address, @operating_system, @notes, @date_purchase, @date_retire);";

                // The connection string to be used to connect to the database. From the appsettings.json file.
                string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

                // Initialize an instance of our DataAccess class so we may store data with it.
                DataAccess _data = new DataAccess();
                
                // Use DataHandler's DataAccess class to save data to the specified database.
                _data.SaveData(
                    sqlStatement,                                   // sql string
                    new { id = model.ID,                            // model parameters
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
                    connectionString);                              // connection string

                // Returns the "Index" view. The new device should be populated to the list.
                // Note that if we did not specify the "Index" view, the "Create" view would be returned instead.
                return RedirectToAction("Index");
            }

            // TODO: This should redirect to error message if model state is not valid.
            return View("Index");
        }

        [HttpPost]
        public IActionResult Edit()
        {
            return View("Index");
        }

        // Summary:
        //  Deletes an entry from the device table based on a given ID.
        // Parameters:
        //  id: an integer representing the ID of the object to be deleted.
        // Returns:
        //  The list view.
        public IActionResult Delete(int? id)    // Note that the id integer must be optional (thus the "?").
        {
            // Define the necessary sql string with parameters.
            string sql = "delete from device where id = @id;";

            // The connection string to be used to connect to the database. From the appsettings.json file.
            string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

            // Initialize an instance of our DataAccess class so we may modify data with it.
            DataAccess _data = new DataAccess();

            // Use DataHandler's DataAccess class to save data to the specified database.
            _data.SaveData(
                sql,            // sql string
                new { id },     // ID of the object to delete
                connectionString);  // connection string

            // Returns the "Index" view. The list should reload and be updated.
            // We must specify that we are redirecting to the "Index" since there is no "Delete" view.
            // Also note that the RedirectToAction is necessary instead of just View("Index");
            return RedirectToAction("Index");
        }
    }
}