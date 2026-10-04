-- Adds the week 1 players to the 2026 scoreboard table in bfscoresdb.
-- Run in phpMyAdmin: select bfscoresdb, open the SQL tab, paste, click Go.
--
-- Names come from the week 1 picks page (one2026), trimmed.
-- Week columns and total are left empty; the score entry page fills them in.
-- The NOT EXISTS check skips anyone already in the table, so this is safe to re-run.

USE `bfscoresdb`;

INSERT INTO `2026` (firstname, lastname)
SELECT n.firstname, n.lastname
FROM (
          SELECT 'MacKenzie' AS firstname,    'Andrews' AS lastname
UNION ALL SELECT 'Paul',         'Andrews'
UNION ALL SELECT 'Andy',         'Bell'
UNION ALL SELECT 'Big Pete',     'Bell'
UNION ALL SELECT 'Dongle',       'Bell'
UNION ALL SELECT 'Richard Bing', 'Bell'
UNION ALL SELECT 'Rick',         'Bell'
UNION ALL SELECT 'Mark',         'Fleming'
UNION ALL SELECT 'Louis',        'Foudos'
UNION ALL SELECT 'George',       'Franklin'
UNION ALL SELECT 'Jill',         'Franklin'
UNION ALL SELECT 'Kathy',        'Franklin'
UNION ALL SELECT 'Eryn',         'Gray'
UNION ALL SELECT 'Steve',        'Graziano'
UNION ALL SELECT 'Steve',        'Johanson'
UNION ALL SELECT 'Ashley',       'Kreitz'
UNION ALL SELECT 'Terry',        'Kreitz'
UNION ALL SELECT 'Jennifer',     'Lindgren'
UNION ALL SELECT 'Ann',          'Rea'
UNION ALL SELECT 'Bill',         'Rea'
UNION ALL SELECT 'Paul',         'Siegmund'
UNION ALL SELECT 'Dorothy',      'Smock'
UNION ALL SELECT 'Matt',         'Voorhees'
) AS n
WHERE NOT EXISTS (
    SELECT 1 FROM `2026` s
    WHERE UPPER(TRIM(s.firstname)) = UPPER(n.firstname)
      AND UPPER(TRIM(s.lastname))  = UPPER(n.lastname)
);

-- Check the result:
SELECT firstname, lastname, total FROM `2026` ORDER BY lastname, firstname;
