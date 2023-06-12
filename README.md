To run the api application:

1)pull the source code to a local folder on your machine

2)open the project folder, or alternatively the ApiMonsta.sln in visual studio code.

3)install all the dependencies in Monsta.csproj

4)make sure to add a new database scheme called 'TaskDB' in your mysql using your mysql database mangement tool,
or alternatively in your xampp or wamp mysql if you have mysql database service on virtual server.

5)make sure to modify the database connection url in AppSettings.json if you have different credentials than the ones provided initialy in the url,
or if you want also to change the database name, then you can modify the connection url.

6)finally you can run the application now in visual studio and consume the api
