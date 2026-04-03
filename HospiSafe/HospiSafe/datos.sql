------------------------------------------------
-- 1) USUARIOS
------------------------------------------------

DELETE FROM HospiSafe_BD.dbo.Usuarios;
DBCC CHECKIDENT ('HospiSafe_BD.dbo.Usuarios', RESEED, 0);

SET IDENTITY_INSERT HospiSafe_BD.dbo.Usuarios ON;
GO

INSERT INTO HospiSafe_BD.dbo.Usuarios
(IdUsuario, Nombre, Apellidos, DNI, FechaNacimiento, Telefono, CorreoElectronico, PasswordHash, Rol)
VALUES
(1,'Ana','Martínez López','12345678A','1985-03-10','600111001','ana.martinez@clinica.com','8F42C009B047545CC45D07F2E945C9955E2AB2694B42CD2FF7CA3A0CE37EEC1160904E32BEA5F11B8BC109B230CFB943',0),
(2,'Luis','Gómez Pérez','22345678B','1990-07-22','600111002','luis.gomez@clinica.com','803D9B74F56F9D58EBE59B0823D0464906FC62D4646F9C9AB266935763018890FFF1BCA3D2128A737B4452973D8D69EC',2),
(3,'Marta','Ruiz Torres','32345678C','1982-11-05','600111003','marta.ruiz@clinica.com','82CB6A3D3E550B236BD6C50F1A27902552888442EFF3390189897FCF73FCEFABFA0700885E8095B0D0331B9944610E66',2),
(4,'Carlos','Sánchez Ríos','42345678D','1978-01-18','600111004','carlos.sanchez@clinica.com','2C9A80E333D266B89DA7335502C7550030CE91CB0132922367379D06F37E5AF29E2CFDFB515D52A7983DE7D122A88BA',1),
(5,'Laura','Fernández Mora','52345678E','1995-09-30','600111005','laura.fernandez@clinica.com','12AA84F2248DC54E0EAA0A247FE13179284453CB79BADE58A8C08D5F1306ED290DF1BD70BB7CD126678A5A9440378384',2),
(6,'Pedro','Navarro Gil','62345678F','1988-06-14','600111006','pedro.navarro@clinica.com','6AB4F303F913300213A8344EF479537D2EA80322AE6C9FF1B5CC76897BE810228532643DC447C3AE5D2255AD7BE6CF9B',2),
(7,'Isabel','Romero Díaz','72345678G','1975-02-28','600111007','isabel.romero@clinica.com','F0ABC5E961FB58FEB21626D76EC226BF8981273A78B811767F884F75D3C6309065D7DDE8BEA970E40B3BB504B9911F6',1),
(8,'Javier','Molina Costa','82345678H','1992-12-12','600111008','javier.molina@clinica.com','9D70FFCBAC6605FA42E4766DFE6489CBC9F353CA34C59A12C015A33AFF258159BB7D7BAD3E60993A9774F2CE8A7B6C94A',2),
(9,'Sara','Blanco Ruiz','92345678I','1984-08-08','600111009','sara.blanco@clinica.com','66F2148E178B9ED79FE72B5FFF6E4EB4C7151EC5F8B519FB3143C0AA242343DA559DFE7584F50A158DE2C1A7300090E',2),
(10,'Miguel','Torres Ramos','01345678J','1980-04-20','600111010','miguel.torres@clinica.com','CD31EC8B6F7D41BB451512C7DE1A4166E1A0B2B5E992D464E87BBFF1CA06CA67C1D412C198A64A0D152C2946606BCD70',1),
(11,'Admin','Admin','99999999Z','1990-01-01','600000000','admin@clinica.com','CD31EC8B6F7D41BB451512C7DE1A4166E1A0B2B5E992D464E87BBFF1CA06CA67C1D412C198A64A0D152C2946606BCD70',1),
(12,'Admin','Admin','99999999Z','1990-01-01','600000000','admin','CD31EC8B6F7D41BB451512C7DE1A4166E1A0B2B5E992D464E87BBFF1CA06CA67C1D412C198A64A0D152C2946606BCD70',1);
GO

