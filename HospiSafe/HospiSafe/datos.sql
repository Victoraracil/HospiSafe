
-------------------------------------------------
-- USUARIOS (10)
-------------------------------------------------
INSERT INTO Usuarios
(Nombre, Apellidos, DNI, FechaNacimiento, Telefono, CorreoElectronico, PasswordHash, Rol)
VALUES
('Carlos','Gomez Ruiz','12345678A','1985-03-12','600000001','carlos@clinic.com','hash1',1), -- Admin
('Laura','Martinez Lopez','12345678B','1990-06-22','600000002','laura@clinic.com','hash2',2), -- Personal
('Ana','Sanchez Mora','12345678C','1988-01-15','600000003','ana@clinic.com','hash3',2),
('David','Perez Gil','12345678D','1979-09-03','600000004','david@clinic.com','hash4',2),
('Marta','Navarro Diaz','12345678E','1992-11-30','600000005','marta@clinic.com','hash5',2),
('Luis','Romero Torres','12345678F','1983-04-10','600000006','luis@clinic.com','hash6',3),
('Paula','Hernandez Cruz','12345678G','1995-07-19','600000007','paula@clinic.com','hash7',3),
('Jorge','Iglesias Soto','12345678H','1986-02-05','600000008','jorge@clinic.com','hash8',2),
('Sofia','Vidal Ramos','12345678I','1991-12-01','600000009','sofia@clinic.com','hash9',3),
('Raul','Castro León','12345678J','1984-08-27','600000010','raul@clinic.com','hash10',2);
GO


-------------------------------------------------
-- PACIENTES (10)
-------------------------------------------------
INSERT INTO Pacientes
(Nombre, Apellidos, DNI, FechaNacimiento, Telefono, CorreoElectronico, NumSS)
VALUES
('Juan','Perez Lopez','87654321A','2000-01-01','611000001','juan@gmail.com','111111111111'),
('Maria','Lopez Ruiz','87654321B','1998-02-02','611000002','maria@gmail.com','222222222222'),
('Pedro','Garcia Mora','87654321C','1975-03-03','611000003','pedro@gmail.com','333333333333'),
('Lucia','Fernandez Gil','87654321D','1982-04-04','611000004','lucia@gmail.com','444444444444'),
('Sergio','Martinez Diaz','87654321E','1990-05-05','611000005','sergio@gmail.com','555555555555'),
('Elena','Navarro Torres','87654321F','1995-06-06','611000006','elena@gmail.com','666666666666'),
('Alberto','Romero Cruz','87654321G','1987-07-07','611000007','alberto@gmail.com','777777777777'),
('Nuria','Soto Iglesias','87654321H','2001-08-08','611000008','nuria@gmail.com','888888888888'),
('Victor','Ramos Vidal','87654321I','1993-09-09','611000009','victor@gmail.com','999999999999'),
('Clara','Castro Leon','87654321J','1989-10-10','611000010','clara@gmail.com','101010101010');
GO

-------------------------------------------------
-- CITAS (10)
-- Estado: 0 = Activa, 1 = Cancelada
-------------------------------------------------
INSERT INTO Citas
(Fecha, Estado, IdPaciente, IdUsuario)
VALUES
('2026-01-10 09:00',0,1,2),
('2026-01-10 09:30',0,2,3),
('2026-01-10 10:00',1,3,4),
('2026-01-11 11:00',0,4,5),
('2026-01-11 11:30',0,5,6),
('2026-01-12 12:00',1,6,7),
('2026-01-12 12:30',0,7,8),
('2026-01-13 13:00',0,8,9),
('2026-01-13 13:30',1,9,10),
('2026-01-14 14:00',0,10,2);
GO
