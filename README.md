# Candidate Management API

This is a Demo of RESTful API project that provides functionalities to insert or update candidates in a database.

## Project Overview

This API allows you to:
- **Upsert (Insert or Update)** candidate data.
- **Cache** candidate data for improved performance.
- **Handle exceptions** and log them globally.

It follows the repository pattern and uses an in-memory cache to avoid frequent database hits. The application is structured to allow easy updates to both the data layer and the caching layer.

## Technologies

- **C#**
- **.NET 8** (or the latest version you're using)
- **Entity Framework Core**
- **AutoMapper** for object-to-object mapping
- **Moq** for unit testing and mocking
- **XUnit** for unit tests
- **In-memory cache** using `ConcurrentDictionary` for caching



### Steps to Install

1. Clone the repository:

    ```bash
    git clone https://github.com/maharjananil551/CandidateAPI.git
    ```


2.Open project in Visual Studio and build it to restore package

3.Run Migration to setup databse with command
 **Add-Migration InitialCreate_CandidatApi** 
 **Update-Database**


4.Run project will should be able to see Swagger Page where you can test api
 Please request with valid parameter like below
 
 {
  "firstName": "test",
  "lastName": "test",
  "phoneNumber": "test",
  "email": "usertest@example.com",
  "preferredCallTime": "test",
  "linkedInProfile": "https://www.google.com/",
  "gitHubProfile": "https://www.google.com/",
  "comments": "test"
}

## Configuration

The application uses an in-memory cache, and you can adjust the cache expiration time and other settings via the `appsettings.json` file.

### Example `appsettings.json`

```json
{
  "CacheSettings": {
    "DefaultExpirationMinutes": 60
  }
}


