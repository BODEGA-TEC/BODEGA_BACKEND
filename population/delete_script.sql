-- Deshabilitar claves de seguridad
SET SQL_SAFE_UPDATES = 0;

-- Borrar todas las filas de la tabla
DELETE FROM sibe_db.Equipo;
DELETE FROM sibe_db.Componente;
DELETE FROM sibe_db.Categoria;
-- Habilitar claves de seguridad después de borrar las filas
SET SQL_SAFE_UPDATES = 1;

-- Reiniciar el índice autoincremental a 1
ALTER TABLE sibe_db.Equipo AUTO_INCREMENT = 1;
ALTER TABLE sibe_db.Componente AUTO_INCREMENT = 1;
ALTER TABLE sibe_db.Categoria AUTO_INCREMENT = 1;