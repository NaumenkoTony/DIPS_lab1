\connect persons

CREATE TABLE IF NOT EXISTS person
(
	id SERIAL PRIMARY KEY,
	name varchar NOT NULL,
	age int,
	address varchar,
	WORK varchar
);

GRANT ALL PRIVILEGES ON TABLE person TO program;
GRANT USAGE, SELECT ON SEQUENCE person_id_seq TO program;