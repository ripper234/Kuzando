/*
SQLyog Enterprise - MySQL GUI v7.02 
MySQL - 5.1.42-community : Database - kuzando
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE DATABASE /*!32312 IF NOT EXISTS*/`kuzando` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `kuzando`;

/*Table structure for table `tasks` */

DROP TABLE IF EXISTS `tasks`;

CREATE TABLE `tasks` (
  `Id` int(4) unsigned NOT NULL AUTO_INCREMENT,
  `UserId` int(4) unsigned NOT NULL,
  `Text` varchar(5000) NOT NULL,
  `CreationDate` datetime NOT NULL,
  `DueDate` datetime DEFAULT NULL,
  `IsDone` tinyint(1) NOT NULL,
  `Importance` tinyint(4) NOT NULL,
  `PriorityInDay` tinyint(3) unsigned NOT NULL,
  `Deleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `Id` int(4) unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `SignupDate` datetime NOT NULL,
  `OpenId` varchar(100) NOT NULL,
  `Email` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
