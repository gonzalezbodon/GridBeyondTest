# GridBeyond Test

## :books: Introduction
This project is a simple **ASP.NET MVC web application**. The main goal is to **load, save and analyze data** from a csv file. It is thought to demostrate the knowledge about .NET Framework

![GridBeyond Test Screenshot](https://github.com/gonzalezbodon/GridBeyondTest/blob/master/screenshot.PNG?raw=true)


## :rocket: How to run

* Download **GridBeyondTest** from [GitHub](https://github.com/gonzalezbodon/GridBeyondTest)
* Download and install [Microsoft Visual Studio](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio)
* Create database using the [script included in the project](https://github.com/gonzalezbodon/GridBeyondTest/blob/master/GridBeyondTest/Data/DatabaseCreation.sql)
* Execute  *GridBeyondTest.sln*
* Change the string connection in [appsettings.json](https://github.com/gonzalezbodon/GridBeyondTest/blob/master/GridBeyondTest/appsettings.json) for yours

```
"GridBeyondDB": "Persist Security Info=False;User ID=[USER_LOGIN];Password=[PASSWORD];Initial Catalog=[DATABASE NAME];Data Source=[SERVER]\\[INSTANCE]; MultipleActiveResultSets =True"
```

* Run the project

## :computer: Main code features

* Use of **interfaces** and **dependency injections**.
* **Entity Framework**.
* **Load and save data by packages**
* **Web pages** and **API** implementation.
* Views implemented with **Razor pages** using *HTML5*, *CSS3*, jQUERY, *AJAX* and *Javascript* (object and classes examples).


## :file_folder: Directory Description

* ***wwwroot***: Static files like js, css and images. The main js code is in [site.js](https://github.com/gonzalezbodon/GridBeyondTest/blob/master/GridBeyondTest/wwwroot/js/site.js)
* ***Controllers***: Controllers of the application.
* ***Data***: Code related to the database
* ***Models***: Models of the application with data annotations.
* ***ViewModels***: Models to work with the views and the API.
* ***Views***: Web interface implemented with Razor pages
