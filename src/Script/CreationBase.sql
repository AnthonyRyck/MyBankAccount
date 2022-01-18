CREATE SCHEMA `banking`;
USE `banking`;

CREATE TABLE typestransaction
(idtype int NOT NULL AUTO_INCREMENT,
nom VARCHAR(25) NOT NULL,
description VARCHAR(50) NOT NULL,
PRIMARY KEY(idtype));

CREATE TABLE anneetraitement
(annee int NOT NULL,
PRIMARY KEY(annee));

CREATE TABLE moistraitement
(mois int NOT NULL,
PRIMARY KEY(mois));

CREATE TABLE comptes
(idcompte int NOT NULL AUTO_INCREMENT,
nomcompte VARCHAR(25) NOT NULL,
description VARCHAR(50) NOT NULL,
PRIMARY KEY(idcompte));

CREATE TABLE budgets
(idbudget int NOT NULL AUTO_INCREMENT,
nombudget VARCHAR(25) NOT NULL,
description VARCHAR(50) NOT NULL,
PRIMARY KEY(idbudget));

CREATE TABLE comptebudget
(idcompte int NOT NULL,
idbudget int NOT NULL,
FOREIGN KEY(idcompte) REFERENCES comptes(idcompte),
FOREIGN KEY(idbudget) REFERENCES budgets(idbudget),
PRIMARY KEY(idcompte, idbudget));

CREATE TABLE suivicompte
(idcompte int NOT NULL,
idannee int NOT NULL,
idmois int NOT NULL,
datetransaction DATETIME,
typeid int NOT NULL,
nomtransaction VARCHAR(25),
isvalidate bit(1) NOT NULL,
montant double NOT NULL,
FOREIGN KEY(idcompte) REFERENCES comptes(idcompte),
FOREIGN KEY(idannee) REFERENCES anneetraitement(annee),
FOREIGN KEY(idmois) REFERENCES moistraitement(mois),
FOREIGN KEY(typeid) REFERENCES typestransaction(idtype),
PRIMARY KEY(idcompte, idannee, idmois));

CREATE TABLE transactionobligatoire
(idtransac int NOT NULL AUTO_INCREMENT,
idcompte int NOT NULL,
nomtransaction VARCHAR(25) NOT NULL,
montant double NOT NULL,
typeid int NOT NULL,
jour int NOT NULL,
FOREIGN KEY(idcompte) REFERENCES comptes(idcompte),
FOREIGN KEY(typeid) REFERENCES typestransaction(idtype),
PRIMARY KEY(idtransac));