# Umbraco Developer Homework

This is my submission for the Umbraco Developer Homework. The solution assumes a running instance of 
a SQL Server database, as well as Visual Studio. 

## Instructions

1. Clone the repository
2. Open the solution in Visual studio and replace the connection string in the `appsettings.json` file with your own connection string.
eg. `Server=MY-DATABASE\\SQLEXPRESS;`
3. To migrate the database, open the Package Manager Console and run the following commands:
```bash
>Add-Migration InitialCreate -Project AcmeCorp.Data -StartupProject AcmeCorp.Web
>Update-Database -Project AcmeCorp.Data -StartupProject AcmeCorp.Web
```
4. Optionally, you can pre-seed the database by running the accomapnying `seeder.py` python script in the root of the solution. 
To do this, activate the virtual environment and run the script by the following commands:
```bash
.\venv\Scripts\Activate.ps1 \\ for PowerShell
.\venv\Scripts\Activate.bat \\ for CMD

>python seeder.py
``` 

The solution can now be run. The seeder script uses serial numbers located in a text file the root of the solution. 
