CREATE VIEW IF NOT EXISTS vaccine_summary AS
SELECT 
    v.id,
    v.count,
    v.dt_create,
    v.party_name,
    v.series,
    v.disease_id,
    v.expiration_date,
    d.desc as disease_name,
    v.vaccine_version_id,
    vv.version as vaccine_version
FROM vaccines v
INNER JOIN diseases d ON v.disease_id = d.id
INNER JOIN vaccine_versions vv ON v.vaccine_version_id = vv.id;