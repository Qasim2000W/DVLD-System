

The backup file included in this repository is DVLDNew.bak. It is an archive of the project/test database for training/demo purposes only.

The username/password values in the app config are placeholders. Replace them with your actual database connection information when running the application. Generic SQL Server connection example:

Server=YOUR_DB_HOST;Database=DVLDNew;User Id=YOUR_DB_USER;Password=YOUR_DB_PASSWORD;
Bypass the login screen with this data: Username=Qas Password=1234
or To create a quick test user:

INSERT INTO dbo.[users] (username, password, /* other columns as needed */)
VALUES ('testuser','testpassword' /*, other column values */);
