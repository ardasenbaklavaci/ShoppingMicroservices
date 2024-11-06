CREATE DATABASE ProductDb;

USE ProductDb;

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incrementing primary key
    Name NVARCHAR(255) NOT NULL,        -- Product name
    Brand NVARCHAR(255) NOT NULL,       -- Product brand
    Summary NVARCHAR(MAX) NOT NULL,     -- Short summary of the product
    Description NVARCHAR(MAX) NOT NULL,  -- Detailed product description
    Price DECIMAL(18, 2) NOT NULL,      -- Product price with two decimal places
    Category NVARCHAR(255) NOT NULL      -- Category to which the product belongs
);

INSERT INTO Products (Name, Brand, Summary, Description, Price, Category)
VALUES
    ('Wireless Mouse', 'Logitech', 'A wireless mouse for all-day comfort.', 'This wireless mouse features an ergonomic design, making it perfect for all-day use, and its long battery life ensures you won’t have to worry about changing batteries frequently.', 29.99, 'Electronics'),
    ('Bluetooth Headphones', 'Sony', 'High-quality sound with noise cancellation.', 'These Bluetooth headphones deliver superior sound quality with noise cancellation technology, perfect for enjoying your favorite music without distractions.', 99.99, 'Audio'),
    ('4K Monitor', 'Dell', 'Ultra HD monitor with vibrant colors.', 'Experience stunning visuals and vibrant colors with this 4K monitor. Ideal for both gaming and professional work.', 399.99, 'Monitors'),
    ('Smartphone', 'Apple', 'Latest iPhone with advanced features.', 'The latest iPhone offers cutting-edge technology with an impressive camera and long-lasting battery.', 999.99, 'Mobile Phones'),
    ('Gaming Laptop', 'Asus', 'High-performance laptop for gamers.', 'This gaming laptop is equipped with the latest graphics card and high refresh rate display, providing an immersive gaming experience.', 1499.99, 'Computers'),
    ('Smartwatch', 'Samsung', 'Fitness tracking and smart features.', 'Stay connected and monitor your health with this stylish smartwatch that features fitness tracking and notifications.', 249.99, 'Wearables');
