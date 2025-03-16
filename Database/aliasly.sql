-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 16, 2025 at 01:27 PM
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

-- --------------------------------------------------------

--
-- Table structure for table `adatok`
--

CREATE TABLE `adatok` (
  `id` int(11) NOT NULL,
  `jelszo` varchar(255) NOT NULL,
  `email` varchar(255) DEFAULT NULL,
  `nev` varchar(255) DEFAULT NULL,
  `url` varchar(255) DEFAULT NULL,
  `hozzafuzes` varchar(1020) DEFAULT NULL,
  `mester_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `hozzafereslog`
--

CREATE TABLE `hozzafereslog` (
  `log_id` int(11) NOT NULL,
  `datum_ido` datetime DEFAULT NULL,
  `ipcim` varchar(15) DEFAULT NULL,
  `leiras` varchar(1020) DEFAULT NULL,
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `mesterkulcs`
--

CREATE TABLE `mesterkulcs` (
  `mester_id` int(11) NOT NULL,
  `kulcs_string` char(64) NOT NULL,
  `salt` char(64) NOT NULL,
  `mester_kulcs` char(128) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `adatok`
--
ALTER TABLE `adatok`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_adat_mester` (`mester_id`);

--
-- Indexes for table `hozzafereslog`
--
ALTER TABLE `hozzafereslog`
  ADD PRIMARY KEY (`log_id`),
  ADD KEY `fk_log_adat` (`id`);

--
-- Indexes for table `mesterkulcs`
--
ALTER TABLE `mesterkulcs`
  ADD PRIMARY KEY (`mester_id`),
  ADD UNIQUE KEY `kulcs_string` (`kulcs_string`),
  ADD UNIQUE KEY `salt` (`salt`),
  ADD UNIQUE KEY `mester_kulcs` (`mester_kulcs`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `adatok`
--
ALTER TABLE `adatok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `hozzafereslog`
--
ALTER TABLE `hozzafereslog`
  MODIFY `log_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `mesterkulcs`
--
ALTER TABLE `mesterkulcs`
  MODIFY `mester_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `adatok`
--
ALTER TABLE `adatok`
  ADD CONSTRAINT `fk_adat_mester` FOREIGN KEY (`mester_id`) REFERENCES `mesterkulcs` (`mester_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `hozzafereslog`
--
ALTER TABLE `hozzafereslog`
  ADD CONSTRAINT `fk_log_adat` FOREIGN KEY (`id`) REFERENCES `adatok` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
