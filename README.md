# Movies Library Backend

This is the backend service for a movie library that allows users to search for movies, rate and comment on them.

Frontend to this app is still under development (Angular) https://github.com/Kamok1/MoviesAngular

# Features

### Authorization System
The project has an authorization system based on JWT (JSON Web Token). 
Users are required to authenticate before accessing certain endpoints. 

### Admin Functions
Administrators have additional privileges over regular users. 
They can add new movies to the library, including images and posters, and update or delete existing movies. 
They can also moderate reviews and user's profiles.

### User Functions
Users can search for movies by id, year, title, genre, director or actor. 
They can also add movies to their favorites list, create their own reviews, delete and edit them.

Users can also update personal information, including their password, email, display name, and profile description.

### Pagination

The API allows users to paginate the provided data. Users can specify a page number and the number of items they want to receive per page.
## API Reference
The API reference for this project can be found in the Swagger documentation, which can be accessed by navigating to `http://localhost:7113/swagger` after starting the project.
