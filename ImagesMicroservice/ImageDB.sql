-- Create the database
CREATE DATABASE ImageDB;
GO

-- Use the database
USE ImageDB;
GO

-- Create the Images table
CREATE TABLE Images (
    Id INT IDENTITY(1,1) PRIMARY KEY,         -- Primary key with auto-increment
    FileName NVARCHAR(255) NOT NULL,          -- Name of the uploaded file
    ContentType NVARCHAR(100) NOT NULL,       -- MIME type of the file
    FilePath NVARCHAR(MAX) NOT NULL,          -- Full path to the file on disk
    UploadedAt DATETIME NOT NULL DEFAULT GETDATE()  -- Timestamp of when the file was uploaded
);
GO
