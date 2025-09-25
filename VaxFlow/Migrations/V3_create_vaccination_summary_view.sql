CREATE VIEW IF NOT EXISTS vaccination_summary AS
SELECT
    COALESCE(d.last_name, '') || ' ' || SUBSTR(COALESCE(d.first_name, ''), 1, 1) || '.' || SUBSTR(COALESCE(d.patronymic, ''), 1, 1) || '.' AS doctor_initials,
    COALESCE(ps.vaccine_name, '') || ' ' || COALESCE(ps.vaccine_version, '') || ' (' || COALESCE(ps.party_name, '') || ')' AS desc_party,
    v.doctor_id,
    v.patient_id,
    v.party_id,
    v.dt_of_vaccination
FROM vaccinations v
INNER JOIN party_summary ps ON ps.id = v.party_id
INNER JOIN doctors d ON d.id = v.doctor_id;