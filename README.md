# JoggingApp
Jogging ap with asp.net 6 core + angular 

1) Clone the repository

   a) How to run backend for full e2e functionality
    
     1) Make sure you are running a local instance of SQL server, the newer the better
     2) Configure ConnectionString section to point to that inscance
     3) Install SMTP4DEV, a fake development SMTP server : https://github.com/rnwood/smtp4dev
     4) To install smtp4dev as a global dotnet tool in CLI run: dotnet tool install -g Rnwood.Smtp4dev
     5) In CLI run: smtp4dev to start the service
     6) Open the .sln file with your favorite IDE and hit the play button
     
   b) How to run frontend
    1) Install node, npm
    2) Position yourself into spa folder
    3) In CLI run: npm install
    4) In CLI run: npm run start
    5) browse localhost:4200

See tests project in the backend project for documentation related to business rules and functionality
