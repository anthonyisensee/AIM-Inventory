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
        //  Displays a page from a list of devices from a database (by default displays the first page).
        //  Note that pages begin index at 1, not 0, since parameters are seen in the address bar user side.
        // Parameters:
        //  page: The page of data to show the user. Note that page is set to 1 by default, and that page indexing begins at 1.
        // Returns:
        //  A view with a list of devices.
        public IActionResult Index(string search = null, int? page = 1)   // Note that page is set to 1 by default, but can be modified.
        {
            // Send the search string to the view so that it may use it to load further pages of the search.
            ViewData["SearchString"] = search;

            // Assign the view the information it needs to be able to ask for the next and previous page.
            ViewData["LastPage"] = page - 1;
            ViewData["NextPage"] = page + 1;

            // Set data to inform the view if "Last Page" button should be shown.
            if (page == 1) { ViewData["LastPageButtonShown"] = false; }     // If first page, don't show "Last Page" button.
            else { ViewData["LastPageButtonShown"] = true; }                // If not first page, do show "Last Page" button.

            // Define the maximum number of items displayed per page. Used by the query.
            int items_per_page = 5;

            // Calculate the offset (number of items to skip in the query) based on the page and limit. Used by the query.
            int offset = ( (int)page - 1) * items_per_page;  // The page variable must be cast from "int?" to "int"

            // Retrieve the database connection string from the appsettings.json file.
            string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

            // Initialize a list to store populated device models inside of.
            List<DeviceModel> devices = new List<DeviceModel>();

            // Create an instance of our DataAccess class so that we may use its functionality to populate a list with data.
            DataAccess _data = new DataAccess();

            // Check for a search string.
            // If there is no search string
            if (search == null) 
            {
                // Create the SQL statement that is to be executed to list paginated view of all devices.
                string sqlStatement = "SELECT * FROM `device` LIMIT " + items_per_page.ToString() + " OFFSET " + offset.ToString() + ";";

                // Populate the list of devices using our DataAccess logic.
                devices = _data.LoadData<DeviceModel, dynamic>(sqlStatement, new { }, connectionString);
            }
            // If there is a search string
            else
            {
                // Append "%" to beginning and end of search string to allow searching for entries that contain the search string
                string preparedSearchString = "%" + search + "%";

                // Create the SQL statement that is to be executed to list paginated view of all devices.
                string sqlStatement = @"SELECT * FROM `device` 
                                        WHERE `type` LIKE @search
                                        OR `friendly_name` LIKE @search
                                        OR `ip_address` LIKE @search
                                        OR `serial_number` LIKE @search
                                        OR `mac_address` LIKE @search
                                        OR `notes` LIKE @search
                                        LIMIT " + items_per_page.ToString() + " OFFSET " + offset.ToString() + ";";

                // Populate the list of devices using our DataAccess logic, feeding in sql statement and the prepared search
                // string as a parameter. We avoid hard coding the string into the query to prevent malicious sql injection.
                devices = _data.LoadData<DeviceModel, dynamic>(sqlStatement, new { search = preparedSearchString }, connectionString);
            }

            // Set data to inform the view if "Next Page" button should be shown.
            if ( devices.Count == items_per_page )              // If this page has a full list of devices...
            { ViewData["NextPageButtonShown"] = true; }         // ...assume there is another page and display "NextPage" button.
            else { ViewData["NextPageButtonShown"] = false; }   // Otherwise, don't display the next page button.
            
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
                string sqlStatement = @"INSERT INTO device (id, type, friendly_name, ip_address, serial_number, model_number, mac_address, operating_system, notes, date_purchase, date_retire) 
                                        VALUES (@id, @type, @friendly_name, @ip_address, @serial_number, @model_number, @mac_address, @operating_system, @notes, @date_purchase, @date_retire);";

                // The connection string to be used to connect to the database. From the appsettings.json file.
                string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

                // Initialize an instance of our DataAccess class so we may store data with it.
                DataAccess _data = new DataAccess();

                // Use DataHandler's DataAccess class to save data to the specified database.
                _data.ExecuteStatement(
                    sqlStatement,                                   // sql string
                    new {
                        id = model.ID,                            // model parameters
                        type = model.Type,
                        friendly_name = model.Friendly_Name,
                        ip_address = model.IP_Address,
                        serial_number = model.Serial_Number,
                        model_number = model.Model_Number,
                        mac_address = model.MAC_Address,
                        operating_system = model.Operating_System,
                        notes = model.Notes,
                        date_purchase = model.Date_Purchase,
                        date_retire = model.Date_Retire
                    },
                    connectionString);                              // connection string

                // Returns the "Index" view. The new device should be populated to the list.
                // Note that if we did not specify the "Index" view, the "Create" view would be returned instead.
                return RedirectToAction("Index");
            }

            // TODO: This should redirect to error message if model state is not valid.
            return View("Index");
        }

        // Summary:
        //  Retrieves a single row from the database and returns a view with the row's details.
        // Parameters:
        //  id: The id of the device to locate.
        // Returns:
        //  A view with the information of the specific device.
        public IActionResult Edit(int? id)
        {
            // Initialize the SQL statement that is to be executed.
            string sqlStatement = "select * from device where id = @id;";

            // Retrieve the database connection string from the appsettings.json file.
            string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

            // Initialize a list to store populated device models inside of.
            List<DeviceModel> devices = new List<DeviceModel>();

            // Create an instance of our DataAccess class so that we may use its functionality to populate a list with data.
            DataAccess _data = new DataAccess();

            // Populate the list of devices using our DataAccess logic.
            devices = _data.LoadData<DeviceModel, dynamic>(sqlStatement, new { id }, connectionString);

            // Return the "Edit" view with the first device in the device list.
            return View("Edit", devices[0]);
        }

        // Summary:
        //  Handles the confirm on an edit to an entry in the database.
        // Parameters:
        //  model: the model of the specific item to edit.
        // Returns:
        //  The edited list of entries.
        [HttpPost]
        public IActionResult Edit(DeviceModel model)
        {
            // Here we check to make sure that the model follows all the attribute rules of "DeviceModel".
            // The "Create" view should have kept the user within the parameters, but an extra check is not a bad thing.
            if (ModelState.IsValid)
            {
                // Define the necessary sql string with parameters. (@ symbol is only there so string can wrap multiple lines.)
                string sqlStatement = @"update device 
                                        set type = @type, friendly_name = @friendly_name, ip_address = @ip_address, 
                                        serial_number = @serial_number, model_number = @model_number, mac_address = @mac_address, 
                                        operating_system = @operating_system, notes = @notes, date_purchase = @date_purchase,
                                        date_retire = @date_retire
                                        where id = @id;";

                // The connection string to be used to connect to the database. From the appsettings.json file.
                string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

                // Initialize an instance of our DataAccess class so we may store data with it.
                DataAccess _data = new DataAccess();

                // Use DataHandler's DataAccess class to save data to the specified database.
                _data.ExecuteStatement(
                    sqlStatement,                                   // sql string
                    new
                    {
                        id = model.ID,                            // model parameters
                        type = model.Type,
                        friendly_name = model.Friendly_Name,
                        ip_address = model.IP_Address,
                        serial_number = model.Serial_Number,
                        model_number = model.Model_Number,
                        mac_address = model.MAC_Address,
                        operating_system = model.Operating_System,
                        notes = model.Notes,
                        date_purchase = model.Date_Purchase,
                        date_retire = model.Date_Retire
                    },
                    connectionString);                              // connection string

                // Returns the "Index" view. The new device should be populated to the list.
                // Note that if we did not specify the "Index" view, the "Create" view would be returned instead.
                return RedirectToAction("Index");
            }

            // TODO: This should redirect to error message if model state is not valid.
            return View("Index");
        }

        // Summary:
        //  Shows item details with a link to delete device.
        // Parameters:
        //  id: an integer representing the ID of the object that may be deleted.
        // Returns:
        //  The Delete view.
        public IActionResult Delete(int? id)
        {
            // Initialize the SQL statement that is to be executed.
            string sqlStatement = "select * from device where id = @id;";

            // Retrieve the database connection string from the appsettings.json file.
            string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

            // Initialize a list to store populated device models inside of.
            List<DeviceModel> devices = new List<DeviceModel>();

            // Create an instance of our DataAccess class so that we may use its functionality to populate a list with data.
            DataAccess _data = new DataAccess();

            // Populate the list of devices using our DataAccess logic.
            devices = _data.LoadData<DeviceModel, dynamic>(sqlStatement, new { id }, connectionString);

            // Return the default "Delete" view with the first device in the device list.
            return View("Delete", devices[0]);
        }

        // Summary:
        //  Redirects the user from the delete page to the delete confirmation page.
        // Parameters:
        //  model: The model defining the device that is to be deleted.
        // Returns:
        //  A redirection to the delete confirmation page.
        [HttpPost]
        public IActionResult Delete(DeviceModel model)
        {
            // Redirects to confirm delete page providing the model's ID.
            return RedirectToAction("ConfirmDelete", model.ID);
        }

        // Summary:
        //  Returns a view of the device to be confirmed for deletion. View contains a final confirm delete button.
        // Parameters:
        //  id: an integer representing the ID of the object to be deleted.
        // Returns:
        //  The ConfirmDelete view which contains a button to confirm and finalize deletion.
        public IActionResult ConfirmDelete(int? id)    // Note that the id integer must be optional (thus the "?").
        {            
            // Initialize the SQL statement that is to be executed.
            string sqlStatement = "select * from device where id = @id;";

            // Retrieve the database connection string from the appsettings.json file.
            string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

            // Initialize a list to store populated device models inside of.
            List<DeviceModel> devices = new List<DeviceModel>();

            // Create an instance of our DataAccess class so that we may use its functionality to populate a list with data.
            DataAccess _data = new DataAccess();

            // Populate the list of devices using our DataAccess logic.
            devices = _data.LoadData<DeviceModel, dynamic>(sqlStatement, new { id = id }, connectionString);

            // Return the ConfirmDelete view with the first device in the device list.
            return View("ConfirmDelete", devices[0]);
        }

        // Summary:
        //  Deletes a device from the database upon user confirmation.
        // Parameters:
        //  model: The detailed model of the device that is to be deleted.
        // Returns:
        //  A redirection to the Devices Index.
        [HttpPost]
        public IActionResult ConfirmDelete(DeviceModel model)
        {            
            // Define the necessary sql string for deletion with parameters.
            string sql = "delete from device where id = @id;";

            // The connection string to be used to connect to the database. From the appsettings.json file.
            string connectionString = ConfigurationExtensions.GetConnectionString(_config, "default");

            // Initialize an instance of our DataAccess class so we may modify data with it.
            DataAccess _data = new DataAccess();

            // Use DataHandler's DataAccess class to delete data from the specified database.
            _data.ExecuteStatement(
                sql,                        // sql string
                new { id = model.ID },      // ID of the object to delete
                connectionString);          // connection string

            //Returns the "Index" view. New list reflecting deletion will load.
            return RedirectToAction("Index");
        }
    }
}