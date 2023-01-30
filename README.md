![Asteroid](https://upload.wikimedia.org/wikipedia/commons/thumb/5/52/Artist%E2%80%99s_impression_of_exiled_asteroid_2004_EW95.jpg/120px-Artist%E2%80%99s_impression_of_exiled_asteroid_2004_EW95.jpg)

_P.H.O, a sample project with some Clean Architecture patterns_

# Potentially Hazardous Object (PHO) API

[![build](https://github.com/paolofulgoni/pho/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/paolofulgoni/pho/actions/workflows/build.yml?query=branch%3Amain)
[![docker](https://github.com/paolofulgoni/pho/actions/workflows/docker.yml/badge.svg)](https://github.com/paolofulgoni/pho/actions/workflows/docker.yml)

This REST API provides a single endpoints to get the top three largest asteroids, if any, with potential risk of impact in the next specified days:

`GET /asteroids?days=7`

```json
[
  {
    "name": "378358 (2007 LD)",
    "diameter": 0.5991597306,
    "velocity": 54127.4382012985,
    "date": "2023-02-05"
  },
  {
    "name": "(2022 UE1)",
    "diameter": 0.31735300639999997,
    "velocity": 55153.0307056723,
    "date": "2023-02-04"
  },
  {
    "name": "511684 (2015 BN509)",
    "diameter": 0.29753812155,
    "velocity": 43502.2461627361,
    "date": "2023-02-04"
  }
]
```

## 🎠 Run

### Before you start

The service uses the [Nasa NeoWs](https://api.nasa.gov/) API to get the information about the asteroids.
You need to [register](https://api.nasa.gov/index.html#apply-for-an-api-key) to get an API key.

Alternatively, you can use the `DEMO_KEY` key, but you'll be limited to 30 requests per IP address per hour.

### Run using Docker

The easiest way to run the service is through Docker.

If you don't have Docker installed on your system, take a look [here](https://docs.docker.com/get-docker/) first.

This command will run a container named `pho`, listening on port 5000 (choose another port number if that one is already in use):

```shell
docker image build -t pho:latest .
docker container run -d -p 5000:80 --name pho -e NasaNeoService__ApiKey=DEMO_KEY pho:latest
```

Alternatively, given that the Docker image is published to Docker Hub (repository [paolofulgoni/pho](https://hub.docker.com/repository/docker/paolofulgoni/pho)), you don't need to build the image from source.

```shell
docker container run -d -p 5000:80 --name pho -e NasaNeoService__ApiKey=DEMO_KEY paolofulgoni/pho:latest
```

You can now call the endpoints with your tool of choice, e.g. [HTTPie](https://httpie.io/cli):

```shell
http "localhost:5000/asteroids?days=7"
```

If you prefer to use the Swagger UI, run the service in `Development` mode and open your browser to http://localhost:5000/swagger

```shell
docker container run -d -p 5000:80 --name pho -e NasaNeoService__ApiKey=DEMO_KEY -e ASPNETCORE_ENVIRONMENT=Development paolofulgoni/pho
```

To stop the service, run this command:

```shell
docker container stop pho
```

When you're done, remove the container and the image:

```shell
docker container rm pho
docker image rm pho
```

### Run using .NET 7 SDK

You can easily run the service from your computer, but you'll have to compile it first. Therefore, you need to:

* Install the [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
* Clone the repository locally

Then use the dotnet CLI to run the service. Make sure you're on the root folder of the project, then type:

```shell
dotnet run --project ./src/Pho.Web
```

This will use the `Development` Hosting environment, therefore you can open a browser to http://localhost:5000/swagger and have fun with the Swagger UI.

Press `CTRL+C` when you're done.

## 🔧 Build and test

Make sure [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) is installed on your dev environment. Then just open the project with your IDE of choice.

If you want to build the project using the .NET CLI, run the following command from the project's root folder:

```shell
dotnet build
```

The project contains some unit and integration tests. You can run them with the following command:

```shell
dotnet test
```

## 📁Project structure

The project is structured in the following way:
* `src/Pho.Web` - The ASP.NET Core Web API project, i.e. the entry point of the service
* `src/Pho.Core` - The core library, containing the business logic and the domain model
* `src/Pho.Infrastructure` - The infrastructure library, containing the implementation of the external HTTP calls to the Nasa API

Tests are in the `test` folder, following the same structure as the source code.

## ☑ Todo

A few things I wanted to do, but didn't have enough time:

* Add a cache layer, since the information returned by the API rarely changes and the third-party Nasa API has Rate Limits
* Add Integration tests, by mocking the third-party Nasa API
* Add retry and circuit breaker policies to external HTTP calls
* Hide the Nasa API key (which is in the querystring) from logs
* Move all NuGet packages versions to the `Directory.Packages.props` file
* Allow days range bigger than 7 days, by making multiple calls to the Nasa API (the API allows only 7 days at a time)

## 🙏 Credits

* [Nasa NeoWs](https://api.nasa.gov/) - Nasa's RESTful web service for near earth Asteroid information
* [Asteroid image](https://commons.wikimedia.org/wiki/File:Artist%E2%80%99s_impression_of_exiled_asteroid_2004_EW95.jpg) - Artist’s impression of exiled asteroid 2004 EW95 [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/deed.en) by ESO/M. Kornmesser
