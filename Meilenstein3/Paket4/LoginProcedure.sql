CREATE DEFINER=`denis`@`localhost` PROCEDURE `LoginProcedure`(
	IN `Lname` VARCHAR(30)
)
    READS SQL DATA
BEGIN
DECLARE drole ENUM('Gast','Student','Mitarbeiter');
DECLARE	dloginname VARCHAR(30);
DECLARE	dsalt VARCHAR(32);
DECLARE	dhash VARCHAR(24);


IF (select count(*) from Student as st 
	join `FH Angehörige` as fh on fh.ID = st.`fkFHange`
	join `Benutzer` as b on b.Nummer = fh.`fkBenutzer`
	where b.Nutzername = Lname) = 1 THEN
	set drole = 'Student';
ELSEIF (select count(*) from Mitarbeiter as ma 
	join `FH Angehörige` as fh on fh.ID = ma.`fkFHange`
	join `Benutzer` as b on b.Nummer = fh.`fkBenutzer`
	where b.Nutzername = Lname) = 1 THEN
	set drole = 'Mitarbeiter';
ELSE
	set drole = 'Gast';
END IF;


select be.Nutzername, be.Salt, be.`Hash`
into dloginname, dsalt, dhash
			from Benutzer as be
			where be.Nutzername = Lname limit 1;

select drole as role, dloginname as Nutzername, dsalt as salt, dhash as `hash`;

END