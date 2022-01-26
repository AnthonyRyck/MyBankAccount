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

CREATE TABLE typebudget
(idtypebudget int NOT NULL AUTO_INCREMENT,
nomtypebudget VARCHAR(25) NOT NULL,
PRIMARY KEY(idtypebudget));

CREATE TABLE budgets
(idbudget int NOT NULL AUTO_INCREMENT,
nombudget VARCHAR(25) NOT NULL,
description VARCHAR(50) NOT NULL,
idcompte int NOT NULL,
typebudgetid int NOT NULL,
montant DECIMAL(6,2) NULL,
FOREIGN KEY(typebudgetid) REFERENCES typebudget(idtypebudget),
FOREIGN KEY(idcompte) REFERENCES comptes(idcompte),
PRIMARY KEY(idbudget, idcompte));

CREATE TABLE suivicompte
(idcompte int NOT NULL,
idannee int NOT NULL,
idmois int NOT NULL,
datetransaction DATETIME,
typeid int NOT NULL,
nomtransaction VARCHAR(25),
isvalidate bit(1) NOT NULL,
montant DECIMAL(6,2) NOT NULL,
idbudget int,
FOREIGN KEY(idcompte) REFERENCES comptes(idcompte),
FOREIGN KEY(idannee) REFERENCES anneetraitement(annee),
FOREIGN KEY(idmois) REFERENCES moistraitement(mois),
FOREIGN KEY(typeid) REFERENCES typestransaction(idtype),
FOREIGN KEY(idbudget) REFERENCES budgets(idbudget),
PRIMARY KEY(idcompte, idannee, idmois));

CREATE TABLE transactionobligatoire
(idtransac int NOT NULL AUTO_INCREMENT,
idcompte int NOT NULL,
nomtransaction VARCHAR(25) NOT NULL,
montant DECIMAL(6,2) NOT NULL,
typeid int NOT NULL,
jour int NOT NULL,
FOREIGN KEY(idcompte) REFERENCES comptes(idcompte),
FOREIGN KEY(typeid) REFERENCES typestransaction(idtype),
PRIMARY KEY(idtransac));

CREATE TABLE configbank
(idcomptedefault int NOT NULL,
annee int NOT NULL,
mois int NOT NULL,
FOREIGN KEY(idcomptedefault) REFERENCES comptes(idcompte),
FOREIGN KEY(annee) REFERENCES anneetraitement(annee),
FOREIGN KEY(mois) REFERENCES moistraitement(mois),
PRIMARY KEY(idcomptedefault, annee, mois));