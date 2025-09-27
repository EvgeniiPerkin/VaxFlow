CREATE TRIGGER after_vaccinations_delete
AFTER DELETE ON vaccinations
FOR EACH ROW
BEGIN
    UPDATE vaccines 
    SET count = count + 1
    WHERE id = OLD.vaccine_id;
END;

CREATE TRIGGER after_vaccinations_insert
AFTER INSERT ON vaccinations
FOR EACH ROW
BEGIN
    UPDATE vaccines 
    SET count = count - 1
    WHERE id = NEW.vaccine_id;
END;