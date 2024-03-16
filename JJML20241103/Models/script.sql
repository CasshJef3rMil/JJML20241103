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



GO

-- Usar la base de datos recién creada
USE Practica20240305DB;
GO

-- Crear la tabla FacturaVenta
CREATE TABLE FacturaVentas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FechaVenta DATE NOT NULL,
    Correlativo VARCHAR(20) NOT NULL,
	Cliente VARCHAR(100) NOT NULL,
    TotalVenta DECIMAL(10,2) NOT NULL
);
GO

-- Crear la tabla DetFacturaVenta
CREATE TABLE DetFacturaVentas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdFacturaVenta INT NOT NULL,
    Producto VARCHAR(100) NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IdFacturaVenta) REFERENCES FacturaVentas(Id) ON DELETE CASCADE
);
GO