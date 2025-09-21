CREATE VIEW IF NOT EXISTS party_summary AS
SELECT 
    p.Id,
    p.count,
    p.dt_create,
    p.party_name,
    v.desc as vaccine_name,
    vv.version as vaccine_version
FROM parties p
INNER JOIN vaccines v ON p.vaccine_id = v.id
INNER JOIN vaccine_versions vv ON p.vaccine_version_id = vv.id;