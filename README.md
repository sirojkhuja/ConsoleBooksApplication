# Book Console Application

### Required Packages

Installing via Package Manager NuGet

```
    Install-Package ConsoleTables -Version 2.4.2
    Install-Package DotNetEnv -Version 2.2.0
    Install-Package MySql.Data -Version 8.0.27
```

### Requirements for DB connection

1. Create database called `books`
2. Import `./books.sql` to the database
3. Create `./.env` file and add DBNAME, DBUSER, DBUSER values
