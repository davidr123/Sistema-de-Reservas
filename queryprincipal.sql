-- Esquema inicial para Plataforma de Restaurante / Centro de Eventos
-- SQL Server (compatible con SQL Server Express)
-- NOTA: reglas de negocio como "reservas con mínimo 24 horas" deben aplicarse en la capa aplicación.

SET NOCOUNT ON;

-- =========================
-- Tablas de seguridad/usuarios
-- =========================
CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(250) NULL
);

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NULL, -- opcional si solo se usan correos
    Email NVARCHAR(200) NOT NULL UNIQUE,
    FullName NVARCHAR(200) NULL,
    PasswordHash VARBINARY(MAX) NULL, -- almacenar hash seguro (PBKDF2/BCrypt/Argon2), nullable para cuentas sociales
    PasswordSalt VARBINARY(200) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    LastLoginAt DATETIME2 NULL
);

CREATE TABLE UserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Role FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);

-- =========================
-- Clientes (personas que reservan)
-- =========================
CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NULL,
    Phone NVARCHAR(50) NULL,
    Notes NVARCHAR(1000) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE INDEX IX_Customers_Email ON Customers(Email);

-- =========================
-- Estructura de espacios (pisos/areas)
-- =========================
CREATE TABLE Floors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(400) NULL
);

CREATE TABLE Areas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FloorId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(400) NULL,
    Capacity INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Areas_Floors FOREIGN KEY (FloorId) REFERENCES Floors(Id) ON DELETE CASCADE
);

CREATE INDEX IX_Areas_FloorId ON Areas(FloorId);

-- =========================
-- Eventos especiales (bloqueos de disponibilidad)
-- =========================
CREATE TABLE SpecialEvents (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    StartAt DATETIME2 NOT NULL,
    EndAt DATETIME2 NOT NULL,
    AllDay BIT NOT NULL DEFAULT 0,
    BlocksAvailability BIT NOT NULL DEFAULT 1, -- si true bloquea disponibilidad
    CapacityBlocked INT NULL, -- cuántas plazas bloquea (opcional)
    IsPrivate BIT NOT NULL DEFAULT 0,
    CreatedByUserId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_SpecialEvents_CreatedBy FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);

CREATE INDEX IX_SpecialEvents_StartAt ON SpecialEvents(StartAt);
CREATE INDEX IX_SpecialEvents_EndAt ON SpecialEvents(EndAt);

-- =========================
-- Parámetros del sistema (clave-valor)
-- =========================
CREATE TABLE SystemParameters (
    ParamKey NVARCHAR(100) PRIMARY KEY,
    ParamValue NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(500) NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- =========================
-- Reservas
-- =========================
CREATE TABLE Reservations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReservationNumber NVARCHAR(30) NOT NULL UNIQUE,
    CustomerId INT NULL, -- si es usuario registrado
    GuestFullName NVARCHAR(200) NULL, -- datos si es reserva de invitado
    GuestEmail NVARCHAR(200) NULL,
    GuestPhone NVARCHAR(50) NULL,
    StartAt DATETIME2 NOT NULL, -- fecha y hora de inicio de la reserva
    EndAt DATETIME2 NULL,       -- opcional
    AdultCount INT NOT NULL DEFAULT 1,
    ChildCount INT NOT NULL DEFAULT 0,
    TotalPeople AS (AdultCount + ChildCount) PERSISTED,
    PricePerAdult DECIMAL(12,2) NULL, -- valores a almacenar en el momento de la reserva
    PricePerChild DECIMAL(12,2) NULL,
    DiscountAmount DECIMAL(12,2) NULL DEFAULT 0,
    TotalAmount DECIMAL(12,2) NOT NULL DEFAULT 0, -- calculado por la aplicación y almacenado
    Currency NVARCHAR(10) NOT NULL DEFAULT 'USD',
    PromotionCode NVARCHAR(50) NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'PENDING', -- Ej: PENDING, CONFIRMED, CANCELLED, COMPLETED, NO_SHOW
    CreatedByUserId INT NULL, -- si el admin crea la reserva
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Notes NVARCHAR(1000) NULL,
    CONSTRAINT FK_Reservations_Customer FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_Reservations_CreatedBy FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);

CREATE INDEX IX_Reservations_StartAt ON Reservations(StartAt);
CREATE INDEX IX_Reservations_CustomerId ON Reservations(CustomerId);

-- Tabla para asociar qué áreas/pisos quedan ocupados por una reserva
CREATE TABLE ReservationAreas (
    ReservationId INT NOT NULL,
    AreaId INT NOT NULL,
    Notes NVARCHAR(500) NULL,
    PRIMARY KEY (ReservationId, AreaId),
    CONSTRAINT FK_ResAreas_Reservation FOREIGN KEY (ReservationId) REFERENCES Reservations(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ResAreas_Area FOREIGN KEY (AreaId) REFERENCES Areas(Id) ON DELETE CASCADE
);

-- =========================
-- Pagos
-- =========================
CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReservationId INT NOT NULL,
    Amount DECIMAL(12,2) NOT NULL,
    Currency NVARCHAR(10) NOT NULL DEFAULT 'USD',
    PaymentMethod NVARCHAR(50) NULL, -- e.g., 'CARD', 'PAYPAL', 'TRANSFER'
    PaymentGateway NVARCHAR(100) NULL, -- e.g., 'Stripe', 'PayU'
    TransactionId NVARCHAR(200) NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'PENDING', -- PENDING, SUCCESS, FAILED, REFUNDED
    PaidAt DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Payments_Reservation FOREIGN KEY (ReservationId) REFERENCES Reservations(Id) ON DELETE CASCADE
);

