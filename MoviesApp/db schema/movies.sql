--
-- PostgreSQL database dump
--

-- Dumped from database version 10.0
-- Dumped by pg_dump version 10.0

-- Started on 2017-10-12 20:32:52

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2812 (class 1262 OID 16722)
-- Name: movies; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE movies WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'English_United States.1252' LC_CTYPE = 'English_United States.1252';


ALTER DATABASE movies OWNER TO postgres;

\connect movies

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12924)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2814 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 197 (class 1259 OID 16732)
-- Name: crew; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE crew (
    name character varying(256) NOT NULL,
    imdb_profile character varying(256),
    crew_id integer NOT NULL
);


ALTER TABLE crew OWNER TO postgres;

--
-- TOC entry 196 (class 1259 OID 16726)
-- Name: movie; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE movie (
    movie_id integer NOT NULL,
    title character varying(256) NOT NULL,
    description text NOT NULL,
    title_original character varying(512) NOT NULL,
    last_update timestamp without time zone DEFAULT now() NOT NULL
);


ALTER TABLE movie OWNER TO postgres;

--
-- TOC entry 198 (class 1259 OID 16742)
-- Name: movie_crew; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE movie_crew (
    movie_id integer NOT NULL,
    crew_id integer NOT NULL,
    job character varying(50) NOT NULL
);


ALTER TABLE movie_crew OWNER TO postgres;

--
-- TOC entry 2682 (class 2606 OID 16746)
-- Name: crew crew_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY crew
    ADD CONSTRAINT crew_pkey PRIMARY KEY (crew_id);


--
-- TOC entry 2684 (class 2606 OID 16786)
-- Name: movie_crew movie_crew_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY movie_crew
    ADD CONSTRAINT movie_crew_pkey PRIMARY KEY (movie_id, crew_id);


--
-- TOC entry 2680 (class 2606 OID 16741)
-- Name: movie movie_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY movie
    ADD CONSTRAINT movie_pkey PRIMARY KEY (movie_id);


--
-- TOC entry 2685 (class 2606 OID 16765)
-- Name: movie_crew crew_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY movie_crew
    ADD CONSTRAINT crew_fk FOREIGN KEY (crew_id) REFERENCES crew(crew_id);


--
-- TOC entry 2686 (class 2606 OID 16770)
-- Name: movie_crew movies_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY movie_crew
    ADD CONSTRAINT movies_fk FOREIGN KEY (movie_id) REFERENCES movie(movie_id);


-- Completed on 2017-10-12 20:32:52

--
-- PostgreSQL database dump complete
--

