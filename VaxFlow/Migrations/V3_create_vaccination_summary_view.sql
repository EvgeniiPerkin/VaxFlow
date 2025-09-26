CREATE VIEW IF NOT EXISTS vaccination_summary AS
SELECT
    COALESCE(d.last_name, '') || ' ' || SUBSTR(COALESCE(d.first_name, ''), 1, 1) || '.' || SUBSTR(COALESCE(d.patronymic, ''), 1, 1) || '.' AS doctor_initials,
    COALESCE(vs.vaccine_name, '') || ' ' || COALESCE(vs.vaccine_version, '') || ' (' || COALESCE(vs.vaccine_name, '') || ')' AS desc_vaccine,
    v.doctor_id,
    v.patient_id,
    v.vaccine_id,
    v.dt_of_vaccination
FROM vaccinations v
INNER JOIN vaccine_summary vs ON vs.id = v.vaccine_id
INNER JOIN doctors d ON d.id = v.doctor_id;