# RTL Assignment
This is a test project for TVMaze scrapper. 
The service scrapes TVMaze for TV show & cast info & persists it. A RESTful API provides a paginated list of shows and cast. 

# Used technologies
- Asp.Net Core / .NET 7.0
- EF Core 7.0 + Sqlite
- Polly
- MediatR 
- Docker 
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

# Trade-offs and expansion points

Since this service is for testing purposes, the items related to the real-world solution are not fully observed.
My personal preference in a real-world service is to use `Hangfire` to manage background jobs and handle exceptions in database updates.


⚠️ I've provided a simpler solution which stores data on `MemoryCache`
and both scrapper and api runs on same process.
it could be found [here](https://github.com/1saeedsalehi/rtl-assignment/tree/7fbbaf1a8b8b4918df5748620b10aab64da875ea)
