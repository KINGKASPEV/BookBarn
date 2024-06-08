BookBarn API


Overview


BookBarn is a comprehensive application for managing a book store, including features such as adding books to carts, checking out, and maintaining a purchase history. This documentation provides detailed instructions on setting up, building, and running the application.


Prerequisites


Before you begin, ensure you have the following installed:

.NET SDK (version 8.0)
SQL Server (or a compatible database)
Entity Framework Core
AutoMapper
Microsoft.Extensions.Logging

Getting Started


Step 1: Clone the Repository
Clone the BookBarn repository from GitHub to your local machine uisng the below snippet.
git clone https://github.com/yourusername/BookBarn.git


Step 2: Configure the Database
Update the appsettings.json file in the BookBarn.API project with your database connection string and JWT Settings.


Step 3: Apply Migrations
Run the following command to apply the Entity Framework Core migrations and create the database schema.
Update-database


Step 4: Build the Application
Build the application to detect any possible eeror.


Step 5: Run the Application
Run the application in your Visual Studio.


Project Structure


=> BookBarn.API: The main entry point for the API.


=> BookBarn.Application: Contains the application logic, including services and DTOs.


=> BookBarn.Domain: Contains the domain entities and interfaces.


=> BookBarn.Infrastructure: Contains the repository implementations and database context.


Key Features

=> Auth Service
1. RegisterAsync: User Registration.
2. LoginAsync : Sign in a user into the system.
3. Logout : Sign out a user .


=> Book Service
1. AddBookAsync: Adds a new book to the database.
2. DeleteBookAsync: Deletes a book by its ID.
3. GetAllBooksAsync: Retrieves all books.
4. GetBookByIdAsync: Retrieves a book by its ID.
5. UpdateBookAsync: Updates the details of an existing book.
6. SearchBooksAsync: Searches for books by title, author, publication year, or genre.


=> Cart Service
1. AddCartAsync: Adds a new cart with books.
2. AddBookToCartAsync: Adds a book to an existing cart.
3. DeleteCartAsync: Deletes a cart.
4. GetAllCartsAsync: Retrieves all carts with included books.
5. GetCartByIdAsync: Retrieves a cart by its ID.
6. RemoveBookFromCartAsync: Removes a book from a cart.
7. UpdateCartAsync: Updates the books in a cart.
8. CheckoutAsync: Processes the checkout and creates a purchase history.


=> Purchase History Service
1. GetPurchaseHistoryAsync: Retrieves the purchase history for a user.


Thought Process
1. Setting Up Repositories: Created generic repository interfaces and implementations for basic CRUD operations.
2. Entities and DTOs: Defined entities for Cart, Book, Checkout, and PurchaseHistory. Created corresponding DTOs for data transfer.
3. Mapping: Configured AutoMapper profiles to map between entities and DTOs.
4. Services: Implemented services for cart management and purchase history retrieval, ensuring all necessary business logic and error handling are in place.
5. Logging: Integrated logging to capture errors and important events for troubleshooting.
6. Database: Utilized Entity Framework Core for database interactions and applied migrations to create the schema.
7. API Endpoints: Exposed API endpoints to perform CRUD operations on Book, carts, handle checkouts, and retrieve purchase histories.


Conclusion


By following this documentation, you should be able to set up, build, and run the BookBarn application successfully. The detailed project structure and thought process provide insights into the development and design decisions made throughout the project. If you encounter any issues, refer to the logs for troubleshooting.
