CREATE VIEW IF NOT EXISTS patient_summary AS
SELECT
    COALESCE(p.last_name, '') || ' ' || SUBSTR(COALESCE(p.first_name, ''), 1, 1) || '.' || SUBSTR(COALESCE(p.patronymic, ''), 1, 1) || '.' AS patient_initials,
    p.id,
    p.last_name,
    p.first_name,
    p.patronymic,
    p.name_suffix,
    p.policy_number,
    p.dt_create
FROM patients p;