-- Deshabilitar claves de seguridad
SET SQL_SAFE_UPDATES = 0;

-- Borrar todas las filas de la tabla
DELETE FROM sibe_db.equipo;
DELETE FROM sibe_db.componente;
DELETE FROM sibe_db.categoria;
-- Habilitar claves de seguridad después de borrar las filas
SET SQL_SAFE_UPDATES = 1;

-- Reiniciar el índice autoincremental a 1
ALTER TABLE sibe_db.equipo AUTO_INCREMENT = 1;
ALTER TABLE sibe_db.componente AUTO_INCREMENT = 1;
ALTER TABLE sibe_db.categoria AUTO_INCREMENT = 1;