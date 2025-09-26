CREATE VIEW IF NOT EXISTS party_summary AS
SELECT 
    p.id,
    p.count,
    p.dt_create,
    p.party_name,
    p.vaccine_id,
    d.desc as disease_name,
    d.vaccine_version_id,
    vv.version as vaccine_version
FROM parties p
INNER JOIN diseases d ON p.disease_id = d.id
INNER JOIN vaccine_versions vv ON p.vaccine_version_id = vv.id;