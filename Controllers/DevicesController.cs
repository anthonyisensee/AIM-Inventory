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

        // Displays a list of devices from a database (db version 0.1)
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

            return View(devices);

        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
