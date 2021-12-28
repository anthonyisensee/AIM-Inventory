-- Anthony Isensee
-- 12/27/21
-- Version: 1.0.0
-- Location: Production database.

-- Create schema.
CREATE SCHEMA `inventory`;

-- Create device table.
CREATE TABLE `inventory`.`device` (
  `id` INT NOT NULL AUTO_INCREMENT,
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
COMMENT = 'A list of AIM devices. Version 1.0.0';
