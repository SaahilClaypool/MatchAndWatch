#! /bin/sh
/bin/rm -rf /home/saahil/gitProjects/MatchAndWatch/app.db && /bin/rm -rf /home/saahil/gitProjects/MatchAndWatch/Infrastructure/Migrations/* && dotnet ef migrations add Init && dotnet format