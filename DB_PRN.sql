USE master
GO
CREATE DATABASE DBG1FOOD
GO
USE DBG1FOOD
GO

CREATE TABLE [Role] (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255) NOT NULL DEFAULT 'no name',
  description NVARCHAR(MAX) DEFAULT ''
);

CREATE TABLE [Status] (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255) NOT NULL DEFAULT 'no name',
  description NVARCHAR(MAX) DEFAULT ''
);

CREATE TABLE [Unit] (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255) NOT NULL,
  description NVARCHAR(MAX)
);

CREATE TABLE Warehouse_Ingredient (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255)
);

CREATE TABLE Warehouse_Tableware (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255)
);

CREATE TABLE Ingredient (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255) NOT NULL,
  description NVARCHAR(MAX),
  unitId UNIQUEIDENTIFIER
);

CREATE TABLE Tableware (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255) NOT NULL,
  description NVARCHAR(MAX),
  unitId UNIQUEIDENTIFIER
);

CREATE TABLE Categogy (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255) NOT NULL DEFAULT 'no name',
  description NVARCHAR(MAX) DEFAULT ''
);

CREATE TABLE Product (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255) NOT NULL DEFAULT 'no name',
  description NVARCHAR(MAX) DEFAULT '',
  price MONEY NOT NULL,
  salePercent INT,
  image NVARCHAR(255),
  statusId UNIQUEIDENTIFIER,
  categogyId UNIQUEIDENTIFIER
);

CREATE TABLE Cart (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  quantity INT NOT NULL,
  productId UNIQUEIDENTIFIER,
  accountId UNIQUEIDENTIFIER
);

CREATE TABLE Voucher (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  quantity INT NOT NULL,
  salePercent INT NOT NULL,
  description NVARCHAR(MAX),
  statusId UNIQUEIDENTIFIER
);

CREATE TABLE [Order] (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  [date] DATE NOT NULL,
  note NVARCHAR(MAX),
  statusId UNIQUEIDENTIFIER,
  userId UNIQUEIDENTIFIER,
  scheduleId UNIQUEIDENTIFIER,
  voucherId UNIQUEIDENTIFIER
);

CREATE TABLE [User] (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255) NOT NULL DEFAULT 'no name',
  addressDetail NVARCHAR(255) NOT NULL DEFAULT '',
  phone NVARCHAR(255) NOT NULL DEFAULT '',
  defaultUser BIT,
  accountId UNIQUEIDENTIFIER
);

CREATE TABLE Account (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  email NVARCHAR(255) NOT NULL DEFAULT '',
  encryptedPassword NVARCHAR(255) NOT NULL DEFAULT '',
  roleId UNIQUEIDENTIFIER,
  statusId UNIQUEIDENTIFIER
);

CREATE TABLE Schedule (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  [date] DATE NOT NULL,
  note NVARCHAR(MAX),
  shiftId UNIQUEIDENTIFIER
);

CREATE TABLE Schedule_Detail (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  accountId UNIQUEIDENTIFIER,
  scheduleId UNIQUEIDENTIFIER
);

CREATE TABLE Comment (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  accountId UNIQUEIDENTIFIER,
  productId UNIQUEIDENTIFIER,
  parentCommentId UNIQUEIDENTIFIER DEFAULT NULL
);

CREATE TABLE Recipe (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  quantity INT NOT NULL,
  productId UNIQUEIDENTIFIER,
  ingredientId UNIQUEIDENTIFIER
);

CREATE TABLE OrderDetail (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  price MONEY NOT NULL,
  salePercent INT,
  quantity INT NOT NULL,
  note NVARCHAR(MAX),
  ingredientExportId UNIQUEIDENTIFIER,
  tablewareExportId UNIQUEIDENTIFIER,
  productId UNIQUEIDENTIFIER,
  orderId UNIQUEIDENTIFIER
);

CREATE TABLE Ingredient_Import (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  [date] DATE NOT NULL,
  quantity INT NOT NULL,
  price MONEY NOT NULL,
  warehouse UNIQUEIDENTIFIER,
  ingredientId UNIQUEIDENTIFIER,
  accountId UNIQUEIDENTIFIER
);

CREATE TABLE Ingredient_Export (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  [date] DATE NOT NULL,
  quantity INT NOT NULL,
  warehouseId UNIQUEIDENTIFIER
);

CREATE TABLE Tableware_Import (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  [date] DATE NOT NULL,
  quantity INT NOT NULL,
  price MONEY NOT NULL,
  warehouseId UNIQUEIDENTIFIER,
  tablewareId UNIQUEIDENTIFIER,
  accountId UNIQUEIDENTIFIER
);

CREATE TABLE Tableware_Export (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  [date] DATE NOT NULL,
  quantity INT NOT NULL,
  warehouseId UNIQUEIDENTIFIER
);

CREATE TABLE Destruction (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  [date] DATE NOT NULL,
  quantity INT NOT NULL,
  warehouseId UNIQUEIDENTIFIER,
  accountId UNIQUEIDENTIFIER
);

