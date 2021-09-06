SELECT * FROM `device` WHERE `friendly_name` LIKE "%virtual%" 
	OR `type` LIKE "%virtual%"
    OR `ip_address` LIKE "%virtual%";