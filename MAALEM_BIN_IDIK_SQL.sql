CREATE DATABASE MAALEM_BIN_IDIK_DB
USE MAALEM_BIN_IDIK_DB


CREATE TABLE UTILISATEUR(
	ID_utilisateur INT PRIMARY KEY IDENTITY,
	Nom_utilisateur VARCHAR(50) NOT NULL,
	Prenom_utilisateur VARCHAR(50) NOT NULL,
	DateN_utilisateur DATE NOT NULL,
	Genre_utilisateur VARCHAR(5) CONSTRAINT CHECK_GENRE CHECK (Genre_utilisateur in ('Homme','Femme')),
	Tele_utilisateur INT CONSTRAINT CHECK_TELE CHECK (Tele_utilisateur like '[6-7][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	Email_utilisateur VARCHAR(50) NOT NULL,
	Mdp_utilisateur VARCHAR(50) NOT NULL,
	Email_verifier bit,
	Utilisateur_Banni bit
);

CREATE TABLE ARTISAN(
	ID_artisan INT PRIMARY KEY IDENTITY,
	Nom_artisan VARCHAR(50) NOT NULL,
	Prenom_artisan VARCHAR(50) NOT NULL,
	DateN_artisan DATE NOT NULL,
	Genre_artisan VARCHAR(5) CONSTRAINT CHECK_GENRE2 CHECK (Genre_artisan in ('Homme','Femme')),
	Tele_artisan INT CONSTRAINT CHECK_TELE2 CHECK (Tele_artisan like '[6-7][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	Email_artisan VARCHAR(50) NOT NULL,
	Mdp_artisan VARCHAR(50) NOT NULL,
	Email_verifier bit,
	Artisan_Banni bit
);

CREATE TABLE METIER(
	ID_metier INT PRIMARY KEY IDENTITY,
	Nom_metier VARCHAR(50) NOT NULL,
);
INSERT INTO METIER VALUES ('Plombier');
INSERT INTO METIER VALUES ('Menuisier');
INSERT INTO METIER VALUES ('Electricien');
INSERT INTO METIER VALUES ('Constructeur');
INSERT INTO METIER VALUES ('Peintre');
INSERT INTO METIER VALUES ('Forgeron');
INSERT INTO METIER VALUES ('Tapicier');
INSERT INTO METIER VALUES ('Platrier');

CREATE TABLE SERVICE(
	ID_service INT PRIMARY KEY IDENTITY,
	Titre_service VARCHAR(50) NOT NULL,
	Description_service VARCHAR(1000) NOT NULL,
	Ville_service VARCHAR(50) NOT NULL,
	ID_metier INT NOT NULL,
	ID_artisan INT NOT NULL,
	CONSTRAINT FK1_Table_Services FOREIGN KEY (ID_metier) REFERENCES METIER(ID_metier),
	CONSTRAINT FK2_Table_Services FOREIGN KEY (ID_artisan) REFERENCES ARTISAN(ID_artisan)
);

CREATE TABLE COMMANDE_SERVICE(
	ID_commande INT PRIMARY KEY IDENTITY,
	DateDePrise DATE NOT NULL,
	ID_service INT NOT NULL,
	ID_utilisateur INT NOT NULL,
	Prix_service Money DEFAULT 0,
	CONSTRAINT FK1_Table_Commande FOREIGN KEY (ID_service) REFERENCES SERVICE(ID_service),
	CONSTRAINT FK2_Table_Commande FOREIGN KEY (ID_utilisateur) REFERENCES UTILISATEUR(ID_utilisateur),
);