CREATE TABLE Consume (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  quantity INT NOT NULL,
  productId UNIQUEIDENTIFIER,
  tablewareId UNIQUEIDENTIFIER
);

CREATE TABLE [Shift] (
  id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  name NVARCHAR(255),
  startTime TIME NOT NULL,
  endTime TIME NOT NULL
);


ALTER TABLE [Account] ADD FOREIGN KEY ([roleId]) REFERENCES [Role] ([id])
GO

ALTER TABLE [Account] ADD FOREIGN KEY ([statusId]) REFERENCES [Status] ([id])
GO

ALTER TABLE [User] ADD FOREIGN KEY ([accountId]) REFERENCES [Account] ([id])
GO

ALTER TABLE [Product] ADD FOREIGN KEY ([statusId]) REFERENCES [Status] ([id])
GO

ALTER TABLE [Product] ADD FOREIGN KEY ([categogyId]) REFERENCES [Categogy] ([id])
GO

ALTER TABLE [Cart] ADD FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO

ALTER TABLE [Cart] ADD FOREIGN KEY ([accountId]) REFERENCES [Account] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([statusId]) REFERENCES [Status] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([scheduleId]) REFERENCES [Schedule] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([voucherId]) REFERENCES [Voucher] ([id])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([ingredientExportId]) REFERENCES [Ingredient_Export] ([id])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([tablewareExportId]) REFERENCES [Tableware_Export] ([id])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([orderId]) REFERENCES [Order] ([id])
GO

ALTER TABLE [Voucher] ADD FOREIGN KEY ([statusId]) REFERENCES [Status] ([id])
GO

ALTER TABLE [Ingredient] ADD FOREIGN KEY ([unitId]) REFERENCES [Unit] ([id])
GO

ALTER TABLE [Recipe] ADD FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO

ALTER TABLE [Recipe] ADD FOREIGN KEY ([ingredientId]) REFERENCES [Ingredient] ([id])
GO

ALTER TABLE [Ingredient_Import] ADD FOREIGN KEY ([warehouse]) REFERENCES [Warehouse_Ingredient] ([id])
GO

ALTER TABLE [Ingredient_Import] ADD FOREIGN KEY ([ingredientId]) REFERENCES [Ingredient] ([id])
GO

ALTER TABLE [Ingredient_Import] ADD FOREIGN KEY ([accountId]) REFERENCES [Account] ([id])
GO

ALTER TABLE [Ingredient_Export] ADD FOREIGN KEY ([warehouseId]) REFERENCES [Warehouse_Ingredient] ([id])
GO

ALTER TABLE [Tableware] ADD FOREIGN KEY ([unitId]) REFERENCES [Unit] ([id])
GO

ALTER TABLE [Consume] ADD FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO

ALTER TABLE [Consume] ADD FOREIGN KEY ([tablewareId]) REFERENCES [Tableware] ([id])
GO

ALTER TABLE [Tableware_Import] ADD FOREIGN KEY ([warehouseId]) REFERENCES [Warehouse_Tableware] ([id])
GO

ALTER TABLE [Tableware_Import] ADD FOREIGN KEY ([tablewareId]) REFERENCES [Tableware] ([id])
GO

ALTER TABLE [Tableware_Import] ADD FOREIGN KEY ([accountId]) REFERENCES [Account] ([id])
GO

ALTER TABLE [Tableware_Export] ADD FOREIGN KEY ([warehouseId]) REFERENCES [Warehouse_Tableware] ([id])
GO

ALTER TABLE [Destruction] ADD FOREIGN KEY ([warehouseId]) REFERENCES [Warehouse_Ingredient] ([id])
GO

ALTER TABLE [Destruction] ADD FOREIGN KEY ([accountId]) REFERENCES [Account] ([id])
GO

ALTER TABLE [Schedule] ADD FOREIGN KEY ([shiftId]) REFERENCES [Shift] ([id])
GO

ALTER TABLE [Schedule_Detail] ADD FOREIGN KEY ([accountId]) REFERENCES [Account] ([id])
GO

ALTER TABLE [Schedule_Detail] ADD FOREIGN KEY ([scheduleId]) REFERENCES [Schedule] ([id])
GO

ALTER TABLE [Comment] ADD FOREIGN KEY ([accountId]) REFERENCES [Account] ([id])
GO

ALTER TABLE [Comment] ADD FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO

ALTER TABLE [Comment] ADD FOREIGN KEY ([parentCommentId]) REFERENCES [Comment] ([id])
GO

INSERT INTO [Role] (id, name, description)
VALUES 
  (NEWID(), 'Admin', ''),
  (NEWID(), 'User', ''),
  (NEWID(), 'Staff', ''),
  (NEWID(), 'Cashier', ''),
  (NEWID(), 'Inventory Management', ''),
  (NEWID(), 'Chef', '');