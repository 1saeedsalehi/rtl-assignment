# RTL Assignment
This is a test project for TVMaze scrapper. 
The service scrapes TVMaze for TV show & cast info & persists it. A RESTful API provides a paginated list of shows and cast. 

# Technologies
- Asp.Net Core / .NET 7.0
- EF Core 7.0 + Sqlite
- Polly
- MediatR 
- Docker + Docker Compose
- swagger 

# How to run

## Option 1 : using visual studio

configure project to run multiple startup projects
and choose both **Rtl.TvMaze.SCrapper** and  **Rtl.TvMaze.Presentation** as startup projects.
![](/docs/startup.png)

navigate to [https://localhost:5001/swagger](https://localhost:5001/swagger/index.html)

## Option 2 : using docker

```
docker compose up
```
then 
navigate to [https://localhost:5001/swagger](https://localhost:5001/swagger/index.html)

# Additions

Since this service is for testing purposes, the items related to the real-world solution are not fully observed.
some of my personal preference in a real-world scenarios are listed below:
 - Use `Hangfire`  to manage background jobs 
 - Handle all scenarios when updating the data
 - Use a prdocution-ready database instead of `Sqlite`
 - Integrate with redis for cache 


⚠️ I've provided a simpler solution which stores data on `MemoryCache`
and both scrapper and api runs on same process.
it could be found [here](https://github.com/1saeedsalehi/rtl-assignment/tree/7fbbaf1a8b8b4918df5748620b10aab64da875ea)
