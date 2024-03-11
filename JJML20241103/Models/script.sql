-- Crear la base de datos
CREATE DATABASE JML20241103;
GO

-- Usar la base de datos recién creada
USE JML20241103;
GO

-- Crear tabla de Empleados
CREATE TABLE Empleados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Edad INT,
    Cargo VARCHAR(100),
    FechaContratacion DATE
);

-- Crear tabla de Referencias Personales
CREATE TABLE ReferenciasPersonales (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmpleadoId INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Relacion VARCHAR(100),
    Telefono VARCHAR(20),
    FOREIGN KEY (EmpleadoId) REFERENCES Empleados(Id) On Delete  Cascade
);
