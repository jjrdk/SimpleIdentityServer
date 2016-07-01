#!/bin/sh

./wait-for-it.sh $DB_ALIAS:$DB_PORT
bash -c "cd SimpleIdentityServer.DataAccess.SqlServer && dotnet ef database update"
nohup dotnet run --project SimpleIdentityServer.Startup/project.json --server.urls=http://*:5000 & 
dotnet run --project SimpleIdentityServer.Startup/project.json --server.urls=https://*:5443