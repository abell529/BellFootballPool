-- Creates the 2026 season tables by cloning the 2025 structure.
-- Picks tables go in bfpooldb; the scoreboard table goes in bfscoresdb.
--
-- Run in phpMyAdmin: click the server name at the top of the left tree
-- (not a database), open the SQL tab, paste everything, click Go.
-- The USE statements switch databases, so nothing needs to be selected first.
--
-- CREATE TABLE ... LIKE copies columns, types, indexes and the AUTO_INCREMENT id.
-- It creates EMPTY tables. No rows are copied.
-- It fails if a table already exists, so it is safe to re-run.

-- ---------------------------------------------------------------
-- Weekly picks tables (bfpooldb)
-- Columns: id, firstname, lastname, email, game1 .. game16
-- ---------------------------------------------------------------
USE `bfpooldb`;

CREATE TABLE `one2026`       LIKE `one2025`;
CREATE TABLE `two2026`       LIKE `two2025`;
CREATE TABLE `three2026`     LIKE `three2025`;
CREATE TABLE `four2026`      LIKE `four2025`;
CREATE TABLE `five2026`      LIKE `five2025`;
CREATE TABLE `six2026`       LIKE `six2025`;
CREATE TABLE `seven2026`     LIKE `seven2025`;
CREATE TABLE `eight2026`     LIKE `eight2025`;
CREATE TABLE `nine2026`      LIKE `nine2025`;
CREATE TABLE `ten2026`       LIKE `ten2025`;
CREATE TABLE `eleven2026`    LIKE `eleven2025`;
CREATE TABLE `twelve2026`    LIKE `twelve2025`;
CREATE TABLE `thirteen2026`  LIKE `thirteen2025`;
CREATE TABLE `fourteen2026`  LIKE `fourteen2025`;
CREATE TABLE `fifteen2026`   LIKE `fifteen2025`;
CREATE TABLE `sixteen2026`   LIKE `sixteen2025`;
CREATE TABLE `seventeen2026` LIKE `seventeen2025`;
CREATE TABLE `eighteen2026`  LIKE `eighteen2025`;

-- ---------------------------------------------------------------
-- Season scoreboard table (bfscoresdb)
-- Columns: firstname, lastname, week1 .. week18, total
-- ---------------------------------------------------------------
USE `bfscoresdb`;

CREATE TABLE `2026` LIKE `2025`;

-- Optional, NOT run unless you uncomment it:
-- carry last year's players into the 2026 scoreboard with empty scores.
-- The weekly score entry page UPDATEs existing rows and never INSERTs,
-- so each player needs a row here before their first week is scored.
-- INSERT INTO `2026` (firstname, lastname) SELECT firstname, lastname FROM `2025`;
