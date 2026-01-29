PASO A PASO – ENCENDER LA BASE DE DATOS (WPF + EF + SQL SERVER EN DOCKER)

1. ARRANCAR DOCKER

- Abrir Docker Desktop
- Esperar a que esté “Running”

2. ARRANCAR EL CONTENEDOR DE SQL SERVER
Desde la carpeta donde está el docker-compose.yml:

docker-compose up -d

Comprobar que está activo:

docker ps

Debe salir:
sqlserver\_wpf Up ...

3. CONECTAR CON SQL SERVER MANAGEMENT STUDIO
Abrir SSMS y usar estos datos:

Servidor: localhost,1433
Autenticación: SQL Server
Usuario: sa
Contraseña: (la del docker, ej. SqlServer!2024)
Opciones > Seguridad > Confiar en el certificado ✔️

4. COMPROBAR QUE SQL SERVER FUNCIONA
En SSMS deben verse:

- system databases
- (todavía puede no estar la BD del proyecto)

5. ABRIR EL PROYECTO WPF EN VISUAL STUDIO
6. COMPROBAR CONNECTION STRING
En DbContext debe ser:

Server=localhost,1433;
Database=WpfDockerDb;
User Id=sa;
Password=SqlServer!2024;
TrustServerCertificate=True;

7. EJECUTAR MIGRACIONES
Abrir la Consola del Administrador de Paquetes:

Add-Migration InitialCreate
Update-Database

8. VERIFICAR LA BASE DE DATOS
Volver a SSMS → Refresh
Debe aparecer:
WpfDockerDb
con sus tablas