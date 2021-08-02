-- Anthony Isensee
-- 7/30/21
-- DB Version: 0.1
-- Location: Local Database

-- Seed data into inventory_local_test database.
INSERT INTO `inventory_local_test`.`device` (`id`, `type`, `friendly_name`, `ip_address`, `serial_number`, `model_number`, `mac_address`, `operating_system`, `notes`, `date_purchase`) VALUES ('0', 'Laptop', 'Lynx', 'Dynamic', 'VWPZF63', 'Latitude 3510', 'B0-7B-25-81-F6-8C', 'Windows 10', 'Anthony Isensee\'s work laptop.', '2021-07-30');
INSERT INTO `inventory_local_test`.`device` (`id`, `type`, `friendly_name`, `ip_address`, `operating_system`, `notes`, `date_purchase`) VALUES ('1', 'Virtual Machine', 'Walrus', 'Dynamic', 'Windows Server 2019', 'New IT test machine.', '2021-06-15');
INSERT INTO `inventory_local_test`.`device` (`id`, `type`, `friendly_name`, `ip_address`, `operating_system`, `notes`, `date_purchase`) VALUES ('2', 'Server', 'HC1', '172.30.10.11', 'Windows Server 2019', 'To be a fail over clustered VM host.', '2021-04-12');
INSERT INTO `inventory_local_test`.`device` (`id`, `type`, `friendly_name`, `ip_address`, `operating_system`, `notes`, `date_purchase`) VALUES ('3', 'Server', 'Lion2', '10.100.1.10', 'Windows Server 2019', 'Main domain controller.', '2021-05-01');
INSERT INTO `inventory_local_test`.`device` (`id`, `type`, `friendly_name`, `ip_address`, `operating_system`, `notes`, `date_retire`) VALUES ('4', 'Server', 'Lion', '143.207.96.10', 'Windows Server 2016', 'Was a domain controller. Drive error caused it to go down.', '2021-06-03');