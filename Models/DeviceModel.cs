// Anthony Isensee
// 7/30/21
// Much of model layout and .cs features identified from: https://www.youtube.com/watch?v=bIiEv__QNxw

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AIM_Inventory.Models
{

    // This class allows us to create a complete picture of a 'device' in code in th e control structure of our web project.
    // Follows all fields present in inventory_local_test database & device table version 0.1
    public class DeviceModel
    {
        // A reference to a Device's unique ID. This particular field should not be displayed, but the ID can be used to identify an object easily in code.
        [Display(Name = "Device ID")]
        public int ID { get; set; }

        // The type of device (for instance: server, laptop, etc.)
        [Display(Name = "Device Type")]
        public string Type { get; set; }

        // Friendly Name
        [Display(Name = "Device Name")]
        public string Friendly_Name { get; set; }

        // IP Address
        [Display(Name = "IP Address")]
        public string IP_Address { get; set; }

        // Serial Number
        [Display(Name = "Serial Number")]
        public string Serial_Number { get; set; }

        // Model Number
        [Display(Name = "Model Number")]
        public string Model_Number { get; set; }

        // MAC Address
        [Display(Name = "MAC Address")]
        public string MAC_Address { get; set; }

        // Operating System
        [Display(Name = "Operating System")]
        public string Operating_System { get; set; }

        // Notes
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        // Date Purchased
        [Display(Name = "Date Purchased")]
        [DataType(DataType.Date)]
        public DateTime Date_Purchase { get; set; }

        // Date Retired
        [Display(Name = "Date Retired")]
        [DataType(DataType.Date)]
        public DateTime Date_Retire { get; set; }
    }
}
