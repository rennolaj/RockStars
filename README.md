# RockStars Rest API
This is a solution for the Team RockStars Backend assesment

## Technologies used:

* ASP.NET Core 6
* Entity Framework Core
* Response caching (for caching)
* AutoMapper
* Docker

## Software Development Best Practices and Design Principles used:

* Clean Code
* CQS
* Response caching
* REST API Naming Conventions

# Compile the Solution
### Via the command line:

```ruby
  > msbuild RockStars.sln
  ```
### Visual Studio:

Open the solution and use the shortcut:
Ctrl+Shift+B

# Running the application:
  ### Visual Studio:
  Press the IISExpress Play button.
  
  ### Docker:
  Run following commands in the Solutions folder:
  ```ruby
  > cd RockStars.API
  > docker build -t rockstars .
  > docker run -it --rm -p 5000:80 --name core_rockstars rockstars
  ```
  
  ### Deploy manually
  Run following commands in the Solutions folder:
  ```ruby
  > cd RockStars.API
  > dotnet publish -c Release -o published
  > dotnet published\RockStars.API.dll
  ```
  
## Available Routes

### Default port: 51044<br />

### GET localhost:{ChosenPort}

- api/artistcollections/{Ids}<br />
- api/songcollections{Query}<br />
- api/artists/{artistId}/songs<br />
- api/artists/{artistId}/songs/{songId}<br />
- api/artists<br />
- api/artists/{artistId}<br />
- api/artists{Query}<br />
					  
### POST localhost:{ChosenPort}<br />

#### Note: Can be used with any of the .json files or objects in the .json file provided artists.json & songs.json as Body.

- api/artistcollections<br />
- api/songcollections<br />
- api/artists/{artistId}/songs<br />
- api/artists<br />

### PUT localhost:{ChosenPort} 

- api/artists/{artistId}/songs/{songId}<br />

### DELETE localhost:{ChosenPort} 

- api/artists/{artistId}<br />
- api/artists/{artistId}/songs/{songId}<br />

### PATCH localhost:{ChosenPort} 

- api/artists/{artistId}/songs/{songId}<br />

#### Example: This example would update the Name of the chosen {songId}
```ruby
  [
    {
        "op":"replace",
        "path":"/Name",
        "value": "Patch Test Name"
    }
  ]
  ```

# Testing the Application

1 - Run the application with the method of your choosing.<br />
2 - Use the routes to add artists OR songs parsed as body. (This can be done copying the provided files as JSON Body):

	- api/artistcollections
		OR
	- api/songcollections

	
3 - Use the routes to get songs OR artists with the specific filters of your choosing: 

	- api/songcollections{Query}
		OR
	- api/artists{Query}

	
4 - Use any of the provided routes to test Creation, Deletion & Update functionalities.	
