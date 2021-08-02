-- Anthony Isensee
-- 7/30/21
-- Version: 0.1
-- Location: Local Database

-- Create schema.
CREATE SCHEMA `inventory_local_test`; 

-- Create device table.
CREATE TABLE `inventory_local_test`.`device` (
  `id` INT NOT NULL,
  `type` VARCHAR(128) NULL,
  `friendly_name` VARCHAR(128) NULL,
  `ip_address` VARCHAR(128) NULL,
  `serial_number` VARCHAR(128) NULL,
  `model_number` VARCHAR(128) NULL,
  `mac_address` VARCHAR(128) NULL,
  `operating_system` VARCHAR(128) NULL,
  `notes` VARCHAR(10000) NULL,
  `date_purchase` DATE NULL,
  `date_retire` DATE NULL,
  PRIMARY KEY (`id`))
COMMENT = 'A list of AIM devices. Version 0.1';

