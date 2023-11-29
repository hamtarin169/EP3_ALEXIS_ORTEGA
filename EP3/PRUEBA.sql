use Hospital;

CREATE TABLE IF NOT EXISTS `Hospital`.`Medico` (
  `idMedico` INT NOT NULL auto_increment,
  `NombreMed` VARCHAR(50) NULL,
  `ApellidoMed` VARCHAR(50) NULL,
  `RunMed` VARCHAR(50) NULL,
  `Eunacom` VARCHAR(5) NULL,
  `NacionalidadMed` VARCHAR(45) NULL,
  `EspecialidadMed` VARCHAR(45) NULL,
  `Especialidad` VARCHAR(45) NULL,
  `Horarios` TIME NULL,
  `TarifaHr` INT NULL,
  PRIMARY KEY (`idMedico`)
) ENGINE = InnoDB;

-- Insertar un médico con valores específicos
INSERT INTO `Hospital`.`Medico` (`NombreMed`, `ApellidoMed`, `RunMed`, `Eunacom`, `NacionalidadMed`, `EspecialidadMed`, `Especialidad`, `Horarios`, `TarifaHr`)
VALUES ('Dr. Alexis', 'Ortega', '177756148', 'SI', 'Chilena', 'Cirujano General', 'Diabetologia', '08:00:00-16:00:00', 100);

-- Insertar otro médico con diferentes valores
INSERT INTO `Hospital`.`Medico` (`NombreMed`, `ApellidoMed`, `RunMed`, `Eunacom`, `NacionalidadMed`, `EspecialidadMed`, `Especialidad`, `Horarios`, `TarifaHr`)
VALUES ('Dra. Danae', 'Paz', '181848715', 'si', 'Peruana', 'Pediatra', 'Pediatría', '09:00:00-17:00:00', 120);

-- Insertar un tercer médico con más valores distintos
INSERT INTO `Hospital`.`Medico` (`NombreMed`, `ApellidoMed`, `RunMed`, `Eunacom`, `NacionalidadMed`, `EspecialidadMed`, `Especialidad`, `Horarios`, `TarifaHr`)
VALUES ('Dra. Maria', 'Biglia', '81245975', 'no', 'Argentina', 'Matrona', 'Matrona', '11:00:00-19:00:00', 150);

CREATE TABLE IF NOT EXISTS `Hospital`.`Paciente` (
  `idPaciente` INT NOT NULL auto_increment,
  `NombrePac` VARCHAR(50) NULL,
  `ApellidoPac` VARCHAR(50) NULL,
  `RunPac` VARCHAR(25) NULL,
  `Nacionalidad` VARCHAR(50) NULL,
  `Visa` VARCHAR(5) NULL,
  `Genero` VARCHAR(10) NULL,
  `SintomasPac` VARCHAR(100) NULL,
  `Medico_idMedico` INT NULL,
  PRIMARY KEY (`idPaciente`),
  INDEX `fk_Paciente_Medico_idx` (`Medico_idMedico` ASC),
  CONSTRAINT `fk_Paciente_Medico`
    FOREIGN KEY (`Medico_idMedico`)
    REFERENCES `Hospital`.`Medico` (`idMedico`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
) ENGINE = InnoDB;

INSERT INTO `Hospital`.`Paciente` (`NombrePac`, `ApellidoPac`, `RunPac`, `Nacionalidad`, `Visa`, `Genero`, `SintomasPac`, `Medico_idMedico`)
VALUES ('marcelo', 'perez', '126589457', 'Chilena', 'no', 'Masculino', 'Dolor estomacal', NULL);

INSERT INTO `Hospital`.`Paciente` (`NombrePac`, `ApellidoPac`, `RunPac`, `Nacionalidad`, `Visa`, `Genero`, `SintomasPac`, `Medico_idMedico`)
VALUES ('pedro', 'condorito', '185693365', 'Chilena', 'no', 'Masculino', 'Consulta Diabetes', NULL);

INSERT INTO `Hospital`.`Paciente` (`NombrePac`, `ApellidoPac`, `RunPac`, `Nacionalidad`, `Visa`, `Genero`, `SintomasPac`, `Medico_idMedico`)
VALUES ('Danae', 'donoso', '278845956', 'Chilena', 'SI', 'Femenino', 'Embarazo', NULL);

CREATE TABLE IF NOT EXISTS `Hospital`.`Reserva` (
  `idReserva` INT NOT NULL auto_increment,
  `Especialidad` VARCHAR(45) NULL,
  `DuaReserva` DATE NULL,
  `Paciente_idPaciente` INT NULL,
  PRIMARY KEY (`idReserva`),
  INDEX `fk_Reserva_Paciente_idx` (`Paciente_idPaciente` ASC),
  CONSTRAINT `fk_Reserva_Paciente`
    FOREIGN KEY (`Paciente_idPaciente`)
    REFERENCES `Hospital`.`Paciente` (`idPaciente`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
) ENGINE = InnoDB;

-- Insertar una reserva sin paciente asignado
INSERT INTO `Hospital`.`Reserva` (`Especialidad`, `DuaReserva`, `Paciente_idPaciente`)
VALUES ('Cardiología', '2023-12-01', NULL);

-- Insertar una reserva sin paciente asignado
INSERT INTO `Hospital`.`Reserva` (`Especialidad`, `DuaReserva`, `Paciente_idPaciente`)
VALUES ('Matrona', '2023-12-01', NULL);

