
# Todo Api

A rest api project to do CRUD operations for labels, todoitems and lists via HTTP Verbs (GET, POST, PUT, DELETE, PATCH). It includes functionality for assigning labels to items and lists. It also logs each and every request/response or error if any.

Api uses local SQL database.

DB Setup -

Database is configured and migration is already present. It will be created when the application runs for the first time automatically. Only the connection string in appsettings.json needs to be changed accordingly.
If database is to be updated with changes "Update-database" command will update the database.
Pre Requisite:

Microsoft dot net core 3.1 sdk package/ dot net core runtime 3.1 version should be installed on machine.

How to run application:

Step 1: Clone repo in destination folder: git clone https://github.com/divakarjvsg/ToDoApi.git

Step 2: Go to the project folder and run “dotnet restore” in cmd.

Step 3: Go the repo folder and run “dotnet run” in cmd.

Navigate to http://localhost:28120/Graphql to play with GraphQl UI.


For Swagger:

Navigate to http://localhost:28120/ in a browser to play with the Swagger UI.

Step 1: Click Register and register user with valid values.

Step 2: After registration, click Login and enter your credentials.


Note -

A user has to create todo list first in order to add todo item.
One username can not be registered again.
.Net Core Identity was used for Authentication 
