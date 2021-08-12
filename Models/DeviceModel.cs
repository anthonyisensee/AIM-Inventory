// Anthony Isensee
// 7/30/21
// Some of model layout and .cs features identified from: https://www.youtube.com/watch?v=bIiEv__QNxw

using System;
using System.ComponentModel.DataAnnotations;

namespace AIM_Inventory.Models
{
    // This class allows us to create a complete picture of a 'device' in code in th e control structure of our web project.
    // Follows all fields present in inventory_local_test database & device table version 0.1.
    // Names must be the same as the tables in the database, though lower/uppercase letters may be different.
    public class DeviceModel
    {
        // A reference to a Device's unique ID. This particular field should not be displayed, but the ID can be used to identify an object easily in code.
        [Display(Name = "Device ID")]
        public int ID { get; set; }

        // Friendly Name
        [Required]
        [Display(Name = "Device Name", Prompt = "Friendly Name")]
        [StringLength(128, ErrorMessage = "Limit of 128 characters. Supercalifragilisticexpialidocious even fits, but your name doesn't?")]
        public string Friendly_Name { get; set; }

        // IP Address
        [Display(Name = "IP Address", Prompt = "XXX.XXX.XXX.XXX, for dynamic leave empty.")]
        [StringLength(128, ErrorMessage = "Limit of 128 characters. What are you using, IPv128?")]
        public string IP_Address { get; set; }

        // The type of device (for instance: server, laptop, etc.)
        [Display(Name = "Device Type", Prompt = "Server, laptop, etc.")]
        [StringLength(128, ErrorMessage = "Limit of 128 characters.")]
        public string Type { get; set; }

        // Serial Number
        [Display(Name = "Serial Number")]
        [StringLength(128, ErrorMessage = "Limit of 128 characters. No exceptions.")]
        public string Serial_Number { get; set; }

        // Model Number
        [Display(Name = "Model Number")]
        [StringLength(128, ErrorMessage = "Limit of 128 characters. For your sake, I hope this model number really isn't that long!")]
        public string Model_Number { get; set; }

        // MAC Address
        [Display(Name = "MAC Address")]
        [StringLength(128, ErrorMessage = "Limit of 128 characters, though you really shouldn't need that many.")]
        public string MAC_Address { get; set; }

        // Operating System
        [Display(Name = "Operating System", Prompt = "Windows 10 Pro, Ubunutu 20.04, etc.")]
        [StringLength(128, ErrorMessage = "Limit of 128 characters (seriously, how do you need 128 characters for this?).")]
        public string Operating_System { get; set; }

        // Notes
        [StringLength(10000, ErrorMessage = "Limit of 10,000 characters. Speak to project developers if increase is needed.")]
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        // Date Purchased
        // Note that the "?" after the DateTime type makes this attribute nullable. If "?" is not implemented here, the field becomes required.
        [Display(Name = "Date Purchased")]
        [DataType(DataType.Date)]
        public DateTime? Date_Purchase { get; set; }

        // Date Retired
        [Display(Name = "Date Retired")]
        [DataType(DataType.Date)]
        public DateTime? Date_Retire { get; set; }
    }
}