-- Create the CartDB database
CREATE DATABASE CartDB;
GO

-- Use the CartDB database
USE CartDB;
GO

-- Create CartItems table to store cart data
CREATE TABLE CartItems (
    Id INT PRIMARY KEY IDENTITY(1,1),      -- Unique identifier for each cart item
    ProductId VARCHAR(50) NOT NULL,        -- ID of the product added to the cart
    UserId VARCHAR(50) NOT NULL,           -- ID of the user who owns the cart item
    Quantity INT NOT NULL,                 -- Quantity of the product in the cart
    DateAdded DATETIME DEFAULT GETDATE()   -- Timestamp when the item was added to the cart
);
