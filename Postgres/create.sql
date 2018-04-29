--
-- PostgreSQL database dump
--

-- Dumped from database version 10.3
-- Dumped by pg_dump version 10.3

-- Started on 2018-04-29 18:37:28

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2266 (class 0 OID 0)
-- Dependencies: 2265
-- Name: DATABASE postgres; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON DATABASE postgres IS 'default administrative connection database';


--
-- TOC entry 1 (class 3079 OID 12278)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2268 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


--
-- TOC entry 232 (class 1255 OID 16384)
-- Name: MNOHO_MIN(integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."MNOHO_MIN"(param_pole_pole_id integer, param_hra_pocet_oznacenych_min integer) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
declare max_pocet_min integer;

begin
  select obtiznost.pocet_min into max_pocet_min from public."OBTIZNOST" obtiznost 
  inner join public."OBLAST" oblast on oblast.obtiznost = obtiznost.obtiznost_id 
  inner join public."POLE" pole on pole.oblast = oblast.oblast_id 
  where pole.pole_id = param_pole_pole_id;
  
  IF max_pocet_min = param_hra_pocet_oznacenych_min THEN
    return FALSE;
  ELSE
    return TRUE;
  END IF;
end
$$;


ALTER FUNCTION public."MNOHO_MIN"(param_pole_pole_id integer, param_hra_pocet_oznacenych_min integer) OWNER TO postgres;

--
-- TOC entry 234 (class 1255 OID 16385)
-- Name: ODKRYJ_POLE(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."ODKRYJ_POLE"(param_pole_pole_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
declare je_zaminovano integer;
declare pole_je_zaminovano "POLE"%ROWTYPE;
declare pole_soused_id integer;
  
declare pole_odkryvam "POLE"%ROWTYPE;

begin
  
  /* ziskam vsechny informace o poli */
  select * into pole_je_zaminovano from "POLE" pole where pole.pole_id = param_pole_pole_id;
  
  /* zkontroluji zda neni pole oznaceno jako zaminovane */
  select count(mina.mina_id) into je_zaminovano from "MINA" mina where mina.oblast = pole_je_zaminovano.oblast AND mina.souradnice_x = pole_je_zaminovano.souradnice_x AND mina.souradnice_y = pole_je_zaminovano.souradnice_y; 

  /* odkryvam pouze pokud pole neni oznaceno jako zaminovane */
  IF je_zaminovano = 0 THEN
    /* ziskam pole pouze pokud neni zobrazeno, nema zadnou sousedici minu a samo neni minou */
    select * into pole_odkryvam from "POLE" pole where pole.pole_id = param_pole_pole_id AND pole.odkryto = false AND pole.je_mina = false;
    
    update "POLE" pole set odkryto = true where pole.pole_id = pole_odkryvam.pole_id;
    
    /* pokud pole ma miny jako sousedni pole, uz nevolej rekurzi */
    IF pole_odkryvam.sousedni_miny = 0 THEN
      
      select pole.pole_id into pole_soused_id from "POLE" pole where pole.oblast = pole_odkryvam.oblast and pole.souradnice_x = pole_odkryvam.souradnice_x - 1 and pole.souradnice_y = pole_odkryvam.souradnice_y - 1;
      IF pole_soused_id IS NOT NULL THEN
       PERFORM "ODKRYJ_POLE"(pole_soused_id);
      END IF;
      
      select pole.pole_id into pole_soused_id from "POLE" pole where pole.oblast = pole_odkryvam.oblast and pole.souradnice_x = pole_odkryvam.souradnice_x - 1 and pole.souradnice_y = pole_odkryvam.souradnice_y;
      IF pole_soused_id IS NOT NULL THEN
        PERFORM "ODKRYJ_POLE"(pole_soused_id);
      END IF;
      
      select pole.pole_id into pole_soused_id from "POLE" pole where pole.oblast = pole_odkryvam.oblast and pole.souradnice_x = pole_odkryvam.souradnice_x - 1 and pole.souradnice_y = pole_odkryvam.souradnice_y + 1;
      IF pole_soused_id IS NOT NULL THEN
        PERFORM "ODKRYJ_POLE"(pole_soused_id);
      END IF;
      
      select pole.pole_id into pole_soused_id from "POLE" pole where pole.oblast = pole_odkryvam.oblast and pole.souradnice_x = pole_odkryvam.souradnice_x and pole.souradnice_y = pole_odkryvam.souradnice_y + 1;
      IF pole_soused_id IS NOT NULL THEN
        PERFORM "ODKRYJ_POLE"(pole_soused_id);
      END IF;
      
      select pole.pole_id into pole_soused_id from "POLE" pole where pole.oblast = pole_odkryvam.oblast and pole.souradnice_x = pole_odkryvam.souradnice_x + 1 and pole.souradnice_y = pole_odkryvam.souradnice_y + 1;
      IF pole_soused_id IS NOT NULL THEN
        PERFORM "ODKRYJ_POLE"(pole_soused_id);
      END IF;
      
      select pole.pole_id into pole_soused_id from "POLE" pole where pole.oblast = pole_odkryvam.oblast and pole.souradnice_x = pole_odkryvam.souradnice_x + 1 and pole.souradnice_y = pole_odkryvam.souradnice_y;
      IF pole_soused_id IS NOT NULL THEN
        PERFORM "ODKRYJ_POLE"(pole_soused_id);
      END IF;
      
      select pole.pole_id into pole_soused_id from "POLE" pole where pole.oblast = pole_odkryvam.oblast and pole.souradnice_x = pole_odkryvam.souradnice_x + 1 and pole.souradnice_y = pole_odkryvam.souradnice_y - 1;
      IF pole_soused_id IS NOT NULL THEN
        PERFORM "ODKRYJ_POLE"(pole_soused_id);
      END IF;
      
      select pole.pole_id into pole_soused_id from "POLE" pole where pole.oblast = pole_odkryvam.oblast and pole.souradnice_x = pole_odkryvam.souradnice_x and pole.souradnice_y = pole_odkryvam.souradnice_y - 1;
      IF pole_soused_id IS NOT NULL THEN
        PERFORM "ODKRYJ_POLE"(pole_soused_id);
      END IF;
      
    END IF;  
    
  END IF;  

EXCEPTION
  WHEN NO_DATA_FOUND THEN
    NULL;

end
$$;


ALTER FUNCTION public."ODKRYJ_POLE"(param_pole_pole_id integer) OWNER TO postgres;

--
-- TOC entry 238 (class 1255 OID 16386)
-- Name: ODKRYTA_MINA(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."ODKRYTA_MINA"(param_pole_pole_id integer) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
DECLARE 
  result boolean;

begin
  SELECT pole.je_mina INTO result from "POLE" pole where pole.pole_id = param_pole_pole_id;
  
  return result;
end
$$;


ALTER FUNCTION public."ODKRYTA_MINA"(param_pole_pole_id integer) OWNER TO postgres;

--
-- TOC entry 224 (class 1255 OID 16387)
-- Name: OZNAC_MINY(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."OZNAC_MINY"(param_oblast_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE pole_p "POLE"%ROWTYPE;
DECLARE list_poli CURSOR FOR SELECT * FROM "POLE" pole where pole.oblast = param_oblast_id and pole.je_mina = true and pole.odkryto = false;
declare mina_pole_id integer;

begin
  
  FOR pole_p IN list_poli LOOP
  
    /* overim zda uz neni pole s minou oznacene */
    SELECT COUNT(mina.mina_id) INTO mina_pole_id FROM "MINA" mina WHERE mina.oblast = pole_p.oblast AND mina.souradnice_x = pole_p.souradnice_x AND mina.souradnice_y = pole_p.souradnice_y;
    
    IF mina_pole_id = 0 THEN 
      INSERT INTO "MINA"(oblast, souradnice_x, souradnice_y) VALUES (pole_p.oblast, pole_p.souradnice_x, pole_p.souradnice_y);
    END IF;

  
  END LOOP;
end
$$;


ALTER FUNCTION public."OZNAC_MINY"(param_oblast_id integer) OWNER TO postgres;

--
-- TOC entry 230 (class 1255 OID 16388)
-- Name: RADEK_OBLASTI(integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."RADEK_OBLASTI"(param_oblast_oblast_id integer, param_radek integer) RETURNS character varying
    LANGUAGE plpgsql
    AS $$
DECLARE radek varchar(100);
DECLARE tmp varchar(100);
DECLARE oznacena_mina integer;
DECLARE pole_p "POLE"%ROWTYPE;
DECLARE list_poli CURSOR FOR SELECT * FROM "POLE" pole where pole.souradnice_y = param_radek and pole.oblast = param_oblast_oblast_id
        ORDER BY pole.souradnice_x;

begin
  /* oznaceni poli
    pole neni zobrazene: '_'
    pole je zobrazene:
      nema sousedy '-'
      ma sousedy 'n' (pocet sousedu)
      oznaceno jako mina '?'
  */

  FOR pole_p IN list_poli LOOP
    select count(*) into oznacena_mina from "MINA" mina where mina.oblast = pole_p.oblast 
    AND mina.souradnice_x = pole_p.souradnice_x AND mina.souradnice_y = pole_p.souradnice_y;
    
    IF oznacena_mina = 1 THEN
      select CONCAT(radek, '?') into radek;
    
    ELSEIF pole_p.odkryto = true THEN
      IF pole_p.sousedni_miny = 0 THEN
        select CONCAT(radek, 'X') into radek;
      ELSE
        select CONCAT(radek, pole_p.sousedni_miny) into radek;
      END IF;
    
    ELSE
      select CONCAT(radek, '_') into radek;
    
    END IF;
  
    select CONCAT(radek, ' ') into radek;
  
  END LOOP;
  
  RETURN radek;

end
$$;


ALTER FUNCTION public."RADEK_OBLASTI"(param_oblast_oblast_id integer, param_radek integer) OWNER TO postgres;

--
-- TOC entry 226 (class 1255 OID 16389)
-- Name: SPATNY_PARAMETR(integer, integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."SPATNY_PARAMETR"(param_sirka integer, param_vyska integer, param_pocet_min integer) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
DECLARE omezeni_o "OMEZENI"%ROWTYPE;
DECLARE pole_obsah integer;
DECLARE pocet_min_min integer;
DECLARE pocet_min_max integer;

begin
  SELECT * into omezeni_o FROM "OMEZENI" omezeni WHERE omezeni.omezeni_id = 1;

  pole_obsah := param_vyska * param_sirka;
  pocet_min_min := (omezeni_o.pocet_min_min / 100) * pole_obsah;
  pocet_min_max := (omezeni_o.pocet_min_max / 100) * pole_obsah;
  
  IF param_vyska < omezeni_o.vyska_min OR
    param_vyska > omezeni_o.vyska_max OR
    param_sirka < omezeni_o.sirka_min OR
    param_sirka > omezeni_o.sirka_max OR
    param_pocet_min < pocet_min_min OR
    param_pocet_min > pocet_min_max THEN
    
    return false;
  ELSE
    RETURN true;
  END IF; 
  
EXCEPTION
  WHEN NO_DATA_FOUND THEN
    raise exception 'OMEZENI s ID = 1 nebylo nalezeno';

end
$$;


ALTER FUNCTION public."SPATNY_PARAMETR"(param_sirka integer, param_vyska integer, param_pocet_min integer) OWNER TO postgres;

--
-- TOC entry 218 (class 1255 OID 16390)
-- Name: SPOCITEJ_OBLAST(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."SPOCITEJ_OBLAST"(param_oblast_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE pole_p "POLE"%ROWTYPE;
DECLARE list_poli CURSOR FOR SELECT * FROM "POLE" pole where pole.oblast = param_oblast_id and pole.je_mina = false;
DECLARE je_mina boolean;
begin
  
  --OPEN list_poli;
  FOR pole_p IN list_poli LOOP
      
      select pole.je_mina into je_mina from "POLE" pole where pole.oblast = pole_p.oblast and pole.souradnice_x = pole_p.souradnice_x - 1 and pole.souradnice_y = pole_p.souradnice_y - 1;
      if je_mina = true then
         pole_p.sousedni_miny := pole_p.sousedni_miny + 1;
      end if; 
      
      select pole.je_mina into je_mina from "POLE" pole where pole.oblast = pole_p.oblast and pole.souradnice_x = pole_p.souradnice_x - 1 and pole.souradnice_y = pole_p.souradnice_y;
      if je_mina = true then
         pole_p.sousedni_miny := pole_p.sousedni_miny + 1;
      end if; 
      
      select pole.je_mina into je_mina from "POLE" pole where pole.oblast = pole_p.oblast and pole.souradnice_x = pole_p.souradnice_x - 1 and pole.souradnice_y = pole_p.souradnice_y + 1;
      if je_mina = true then
         pole_p.sousedni_miny := pole_p.sousedni_miny + 1;
      end if; 
      
      select pole.je_mina into je_mina from "POLE" pole where pole.oblast = pole_p.oblast and pole.souradnice_x = pole_p.souradnice_x and pole.souradnice_y = pole_p.souradnice_y + 1;
      if je_mina = true then
         pole_p.sousedni_miny := pole_p.sousedni_miny + 1;
      end if; 
      
      select pole.je_mina into je_mina from "POLE" pole where pole.oblast = pole_p.oblast and pole.souradnice_x = pole_p.souradnice_x + 1 and pole.souradnice_y = pole_p.souradnice_y + 1;
      if je_mina = true then
         pole_p.sousedni_miny := pole_p.sousedni_miny + 1;
      end if; 
      
      select pole.je_mina into je_mina from "POLE" pole where pole.oblast = pole_p.oblast and pole.souradnice_x = pole_p.souradnice_x + 1 and pole.souradnice_y = pole_p.souradnice_y;
      if je_mina = true then
         pole_p.sousedni_miny := pole_p.sousedni_miny + 1;
      end if; 
      
      select pole.je_mina into je_mina from "POLE" pole where pole.oblast = pole_p.oblast and pole.souradnice_x = pole_p.souradnice_x + 1 and pole.souradnice_y = pole_p.souradnice_y - 1;
      if je_mina = true then
         pole_p.sousedni_miny := pole_p.sousedni_miny + 1;
      end if; 
      
      select pole.je_mina into je_mina from "POLE" pole where pole.oblast = pole_p.oblast and pole.souradnice_x = pole_p.souradnice_x and pole.souradnice_y = pole_p.souradnice_y - 1;
      if je_mina = true then
         pole_p.sousedni_miny := pole_p.sousedni_miny + 1;
      end if; 
      
      update "POLE" pole set sousedni_miny = pole_p.sousedni_miny where pole.pole_id = pole_p.pole_id;
      
  END LOOP;
  
  RAISE NOTICE 'Oblast je spocitana. ID: %', param_oblast_id;
end
$$;


ALTER FUNCTION public."SPOCITEJ_OBLAST"(param_oblast_id integer) OWNER TO postgres;

--
-- TOC entry 217 (class 1255 OID 16391)
-- Name: TRIGGER_ODEBER_MINU(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TRIGGER_ODEBER_MINU"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare moje_hra_id integer;
declare pocet_min integer;

begin
    /* vyberu aktualni pocet min ve hre */
  SELECT hra.hra_id, hra.pocet_oznacenych_min INTO moje_hra_id, pocet_min FROM "HRA" hra 
  INNER JOIN "OBLAST" oblast on hra.oblast = oblast.oblast_id
  INNER JOIN "POLE" pole ON pole.oblast = oblast.oblast_id
  WHERE pole.oblast = OLD.oblast AND pole.souradnice_x = OLD.souradnice_x AND pole.souradnice_y = OLD.souradnice_y;
  
  pocet_min := pocet_min - 1;
  UPDATE "HRA" hra SET pocet_oznacenych_min = pocet_min WHERE hra.hra_id = moje_hra_id;
 
  RETURN OLD;
  
  EXCEPTION
  WHEN no_data_found then
    raise exception 'Zadane pole nenalezeno';
end
$$;


ALTER FUNCTION public."TRIGGER_ODEBER_MINU"() OWNER TO postgres;

--
-- TOC entry 240 (class 1255 OID 16392)
-- Name: TRIGGER_OMEZENI_OBTIZNOSTI(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TRIGGER_OMEZENI_OBTIZNOSTI"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$declare vysledek boolean;

begin
vysledek := "SPATNY_PARAMETR"(NEW.sirka, NEW.vyska, NEW.pocet_min);
 return new; 
  IF vysledek = false THEN
    raise exception 'Zadana obtiznost nesplnuje minimalni parametry';
  END IF;
end
$$;


ALTER FUNCTION public."TRIGGER_OMEZENI_OBTIZNOSTI"() OWNER TO postgres;

--
-- TOC entry 216 (class 1255 OID 16393)
-- Name: TRIGGER_OZNAC_MINU(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TRIGGER_OZNAC_MINU"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare moje_hra_id integer;
declare pocet_min integer;
declare vybrane_pole_id integer;
declare lze_oznacit_minu boolean;
declare odkryte_pole boolean;
begin
  /* vyberu aktualni pocet min ve hre */
  SELECT hra.hra_id, hra.pocet_oznacenych_min, pole.pole_id, pole.odkryto INTO moje_hra_id, pocet_min, vybrane_pole_id, odkryte_pole FROM "HRA" hra 
  INNER JOIN "OBLAST" oblast on hra.oblast = oblast.oblast_id
  INNER JOIN "POLE" pole ON pole.oblast = oblast.oblast_id
  WHERE pole.oblast = NEW.oblast AND pole.souradnice_x = NEW.souradnice_x AND pole.souradnice_y = NEW.souradnice_y;
  
  lze_oznacit_minu := "MNOHO_MIN"(vybrane_pole_id, pocet_min);

  IF lze_oznacit_minu = true AND odkryte_pole = false THEN
    pocet_min := pocet_min + 1;
    UPDATE "HRA" hra SET pocet_oznacenych_min = pocet_min WHERE hra.hra_id = moje_hra_id;
  ELSE
    raise exception 'Nelze oznacit minu (pole je jiz odkryto nebo je moc oznacenych min)';
  END IF;  
  
  RETURN NEW;
  
  EXCEPTION
  WHEN no_data_found then
    raise exception 'Zadane pole pro zaminovani nenalezeno';

end
$$;


ALTER FUNCTION public."TRIGGER_OZNAC_MINU"() OWNER TO postgres;

--
-- TOC entry 239 (class 1255 OID 16394)
-- Name: TRIGGER_PROVED_TAH(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TRIGGER_PROVED_TAH"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare pole_detail "POLE"%ROWTYPE;
declare prvni_tah TIMESTAMP;
declare vybrana_hra_id integer;
declare je_mina boolean;
declare je_vyhra boolean;

begin
  SELECT * INTO pole_detail FROM "POLE" pole WHERE pole.oblast = NEW.oblast AND pole.souradnice_x = NEW.souradnice_x AND pole.souradnice_y = NEW.souradnice_y;

  SELECT hra.hra_id, hra.cas_prvni_tah INTO vybrana_hra_id, prvni_tah FROM "HRA" hra 
  INNER JOIN "OBLAST" oblast on hra.oblast = oblast.oblast_id 
  INNER JOIN "POLE" pole ON pole.oblast = oblast.oblast_id  WHERE pole.pole_id = pole_detail.pole_id;
  

  IF prvni_tah IS NULL THEN
    prvni_tah := NEW.cas_tahu;
  END IF;
  
  /* aktualizuj tabulku HRA */
  UPDATE "HRA" hra SET cas_prvni_tah = prvni_tah, cas_posledni_tah = NEW.cas_tahu WHERE hra.hra_id = vybrana_hra_id;
  
  /* otestuj zda neni vybrana mina */
  SELECT "ODKRYTA_MINA"(pole_detail.pole_id) INTO je_mina;
  
  IF je_mina = true THEN
    UPDATE "HRA" hra SET stav = 3 WHERE hra.hra_id = vybrana_hra_id;
    RAISE notice 'Prave byla odkryta mina ID_POLE: %, hra konci neuspesne.', pole_detail.pole_id;
  END IF;
  
  /* odkryju pole a vsechny jeho sousedici pole, pokud nema zadne sousedici miny */
  EXECUTE "ODKRYJ_POLE"(pole_detail.pole_id);
  update "POLE" pole set odkryto = true where pole.pole_id = pole_detail.pole_id;
  
  /* otestuj zda se rovna pocet volnych poli s poctem min pole */
  je_vyhra := "VYHRA"(pole_detail.pole_id);
  IF je_vyhra = true THEN
    EXECUTE "OZNAC_MINY"(pole_detail.oblast);
    UPDATE "HRA" hra SET stav = 2 WHERE hra.hra_id = vybrana_hra_id;
    RAISE NOTICE 'Hra byla uspesne dokoncena. Byly odkryty vsechny miny.';
  END IF;

  RETURN NEW;
end
$$;


ALTER FUNCTION public."TRIGGER_PROVED_TAH"() OWNER TO postgres;

--
-- TOC entry 233 (class 1255 OID 16395)
-- Name: TRIGGER_START_HRY(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TRIGGER_START_HRY"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
  sirka integer;
  vyska integer;
BEGIN
  SELECT obtiznost.sirka, obtiznost.vyska INTO sirka, vyska FROM "OBTIZNOST" obtiznost WHERE obtiznost.obtiznost_id = NEW.obtiznost;

  FOR x IN 0..sirka - 1 LOOP
    FOR y IN 0..vyska - 1 LOOP
      INSERT INTO "POLE"(oblast, souradnice_x, souradnice_y, sousedni_miny, je_mina, odkryto) VALUES (NEW.oblast_id, x, y, 0, false, false);
    END LOOP;
  END LOOP;
  
  INSERT INTO "HRA"(oblast, stav, pocet_oznacenych_min) VALUES (NEW.oblast_id, 1, 0);

  EXECUTE "ZAMINUJ_OBLAST"(NEW.oblast_id, new.obtiznost);
  EXECUTE "SPOCITEJ_OBLAST"(NEW.oblast_id);

  return NEW;
  
EXCEPTION
  WHEN no_data_found then
    raise exception 'Zadana obtiznost nenalezena';
  
END;
$$;


ALTER FUNCTION public."TRIGGER_START_HRY"() OWNER TO postgres;

--
-- TOC entry 242 (class 1255 OID 16396)
-- Name: TRIGGER_VALIDACE_TAHU(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TRIGGER_VALIDACE_TAHU"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare stav_hry integer;
declare oznacene_pole_mina integer;
declare odkryte_pole boolean;
declare pole_detail "POLE"%ROWTYPE;

begin
  SELECT * INTO pole_detail FROM "POLE" pole WHERE pole.oblast = NEW.oblast AND pole.souradnice_x = NEW.souradnice_x AND pole.souradnice_y = NEW.souradnice_y;
  
  /* test zda je hra ve stavu rozehrana */
  SELECT COUNT(hra.hra_id) INTO stav_hry FROM "HRA" hra
  INNER JOIN "OBLAST" oblast on hra.oblast = oblast.oblast_id 
  INNER JOIN "POLE" pole ON pole.oblast = oblast.oblast_id 
  WHERE pole.pole_id = pole_detail.pole_id AND hra.stav = 1;
  
  IF stav_hry = 0 THEN
    raise exception 'Hra je jiz ukoncena, neni mozne provest tah';
  END IF;
  
  /* test zda jiz neni pole zobrazene */
  IF pole_detail.odkryto = true THEN
    raise exception 'Pole je jiz zobrazeno';
  END IF;

  /* test zda neni proveden tah na pole, ktere je oznacene jako zaminovane */
  
  SELECT COUNT(mina.mina_id) INTO oznacene_pole_mina FROM "MINA" mina WHERE mina.oblast = pole_detail.oblast AND mina.souradnice_x = pole_detail.souradnice_x AND mina.souradnice_y = pole_detail.souradnice_y;
  
  IF oznacene_pole_mina != 0 THEN
    raise exception 'Pole je oznacene jako zaminovane, neni mozne provest tah';
  END IF;  

  RETURN NEW;
  
EXCEPTION
	WHEN NO_DATA_FOUND THEN
	raise exception 'Zadane pole nenalezeno';

end
$$;


ALTER FUNCTION public."TRIGGER_VALIDACE_TAHU"() OWNER TO postgres;

--
-- TOC entry 219 (class 1255 OID 16397)
-- Name: VYHRA(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."VYHRA"(param_pole_pole_id integer) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
declare pocet_volnych_poli integer;
declare oblast_id integer;
declare obtiznost_pocet_min integer;

begin
   SELECT oblast.oblast_id, obtiznost.pocet_min INTO oblast_id, obtiznost_pocet_min FROM "POLE" pole 
   INNER JOIN "OBLAST" oblast ON oblast.oblast_id = pole.oblast
   INNER JOIN "OBTIZNOST" obtiznost ON obtiznost.obtiznost_id = oblast.obtiznost WHERE pole.pole_id = param_pole_pole_id;
  
  SELECT COUNT(*) INTO pocet_volnych_poli FROM "POLE" pole WHERE oblast_id = pole.oblast AND pole.odkryto = false;
  
  IF obtiznost_pocet_min = pocet_volnych_poli THEN
    RETURN true;
  ELSE
    RETURN false;
  END IF;

end
$$;


ALTER FUNCTION public."VYHRA"(param_pole_pole_id integer) OWNER TO postgres;

--
-- TOC entry 241 (class 1255 OID 16398)
-- Name: ZAMINUJ_OBLAST(integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."ZAMINUJ_OBLAST"(param_oblast_id integer, param_obtiznost_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$DECLARE vyska integer;
DECLARE sirka integer;
DECLARE pocet_min integer;
  
DECLARE x_gen integer;
DECLARE y_gen integer;
  
DECLARE pocet_min_vlozeno integer;
DECLARE nova_mina integer;
DECLARE je_mina boolean;
 
begin
  
  /* vyberu obtiznost podle vytvorene oblasti */
  SELECT obtiznost.vyska, obtiznost.sirka, obtiznost.pocet_min INTO vyska, sirka, pocet_min FROM "OBTIZNOST" obtiznost 
  WHERE obtiznost.obtiznost_id = param_obtiznost_id;
  
  pocet_min_vlozeno := 0;
  
  WHILE pocet_min_vlozeno < pocet_min LOOP
    x_gen := floor(random()*sirka);
    y_gen := floor(random()*vyska);
/*    x_gen := ROUND(dbms_random.value(0, pocet_radku - 1));
    y_gen := ROUND(dbms_random.value(0, pocet_sloupcu - 1));*/

    BEGIN
      SELECT pole.pole_id, pole.je_mina INTO nova_mina, je_mina FROM public."POLE" pole WHERE pole.oblast = param_oblast_id AND pole.souradnice_x = x_gen AND pole.souradnice_y = y_gen;
      
	  IF (je_mina = false) THEN
	  	UPDATE "POLE" pole SET je_mina = TRUE, sousedni_miny = -1 where pole_id = nova_mina;
      	pocet_min_vlozeno := pocet_min_vlozeno + 1;
      END IF;
	  
    EXCEPTION
      WHEN no_data_found THEN
      RAISE EXCEPTION 'Nenalezena zadna data!';
        
    END;
  END LOOP;
  
  RAISE NOTICE 'Oblast je zaminovana. ID: %', param_oblast_id;

end
$$;


ALTER FUNCTION public."ZAMINUJ_OBLAST"(param_oblast_id integer, param_obtiznost_id integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 196 (class 1259 OID 16399)
-- Name: MINA; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MINA" (
    mina_id integer NOT NULL,
    souradnice_x integer,
    souradnice_y integer,
    oblast integer
);


ALTER TABLE public."MINA" OWNER TO postgres;

--
-- TOC entry 197 (class 1259 OID 16402)
-- Name: OBLAST; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OBLAST" (
    oblast_id integer NOT NULL,
    obtiznost integer
);


ALTER TABLE public."OBLAST" OWNER TO postgres;

--
-- TOC entry 198 (class 1259 OID 16405)
-- Name: POLE; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."POLE" (
    pole_id integer NOT NULL,
    souradnice_x integer NOT NULL,
    souradnice_y integer NOT NULL,
    je_mina boolean NOT NULL,
    sousedni_miny integer NOT NULL,
    oblast integer,
    odkryto boolean NOT NULL
);


ALTER TABLE public."POLE" OWNER TO postgres;

--
-- TOC entry 199 (class 1259 OID 16408)
-- Name: CHYBNE_MINY; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public."CHYBNE_MINY" WITH (security_barrier='false') AS
 SELECT pole.pole_id,
    pole.souradnice_x,
    pole.souradnice_y,
    pole.oblast,
    COALESCE(row_number() OVER (ORDER BY pole.pole_id), (0)::bigint) AS myedmxid
   FROM ((public."POLE" pole
     JOIN public."OBLAST" oblast ON ((pole.oblast = oblast.oblast_id)))
     JOIN public."MINA" mina ON ((mina.oblast = oblast.oblast_id)))
  WHERE ((mina.souradnice_x = pole.souradnice_x) AND (mina.souradnice_y = pole.souradnice_y) AND (pole.je_mina = false))
  ORDER BY oblast.oblast_id;


ALTER TABLE public."CHYBNE_MINY" OWNER TO postgres;

--
-- TOC entry 200 (class 1259 OID 16413)
-- Name: HRA; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."HRA" (
    hra_id integer NOT NULL,
    cas_prvni_tah timestamp with time zone,
    cas_posledni_tah timestamp with time zone,
    pocet_oznacenych_min integer NOT NULL,
    oblast integer,
    stav integer
);


ALTER TABLE public."HRA" OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 16416)
-- Name: HRA_hra_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."HRA_hra_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."HRA_hra_id_seq" OWNER TO postgres;

--
-- TOC entry 2269 (class 0 OID 0)
-- Dependencies: 201
-- Name: HRA_hra_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."HRA_hra_id_seq" OWNED BY public."HRA".hra_id;


--
-- TOC entry 202 (class 1259 OID 16418)
-- Name: MINA_mina_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."MINA_mina_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."MINA_mina_id_seq" OWNER TO postgres;

--
-- TOC entry 2270 (class 0 OID 0)
-- Dependencies: 202
-- Name: MINA_mina_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."MINA_mina_id_seq" OWNED BY public."MINA".mina_id;


--
-- TOC entry 203 (class 1259 OID 16420)
-- Name: OBLAST_TISK; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public."OBLAST_TISK" WITH (security_barrier='false') AS
 SELECT pole.oblast AS pole_id,
    pole.souradnice_y,
    public."RADEK_OBLASTI"(pole.oblast, pole.souradnice_y) AS radek,
    COALESCE(row_number() OVER (ORDER BY pole.oblast, pole.souradnice_y), (0)::bigint) AS myedmxid
   FROM ( SELECT DISTINCT pole_1.souradnice_y,
            pole_1.oblast
           FROM public."POLE" pole_1
          ORDER BY pole_1.oblast, pole_1.souradnice_y) pole;


ALTER TABLE public."OBLAST_TISK" OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 16424)
-- Name: OBLAST_oblast_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."OBLAST_oblast_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."OBLAST_oblast_id_seq" OWNER TO postgres;

--
-- TOC entry 2271 (class 0 OID 0)
-- Dependencies: 204
-- Name: OBLAST_oblast_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."OBLAST_oblast_id_seq" OWNED BY public."OBLAST".oblast_id;


--
-- TOC entry 205 (class 1259 OID 16426)
-- Name: OBTIZNOST; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OBTIZNOST" (
    obtiznost_id integer NOT NULL,
    nazev character varying(30) NOT NULL,
    omezeni integer NOT NULL,
    sirka integer NOT NULL,
    vyska integer NOT NULL,
    pocet_min integer NOT NULL
);


ALTER TABLE public."OBTIZNOST" OWNER TO postgres;

--
-- TOC entry 206 (class 1259 OID 16429)
-- Name: OBTIZNOST_obtiznost_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."OBTIZNOST_obtiznost_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."OBTIZNOST_obtiznost_id_seq" OWNER TO postgres;

--
-- TOC entry 2272 (class 0 OID 0)
-- Dependencies: 206
-- Name: OBTIZNOST_obtiznost_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."OBTIZNOST_obtiznost_id_seq" OWNED BY public."OBTIZNOST".obtiznost_id;


--
-- TOC entry 207 (class 1259 OID 16431)
-- Name: OMEZENI; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OMEZENI" (
    omezeni_id integer NOT NULL,
    sirka_max integer,
    sirka_min integer,
    vyska_max integer,
    vyska_min integer,
    pocet_min_max integer,
    pocet_min_min integer
);


ALTER TABLE public."OMEZENI" OWNER TO postgres;

--
-- TOC entry 208 (class 1259 OID 16434)
-- Name: OMEZENI_omezeni_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."OMEZENI_omezeni_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."OMEZENI_omezeni_id_seq" OWNER TO postgres;

--
-- TOC entry 2273 (class 0 OID 0)
-- Dependencies: 208
-- Name: OMEZENI_omezeni_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."OMEZENI_omezeni_id_seq" OWNED BY public."OMEZENI".omezeni_id;


--
-- TOC entry 209 (class 1259 OID 16436)
-- Name: POLE_pole_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."POLE_pole_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."POLE_pole_id_seq" OWNER TO postgres;

--
-- TOC entry 2274 (class 0 OID 0)
-- Dependencies: 209
-- Name: POLE_pole_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."POLE_pole_id_seq" OWNED BY public."POLE".pole_id;


--
-- TOC entry 210 (class 1259 OID 16438)
-- Name: PORAZENI; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public."PORAZENI" WITH (security_barrier='false') AS
 SELECT hra.hra_id,
    hra.oblast,
    obtiznost.nazev,
    obtiznost.pocet_min,
    obtiznost.sirka,
    obtiznost.vyska,
    date_part('epoch'::text, (hra.cas_posledni_tah - hra.cas_prvni_tah)) AS cas_hry,
    ( SELECT count(*) AS count
           FROM ((public."POLE" pole
             JOIN public."OBLAST" oblast_1 ON ((pole.oblast = oblast_1.oblast_id)))
             JOIN public."MINA" mina ON ((mina.oblast = oblast_1.oblast_id)))
          WHERE ((pole.je_mina = true) AND (pole.oblast = hra.oblast) AND (pole.souradnice_x = mina.souradnice_x) AND (pole.souradnice_y = mina.souradnice_y))) AS pocet_odhalenych_min,
    COALESCE(row_number() OVER (ORDER BY hra.hra_id), (0)::bigint) AS myedmxid
   FROM ((public."HRA" hra
     JOIN public."OBLAST" oblast ON ((hra.oblast = oblast.oblast_id)))
     JOIN public."OBTIZNOST" obtiznost ON ((oblast.obtiznost = obtiznost.obtiznost_id)))
  WHERE (hra.stav = 3)
  ORDER BY hra.cas_prvni_tah;


ALTER TABLE public."PORAZENI" OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 16443)
-- Name: STAV; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."STAV" (
    stav_id integer NOT NULL,
    nazev_stavu character varying(30) NOT NULL
);


ALTER TABLE public."STAV" OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 16446)
-- Name: STAV_stav_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."STAV_stav_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."STAV_stav_id_seq" OWNER TO postgres;

--
-- TOC entry 2275 (class 0 OID 0)
-- Dependencies: 212
-- Name: STAV_stav_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."STAV_stav_id_seq" OWNED BY public."STAV".stav_id;


--
-- TOC entry 213 (class 1259 OID 16448)
-- Name: TAH; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TAH" (
    tah_id integer NOT NULL,
    souradnice_x integer NOT NULL,
    souradnice_y integer NOT NULL,
    cas_tahu timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    oblast integer
);


ALTER TABLE public."TAH" OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 16452)
-- Name: TAH_tah_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."TAH_tah_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."TAH_tah_id_seq" OWNER TO postgres;

--
-- TOC entry 2276 (class 0 OID 0)
-- Dependencies: 214
-- Name: TAH_tah_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."TAH_tah_id_seq" OWNED BY public."TAH".tah_id;


--
-- TOC entry 215 (class 1259 OID 16454)
-- Name: VITEZOVE; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public."VITEZOVE" WITH (security_barrier='false') AS
 SELECT hra.hra_id,
    hra.oblast,
    obtiznost.nazev,
    obtiznost.pocet_min,
    obtiznost.sirka,
    obtiznost.vyska,
    date_part('epoch'::text, (hra.cas_posledni_tah - hra.cas_prvni_tah)) AS cas_hry,
    COALESCE(row_number() OVER (ORDER BY hra.hra_id), (0)::bigint) AS myedmxid
   FROM ((public."HRA" hra
     JOIN public."OBLAST" oblast ON ((hra.oblast = oblast.oblast_id)))
     JOIN public."OBTIZNOST" obtiznost ON ((oblast.obtiznost = obtiznost.obtiznost_id)))
  WHERE (hra.stav = 2)
  ORDER BY hra.cas_prvni_tah;


ALTER TABLE public."VITEZOVE" OWNER TO postgres;

--
-- TOC entry 2100 (class 2604 OID 16459)
-- Name: HRA hra_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HRA" ALTER COLUMN hra_id SET DEFAULT nextval('public."HRA_hra_id_seq"'::regclass);


--
-- TOC entry 2097 (class 2604 OID 16460)
-- Name: MINA mina_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MINA" ALTER COLUMN mina_id SET DEFAULT nextval('public."MINA_mina_id_seq"'::regclass);


--
-- TOC entry 2098 (class 2604 OID 16461)
-- Name: OBLAST oblast_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBLAST" ALTER COLUMN oblast_id SET DEFAULT nextval('public."OBLAST_oblast_id_seq"'::regclass);


--
-- TOC entry 2101 (class 2604 OID 16462)
-- Name: OBTIZNOST obtiznost_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBTIZNOST" ALTER COLUMN obtiznost_id SET DEFAULT nextval('public."OBTIZNOST_obtiznost_id_seq"'::regclass);


--
-- TOC entry 2102 (class 2604 OID 16463)
-- Name: OMEZENI omezeni_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OMEZENI" ALTER COLUMN omezeni_id SET DEFAULT nextval('public."OMEZENI_omezeni_id_seq"'::regclass);


--
-- TOC entry 2099 (class 2604 OID 16464)
-- Name: POLE pole_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."POLE" ALTER COLUMN pole_id SET DEFAULT nextval('public."POLE_pole_id_seq"'::regclass);


--
-- TOC entry 2103 (class 2604 OID 16465)
-- Name: STAV stav_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."STAV" ALTER COLUMN stav_id SET DEFAULT nextval('public."STAV_stav_id_seq"'::regclass);


--
-- TOC entry 2105 (class 2604 OID 16466)
-- Name: TAH tah_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TAH" ALTER COLUMN tah_id SET DEFAULT nextval('public."TAH_tah_id_seq"'::regclass);


--
-- TOC entry 2113 (class 2606 OID 16468)
-- Name: HRA HRA_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HRA"
    ADD CONSTRAINT "HRA_pkey" PRIMARY KEY (hra_id);


--
-- TOC entry 2107 (class 2606 OID 16470)
-- Name: MINA MINA_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MINA"
    ADD CONSTRAINT "MINA_pkey" PRIMARY KEY (mina_id);


--
-- TOC entry 2109 (class 2606 OID 16472)
-- Name: OBLAST OBLAST_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBLAST"
    ADD CONSTRAINT "OBLAST_pkey" PRIMARY KEY (oblast_id);


--
-- TOC entry 2115 (class 2606 OID 16474)
-- Name: OBTIZNOST OBTIZNOST_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBTIZNOST"
    ADD CONSTRAINT "OBTIZNOST_pkey" PRIMARY KEY (obtiznost_id);


--
-- TOC entry 2117 (class 2606 OID 16476)
-- Name: OMEZENI OMEZENI_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OMEZENI"
    ADD CONSTRAINT "OMEZENI_pkey" PRIMARY KEY (omezeni_id);


--
-- TOC entry 2111 (class 2606 OID 16478)
-- Name: POLE POLE_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."POLE"
    ADD CONSTRAINT "POLE_pkey" PRIMARY KEY (pole_id);


--
-- TOC entry 2119 (class 2606 OID 16480)
-- Name: STAV STAV_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."STAV"
    ADD CONSTRAINT "STAV_pkey" PRIMARY KEY (stav_id);


--
-- TOC entry 2121 (class 2606 OID 16482)
-- Name: TAH TAH_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TAH"
    ADD CONSTRAINT "TAH_pkey" PRIMARY KEY (tah_id);


--
-- TOC entry 2129 (class 2620 OID 16483)
-- Name: MINA ODEBER_MINU; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "ODEBER_MINU" BEFORE DELETE ON public."MINA" FOR EACH ROW EXECUTE PROCEDURE public."TRIGGER_ODEBER_MINU"();


--
-- TOC entry 2132 (class 2620 OID 16484)
-- Name: OBTIZNOST OMEZENI_OBTIZNOSTI; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "OMEZENI_OBTIZNOSTI" BEFORE INSERT ON public."OBTIZNOST" FOR EACH ROW EXECUTE PROCEDURE public."TRIGGER_OMEZENI_OBTIZNOSTI"();


--
-- TOC entry 2130 (class 2620 OID 16485)
-- Name: MINA OZNAC_MINU; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "OZNAC_MINU" BEFORE INSERT ON public."MINA" FOR EACH ROW EXECUTE PROCEDURE public."TRIGGER_OZNAC_MINU"();


--
-- TOC entry 2133 (class 2620 OID 16486)
-- Name: TAH PROVED_TAH; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "PROVED_TAH" AFTER INSERT ON public."TAH" FOR EACH ROW EXECUTE PROCEDURE public."TRIGGER_PROVED_TAH"();


--
-- TOC entry 2131 (class 2620 OID 16487)
-- Name: OBLAST START_HRY; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "START_HRY" AFTER INSERT ON public."OBLAST" FOR EACH ROW EXECUTE PROCEDURE public."TRIGGER_START_HRY"();


--
-- TOC entry 2134 (class 2620 OID 16488)
-- Name: TAH VALIDACE_TAHU; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "VALIDACE_TAHU" BEFORE INSERT ON public."TAH" FOR EACH ROW EXECUTE PROCEDURE public."TRIGGER_VALIDACE_TAHU"();


--
-- TOC entry 2125 (class 2606 OID 16489)
-- Name: HRA hraje_v_oblasti; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HRA"
    ADD CONSTRAINT hraje_v_oblasti FOREIGN KEY (oblast) REFERENCES public."OBLAST"(oblast_id);


--
-- TOC entry 2127 (class 2606 OID 16494)
-- Name: OBTIZNOST je_omezena; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBTIZNOST"
    ADD CONSTRAINT je_omezena FOREIGN KEY (omezeni) REFERENCES public."OMEZENI"(omezeni_id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 2128 (class 2606 OID 16499)
-- Name: TAH je_v_oblasti; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TAH"
    ADD CONSTRAINT je_v_oblasti FOREIGN KEY (oblast) REFERENCES public."OBLAST"(oblast_id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 2122 (class 2606 OID 16504)
-- Name: MINA je_v_oblasti; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MINA"
    ADD CONSTRAINT je_v_oblasti FOREIGN KEY (oblast) REFERENCES public."OBLAST"(oblast_id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 2124 (class 2606 OID 16509)
-- Name: POLE je_v_oblasti; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."POLE"
    ADD CONSTRAINT je_v_oblasti FOREIGN KEY (oblast) REFERENCES public."OBLAST"(oblast_id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 2126 (class 2606 OID 16514)
-- Name: HRA je_ve_stavu; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HRA"
    ADD CONSTRAINT je_ve_stavu FOREIGN KEY (stav) REFERENCES public."STAV"(stav_id);


--
-- TOC entry 2123 (class 2606 OID 16519)
-- Name: OBLAST obtiznost; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBLAST"
    ADD CONSTRAINT obtiznost FOREIGN KEY (obtiznost) REFERENCES public."OBTIZNOST"(obtiznost_id) ON UPDATE RESTRICT ON DELETE RESTRICT;


-- Completed on 2018-04-29 18:37:29

--
-- PostgreSQL database dump complete
--