SET IDENTITY_INSERT HospiSafe_BD.dbo.Usuarios OFF;
GO


------------------------------------------------
-- 2) PACIENTES
------------------------------------------------
SET IDENTITY_INSERT HospiSafe_BD.dbo.Pacientes ON;
GO

INSERT INTO HospiSafe_BD.dbo.Pacientes
(IdPaciente, Nombre, Apellidos, DNI, FechaNacimiento, Telefono, CorreoElectronico, NumSS)
VALUES
(1,'Raúl','García López','11111111A','2000-05-10','611000001','raul.garcia@mail.com','123456789012'),
(2,'Paula','Moreno Ruiz','22222222B','1998-03-15','611000002','paula.moreno@mail.com','223456789012'),
(3,'David','Pérez Martín','33333333C','1987-07-22','611000003','david.perez@mail.com','323456789012'),
(4,'Lucía','Santos Gil','44444444D','1979-11-05','611000004','lucia.santos@mail.com','423456789012'),
(5,'Jorge','Ramírez Torres','55555555E','1992-01-30','611000005','jorge.ramirez@mail.com','523456789012'),
(6,'Carmen','Vega Muñoz','66666666F','1985-06-14','611000006','carmen.vega@mail.com','623456789012'),
(7,'Antonio','López Rivas','77777777G','1973-02-28','611000007','antonio.lopez@mail.com','723456789012'),
(8,'Beatriz','Navarro Costa','88888888H','1996-12-12','611000008','beatriz.navarro@mail.com','823456789012'),
(9,'Mario','Blanco Ruiz','99999999I','1984-08-08','611000009','mario.blanco@mail.com','923456789012'),
(10,'Sofía','Torres Ramos','00000000J','2001-04-20','611000010','sofia.torres@mail.com','023456789012');

SET IDENTITY_INSERT HospiSafe_BD.dbo.Pacientes OFF;
GO


------------------------------------------------
-- 3) CITAS
------------------------------------------------
SET IDENTITY_INSERT HospiSafe_BD.dbo.Citas ON;
GO

INSERT INTO HospiSafe_BD.dbo.Citas
(IdCita, Fecha, Estado, IdPaciente, IdUsuario)
VALUES
(1,'2025-02-10 09:00',0,1,2),
(2,'2025-02-11 10:00',0,2,3),
(3,'2025-02-12 11:00',0,3,4),
(4,'2025-02-13 12:00',1,4,5),
(5,'2025-02-14 09:30',0,5,6),
(6,'2025-02-15 10:30',0,6,7),
(7,'2025-02-16 11:30',1,7,8),
(8,'2025-02-17 12:30',0,8,9),
(9,'2025-02-18 08:45',0,9,10),
(10,'2025-02-19 09:15',1,10,1);

SET IDENTITY_INSERT HospiSafe_BD.dbo.Citas OFF;
GO


------------------------------------------------
-- 4) PRUEBAS
------------------------------------------------
SET IDENTITY_INSERT HospiSafe_BD.dbo.Pruebas ON;
GO

INSERT INTO HospiSafe_BD.dbo.Pruebas
(IdPrueba, Fecha, TipoAnalisis, Estado, Resultados, IdPaciente, IdUsuario)
VALUES
(1,'2025-01-10','Analítica general',0,NULL,1,2),
(2,'2025-01-12','Análisis de sangre',1,'Glucosa normal',2,3),
(3,'2025-01-15','Radiografía tórax',0,NULL,3,4),
(4,'2025-01-18','Resonancia magnética',1,'Sin anomalías',4,5),
(5,'2025-01-20','Electrocardiograma',1,'Ritmo normal',5,6),
(6,'2025-01-22','Prueba alergias',0,NULL,6,7),
(7,'2025-01-25','PCR COVID',1,'Negativo',7,8),
(8,'2025-01-27','Analítica hormonal',0,NULL,8,9),
(9,'2025-01-30','Prueba orina',1,'Valores normales',9,10),
(10,'2025-02-02','Ecografía abdominal',0,NULL,10,1);

SET IDENTITY_INSERT HospiSafe_BD.dbo.Pruebas OFF;
GO
