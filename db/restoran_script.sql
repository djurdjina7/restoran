-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema restoran
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema restoran
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `restoran` DEFAULT CHARACTER SET utf8mb4 ;
USE `restoran` ;

-- -----------------------------------------------------
-- Table `restoran`.`Drzava`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `restoran`.`Drzava` ;

CREATE TABLE IF NOT EXISTS `restoran`.`Drzava` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Naziv` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `restoran`.`Grad`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `restoran`.`Grad` ;

CREATE TABLE IF NOT EXISTS `restoran`.`Grad` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Drzava_Id` INT NOT NULL,
  `Naziv` VARCHAR(45) NULL,
  `PostanskiBroj` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Grad_Drzava1_idx` (`Drzava_Id` ASC),
  CONSTRAINT `fk_Grad_Drzava1`
    FOREIGN KEY (`Drzava_Id`)
    REFERENCES `restoran`.`Drzava` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `restoran`.`Zaposleni`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `restoran`.`Zaposleni` ;

CREATE TABLE IF NOT EXISTS `restoran`.`Zaposleni` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Ime` VARCHAR(45) NULL DEFAULT NULL,
  `Prezime` VARCHAR(45) NULL DEFAULT NULL,
  `BrojTelefona` VARCHAR(45) NULL DEFAULT NULL,
  `MaticniBroj` VARCHAR(13) NULL,
  `Grad_Id` INT NOT NULL,
  `Adresa` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `id_gost_UNIQUE` (`Id` ASC),
  INDEX `fk_Zaposleni_Grad1_idx` (`Grad_Id` ASC),
  CONSTRAINT `fk_Zaposleni_Grad1`
    FOREIGN KEY (`Grad_Id`)
    REFERENCES `restoran`.`Grad` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `restoran`.`Sto`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `restoran`.`Sto` ;

CREATE TABLE IF NOT EXISTS `restoran`.`Sto` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `BrojMjesta` INT NULL,
  `BrojStola` INT NULL,
  `Dostupan` BIT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `restoran`.`Narudzba`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `restoran`.`Narudzba` ;

CREATE TABLE IF NOT EXISTS `restoran`.`Narudzba` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Zaposleni_Id` INT(11) NOT NULL,
  `Sto_Id` INT NOT NULL,
  `VrijemeKreiranja` DATETIME NULL,
  `VrijemeZavrsetka` DATETIME NULL,
  `Cijena` DECIMAL(10,2) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Narudzba_Zaposleni1_idx` (`Zaposleni_Id` ASC),
  INDEX `fk_Narudzba_Sto1_idx` (`Sto_Id` ASC),
  CONSTRAINT `fk_Narudzba_Zaposleni1`
    FOREIGN KEY (`Zaposleni_Id`)
    REFERENCES `restoran`.`Zaposleni` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Narudzba_Sto1`
    FOREIGN KEY (`Sto_Id`)
    REFERENCES `restoran`.`Sto` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `restoran`.`TipMenija`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `restoran`.`TipMenija` ;

CREATE TABLE IF NOT EXISTS `restoran`.`TipMenija` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Naziv` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `restoran`.`Meni`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `restoran`.`Meni` ;

CREATE TABLE IF NOT EXISTS `restoran`.`Meni` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `TipMenija_Id` INT NOT NULL,
  `Naziv` VARCHAR(45) NULL,
  `Cijena` DECIMAL(10,2) NULL,
  `Kolicina` INT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Meni_TipMenija1_idx` (`TipMenija_Id` ASC),
  CONSTRAINT `fk_Meni_TipMenija1`
    FOREIGN KEY (`TipMenija_Id`)
    REFERENCES `restoran`.`TipMenija` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `restoran`.`SpisakZaNarudzbu`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `restoran`.`SpisakZaNarudzbu` ;

CREATE TABLE IF NOT EXISTS `restoran`.`SpisakZaNarudzbu` (
  `Narudzba_Id` INT(11) NOT NULL,
  `Meni_Id` INT NOT NULL,
  `Kolicina` INT NULL,
  `Cijena` DECIMAL(10,2) NULL,
  PRIMARY KEY (`Narudzba_Id`, `Meni_Id`),
  INDEX `fk_Meni_has_Narudzba_Narudzba1_idx` (`Narudzba_Id` ASC),
  INDEX `fk_Meni_has_Narudzba_Meni1_idx` (`Meni_Id` ASC),
  CONSTRAINT `fk_Meni_has_Narudzba_Meni1`
    FOREIGN KEY (`Meni_Id`)
    REFERENCES `restoran`.`Meni` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meni_has_Narudzba_Narudzba1`
    FOREIGN KEY (`Narudzba_Id`)
    REFERENCES `restoran`.`Narudzba` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `restoran`.`Rezervacija`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `restoran`.`Rezervacija` ;

CREATE TABLE IF NOT EXISTS `restoran`.`Rezervacija` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Sto_Id` INT NOT NULL,
  `PodaciGosta` VARCHAR(256) NULL,
  `Datum` DATETIME NULL,
  `VrijemeOd` DATETIME NULL,
  `VrijemeDo` DATETIME NULL,
  `BrojOsoba` INT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Rezervacija_Sto1_idx` (`Sto_Id` ASC),
  CONSTRAINT `fk_Rezervacija_Sto1`
    FOREIGN KEY (`Sto_Id`)
    REFERENCES `restoran`.`Sto` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
