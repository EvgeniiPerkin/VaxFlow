CREATE TABLE IF NOT EXISTS doctors (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    last_name TEXT NOT NULL CHECK(length(last_name) <= 100),
    first_name TEXT NOT NULL CHECK(length(first_name) <= 100),
    patronymic TEXT CHECK(length(patronymic) <= 100),
    name_suffix TEXT CHECK(length(name_suffix) <= 100),
    is_dismissed INTEGER NOT NULL CHECK (is_dismissed IN (0, 1))
);
CREATE TABLE IF NOT EXISTS job_categories (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    category TEXT NOT NULL CHECK(length(category) <= 150),
    desc TEXT NOT NULL CHECK(length(desc) <= 255)
);
CREATE TABLE IF NOT EXISTS diseases (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    desc TEXT NOT NULL CHECK(length(desc) <= 255)
);
CREATE TABLE IF NOT EXISTS vaccine_versions (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    version TEXT NOT NULL CHECK(length(version) <= 10)
);
CREATE TABLE IF NOT EXISTS vaccines (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    disease_id INTEGER NOT NULL,
    vaccine_version_id INTEGER NOT NULL,
    vaccine_name TEXT NOT NULL CHECK(length(party_name) <= 255),
    series TEXT NOT NULL CHECK(length(party_name) <= 50),
    count INTEGER NOT NULL DEFAULT 0,
    expiration_date TEXT NOT NULL,
    dt_create TEXT NOT NULL,
    FOREIGN KEY (disease_id) REFERENCES diseases(id) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (vaccine_version_id) REFERENCES vaccine_versions(id) 
        ON DELETE CASCADE ON UPDATE CASCADE
);
CREATE TABLE IF NOT EXISTS patients (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    full_name TEXT NOT NULL CHECK(length(full_name) <= 255),
    birthday TEXT NOT NULL,
    registration_address TEXT NOT NULL CHECK(length(registration_address) <= 255),
    dt_create TEXT NOT NULL,
    policy_number TEXT NOT NULL CHECK(length(policy_number) <= 30),
    working_position TEXT NOT NULL CHECK(length(working_position) <= 255),
    job_category_id INTEGER,
    FOREIGN KEY (job_category_id) REFERENCES job_categories(id) 
        ON DELETE SET NULL
);
CREATE TABLE IF NOT EXISTS vaccinations (
    doctor_id INTEGER NOT NULL,
    patient_id INTEGER NOT NULL,
    vaccine_id INTEGER NOT NULL,
    dt_of_vaccination TEXT NOT NULL,
    PRIMARY KEY (doctor_id, patient_id, vaccine_id),
    FOREIGN KEY (doctor_id) REFERENCES doctors(id) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (patient_id) REFERENCES patients(id) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (vaccine_id) REFERENCES vaccines(id) 
        ON DELETE CASCADE ON UPDATE CASCADE
);
CREATE TABLE IF NOT EXISTS patterns ( 
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    desc TEXT NOT NULL CHECK(length(desc) <= 255)
);
CREATE TABLE IF NOT EXISTS parts ( 
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    pattern_id INTEGER NOT NULL,
    serial_number INTEGER NOT NULL,
    body TEXT NOT NULL CHECK(length(body) <= 2048),
    desc TEXT CHECK(length(desc) <= 255),
    is_url INTEGER NOT NULL CHECK (is_url IN (0, 1)),
    url TEXT CHECK(length(url) <= 200),
    is_bold INTEGER NOT NULL CHECK (is_bold IN (0, 1)),
    is_italic INTEGER NOT NULL CHECK (is_italic IN (0, 1)),
    is_underline INTEGER NOT NULL CHECK (is_underline IN (0, 1)),
    FOREIGN KEY (pattern_id) REFERENCES patterns(id) 
        ON DELETE CASCADE ON UPDATE CASCADE
);