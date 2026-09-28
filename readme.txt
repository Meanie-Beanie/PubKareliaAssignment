PubKarelia Client

Assignment relies on the infrastructure provided by school and can't be run locally. This is simply just a repository to store progress.

Students and faculty on the other hand can run this app using api key provided to them, in which case the following is guideline for them to run the app: 

# Requirements

- Latest .net version
- Visual Studio
- Api key for the service

# How to start

Project contains no api key and requires user to provide the api key instead. They can simply do this by inserting it into Visual Studio running the following command in CLI:

1. dotnet user-secrets set "api_key" "123-api-456-apikey"


Note: Run it in the project folder, if you are running it from VS straight, you might need to add --project src/PubKarelia.Client in the end.


