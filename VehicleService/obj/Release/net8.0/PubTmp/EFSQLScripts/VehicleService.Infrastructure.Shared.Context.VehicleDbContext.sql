CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241020063500_InitialMigration') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241020063500_InitialMigration') THEN

    CREATE TABLE `Vehicles` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Brand` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Model` longtext CHARACTER SET utf8mb4 NOT NULL,
        `MaximumSpeed` int NOT NULL,
        `Consumption` int NOT NULL,
        `Dimensions` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Weight` int NOT NULL,
        `CarClass` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Transmission` longtext CHARACTER SET utf8mb4 NOT NULL,
        `TimeType` longtext CHARACTER SET utf8mb4 NOT NULL,
        `RentalCost` int NOT NULL,
        `PickUpPlace` longtext CHARACTER SET utf8mb4 NOT NULL,
        `UrlImage` longtext CHARACTER SET utf8mb4 NOT NULL,
        `RentStatus` longtext CHARACTER SET utf8mb4 NOT NULL,
        `OwnerId` int NOT NULL,
        `IsActive` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Vehicles` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241020063500_InitialMigration') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241020063500_InitialMigration', '8.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

