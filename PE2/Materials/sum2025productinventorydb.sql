USE master
GO

CREATE DATABASE Fa25BearDB
GO

USE Fa25BearDB
GO

-- Bảng người dùng hệ thống
CREATE TABLE BearAccounts (
    AccountID INT PRIMARY KEY,
    Username VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    RoleId INT,
    IsActive BIT DEFAULT 1,
    FullName VARCHAR(255) ,
    Phone VARCHAR(255) ,

);

INSERT INTO BearAccounts (AccountID, Username, Email, Password, RoleId, IsActive) VALUES
(1, 'adminpro', 'admin@system.com', 'admin123', 1, 1),
(2, 'manager1', 'manager@system.com', 'manager123', 2, 1),
(3, 'staff1', 'staff@system.com', 'staff123', 3, 1),
(4, 'member1', 'member1@system.com', 'member123', 4, 1),
(5, 'suspended', 'blocked@system.com', 'nopass', 2, 0);

-- Bảng danh mục sản phẩm
CREATE TABLE BearTypes (
    BearTypesID INT PRIMARY KEY,
    BearTypesName VARCHAR(255) NOT NULL,
    Description VARCHAR(500),
    Origin VARCHAR(500)

);

-- Bảng sản phẩm
CREATE TABLE BearProfiles (
    BearProfilesID INT PRIMARY KEY,
    BearTypesID INT,
    BearProfilesName VARCHAR(255) NOT NULL,
    Charateristics VARCHAR(100),
    BearWeight DECIMAL(10, 2),
    ModifiedDate DATE,
    CONSTRAINT fk_BearProfiles_BearTypes FOREIGN KEY (BearTypesID) REFERENCES BearTypes(BearTypesID) ON DELETE CASCADE
);

-- Dữ liệu mẫu cho danh mục
INSERT INTO BearTypes (BearTypesID, BearTypesName, Description) VALUES
(1, 'Electronics', 'Electronic devices and accessories'),
(2, 'Wearables', 'Smartwatches, fitness bands'),
(3, 'Home Appliances', 'Appliances for home use'),
(4, 'Books', 'Printed and digital books'),
(5, 'Gaming', 'Consoles, accessories and titles');

-- Dữ liệu mẫu cho sản phẩm
INSERT INTO BearProfiles (BearProfilesID, BearTypesID, BearProfilesName, BearWeight, Charateristics, ModifiedDate) VALUES
(1, 1, 'Wireless Earbuds Pro',, 199.99, '100', '2024-01-15'),
(2, 1, 'Smartphone X10', , 999.00, '50', '2024-02-10'),
(3, 2, 'Smartwatch Z3', , 149.99, '75', '2024-03-01'),
(4, 3, 'Air Purifier Pro', , 259.00, '40', '2024-01-05'),
(5, 4, 'Artificial Intelligence 101',, 29.99, '200', '2023-12-20'),
(6, 5, 'NextGen Console V', , 499.00, '30', '2024-02-20'),
(7, 5, 'Wireless Controller 2.0', , 69.99, '150', '2024-01-25'),
(8, 2, 'Fitness Band Plus', , 89.99, '90', '2024-03-10'),
(9, 3, 'Robot Vacuum Cleaner', , 299.00, '25', '2024-04-01'),
(10, 4, 'Data Structures Guidebook', , 45.00, '120', '2024-01-18');
