-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 13, 2025 at 09:44 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `aliasly`
--
CREATE DATABASE IF NOT EXISTS `aliasly` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `aliasly`;

-- --------------------------------------------------------

--
-- Table structure for table `felhasznalo`
--

CREATE TABLE `felhasznalo` (
  `felhasznalo_id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `url` varchar(255) NOT NULL,
  `hozzafuzes` varchar(1020) DEFAULT NULL,
  `jelszo_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `hozzafereslog`
--

CREATE TABLE `hozzafereslog` (
  `log_id` int(11) NOT NULL,
  `datum_ido` datetime NOT NULL DEFAULT current_timestamp(),
  `leiras` varchar(1020) NOT NULL,
  `jelszo_id` char(32) DEFAULT NULL,
  `felhasznalo_id` char(32) DEFAULT NULL,
  `mester_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `jelszo`
--

CREATE TABLE `jelszo` (
  `jelszo_id` int(11) NOT NULL,
  `jelszo_string` varchar(255) NOT NULL,
  `titkositas` char(32) NOT NULL,
  `mester_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `mesterkulcs`
--

CREATE TABLE `mesterkulcs` (
  `mester_id` int(11) NOT NULL,
  `encrypted_kulcs` char(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `felhasznalo`
--
ALTER TABLE `felhasznalo`
  ADD PRIMARY KEY (`felhasznalo_id`),
  ADD KEY `jelszo_id` (`jelszo_id`);

--
-- Indexes for table `hozzafereslog`
--
ALTER TABLE `hozzafereslog`
  ADD PRIMARY KEY (`log_id`),
  ADD KEY `mester_id` (`mester_id`);

--
-- Indexes for table `jelszo`
--
ALTER TABLE `jelszo`
  ADD PRIMARY KEY (`jelszo_id`),
  ADD KEY `mester_id` (`mester_id`);

--
-- Indexes for table `mesterkulcs`
--
ALTER TABLE `mesterkulcs`
  ADD PRIMARY KEY (`mester_id`),
  ADD UNIQUE KEY `encrypted_kulcs` (`encrypted_kulcs`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `felhasznalo`
--
ALTER TABLE `felhasznalo`
  MODIFY `felhasznalo_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `hozzafereslog`
--
ALTER TABLE `hozzafereslog`
  MODIFY `log_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `jelszo`
--
ALTER TABLE `jelszo`
  MODIFY `jelszo_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `mesterkulcs`
--
ALTER TABLE `mesterkulcs`
  MODIFY `mester_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `felhasznalo`
--
ALTER TABLE `felhasznalo`
  ADD CONSTRAINT `felhasznalo_ibfk_1` FOREIGN KEY (`jelszo_id`) REFERENCES `jelszo` (`jelszo_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `hozzafereslog`
--
ALTER TABLE `hozzafereslog`
  ADD CONSTRAINT `hozzafereslog_ibfk_1` FOREIGN KEY (`mester_id`) REFERENCES `mesterkulcs` (`mester_id`);

--
-- Constraints for table `jelszo`
--
ALTER TABLE `jelszo`
  ADD CONSTRAINT `jelszo_ibfk_1` FOREIGN KEY (`mester_id`) REFERENCES `mesterkulcs` (`mester_id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
