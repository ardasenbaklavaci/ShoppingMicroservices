CREATE DATABASE AuthDB;

USE AuthDB;

CREATE TABLE [dbo].[Users] (
    [Id] INT PRIMARY KEY IDENTITY,
    [Username] NVARCHAR(50) NOT NULL,
    [PasswordHash] VARBINARY(256) NOT NULL, -- Store as binary
    [Email] NVARCHAR(100) NOT NULL,
    [Role] NVARCHAR(50) NOT NULL -- e.g., "User", "Admin"
);

DECLARE @Password NVARCHAR(50) = '123123';

-- Insert a new user with hashed password
INSERT INTO [dbo].[Users] (Username, PasswordHash, Email, Role)
VALUES ('testuser', HASHBYTES('SHA2_256', @Password), 'admin@example.com', 'User');