CREATE INDEX IX_Payments_ReservationId ON Payments(ReservationId);

-- =========================
-- Notificaciones (cola / historial)
-- =========================
CREATE TABLE Notifications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReservationId INT NULL,
    Type NVARCHAR(50) NOT NULL, -- e.g., 'ReservationCreated', 'ReservationConfirmed'
    Channel NVARCHAR(50) NOT NULL, -- 'Email', 'WhatsApp', 'Telegram'
    Payload NVARCHAR(MAX) NULL, -- JSON con el contenido
    Status NVARCHAR(50) NOT NULL DEFAULT 'PENDING', -- PENDING, SENT, FAILED
    AttemptCount INT NOT NULL DEFAULT 0,
    LastAttemptAt DATETIME2 NULL,
    SentAt DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Notifications_Reservation FOREIGN KEY (ReservationId) REFERENCES Reservations(Id) ON DELETE SET NULL
);

CREATE INDEX IX_Notifications_Status ON Notifications(Status);

-- =========================
-- Galería / Medios
-- =========================
CREATE TABLE Media (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Url NVARCHAR(1000) NOT NULL,
    MediaType NVARCHAR(50) NULL, -- e.g., 'IMAGE', 'VIDEO'
    RelatedEntity NVARCHAR(100) NULL, -- 'Event', 'Reservation', 'GallerySection'
    RelatedEntityId INT NULL,
    AltText NVARCHAR(250) NULL,
    SortOrder INT NULL,
    IsPublic BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE INDEX IX_Media_Related ON Media(RelatedEntity, RelatedEntityId);

-- =========================
-- Mensajes de contacto
-- =========================
CREATE TABLE ContactMessages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(50) NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'NEW', -- NEW, READ, RESPONDED, CLOSED
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE INDEX IX_ContactMessages_Email ON ContactMessages(Email);

-- =========================
-- Auditoría básica
-- =========================
CREATE TABLE AuditLogs (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL,
    Action NVARCHAR(100) NOT NULL,
    Entity NVARCHAR(100) NULL,
    EntityId NVARCHAR(100) NULL,
    Details NVARCHAR(MAX) NULL, -- JSON con detalles
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_AuditLogs_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE SET NULL
);

CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);

-- =========================
-- Tablas lookup (estados)
-- =========================
CREATE TABLE ReservationStatuses (
    Code NVARCHAR(50) PRIMARY KEY,
    Description NVARCHAR(200) NULL
);

CREATE TABLE PaymentStatuses (
    Code NVARCHAR(50) PRIMARY KEY,
    Description NVARCHAR(200) NULL
);

INSERT INTO ReservationStatuses (Code, Description)
VALUES
('PENDING', 'Pendiente de confirmación'),
('CONFIRMED', 'Confirmada'),
('CANCELLED', 'Cancelada'),
('COMPLETED', 'Completada'),
('NO_SHOW', 'No se presentó');

INSERT INTO PaymentStatuses (Code, Description)
VALUES
('PENDING', 'Pago pendiente'),
('SUCCESS', 'Pago exitoso'),
('FAILED', 'Pago fallido'),
('REFUNDED', 'Reintegrado');

-- =========================
-- Datos semilla básicos
-- =========================
INSERT INTO Roles (Name, Description) VALUES ('ADMIN', 'Administrador con todos los permisos');
INSERT INTO Roles (Name, Description) VALUES ('VIEWER', 'Usuario de consulta (solo lectura)');

-- Parámetros clave: precios y anticipación mínima
INSERT INTO SystemParameters (ParamKey, ParamValue, Description)
VALUES
('TARIFF_PRICE_ADULT', '25.00', 'Tarifa por adulto (decimal)'),
('TARIFF_PRICE_CHILD', '12.50', 'Tarifa por niño (decimal)'),
('MINIMUM_RESERVATION_HOURS', '24', 'Anticipación mínima en horas para reservar'),
('DEFAULT_CURRENCY', 'USD', 'Moneda por defecto'),
('OPENING_HOUR_WEEKDAY', '09:00', 'Horario apertura por defecto (HH:mm)'),
('CLOSING_HOUR_WEEKDAY', '23:00', 'Horario cierre por defecto (HH:mm)');

-- =========================
-- Índices sugeridos adicionales (consultas frecuentes)
-- =========================
CREATE INDEX IX_Reservations_Status_StartAt ON Reservations(Status, StartAt);
CREATE INDEX IX_Reservations_ReservationNumber ON Reservations(ReservationNumber);

-- =========================
-- Comentarios / notas
-- =========================
/*
Notas:
- Las reglas temporales (ej. reservar con 24 horas de anticipación) deben validarse en la capa de negocio.
- El cálculo del TotalAmount se hace en la aplicación usando PricePerAdult/PricePerChild + descuentos/ promos.
- Para disponibilidad en tiempo real se recomienda:
    - Calcular slots disponibles por Fecha/Hora consultando Reservations (StartAt/EndAt) + SpecialEvents + Areas.Capacity.
    - Mantener transacciones/locks al confirmar pago para evitar sobreventa (p. ej. Serializable o row locking).
- Para integraciones de pago y SMS/WhatsApp/Telegram, usar tablas Payments y Notifications como historial/cola.
- Para performance en sistemas con muchas reservas, considerar particionado por fecha o índices compuestos adicionales.
*/