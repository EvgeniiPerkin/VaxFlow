CREATE VIEW IF NOT EXISTS appointment_summary AS
SELECT
    COALESCE(d.last_name, '') || ' ' || SUBSTR(COALESCE(d.first_name, ''), 1, 1) || '.' || SUBSTR(COALESCE(d.patronymic, ''), 1, 1) || '.' AS doctor_initials,
    COALESCE(p.last_name, '') || ' ' || SUBSTR(COALESCE(p.first_name, ''), 1, 1) || '.' || SUBSTR(COALESCE(p.patronymic, ''), 1, 1) || '.' AS patient_initials,
    COALESCE(ps.vaccine_name, '') || ' ' || COALESCE(ps.vaccine_version, '') || ' (' || COALESCE(ps.party_name, '') || ')' AS desc_party,
    da.doctor_id,
    da.patient_id,
    da.party_id,
    da.dt_of_appointment
FROM doctors_appointments da
INNER JOIN patients p ON p.id = da.patient_id
INNER JOIN party_summary ps ON ps.id = da.party_id
INNER JOIN doctors d ON d.id = da.doctor_id;