create database ECommerceTest

use ECommerceTest

select * from Products

select * from AspNetUserRoles

select * from AspNetUsers

select * from AspNetRoles

select * from Permissions
order by Id

select * from RolePermissions

select * from Carts

select * from MenuItems


SELECT * FROM __EFMigrationsHistory;

DELETE FROM __EFMigrationsHistory
WHERE MigrationId = '20260415074211_RecreateMenuItems';

SELECT * 
FROM sys.indexes 
WHERE name = 'IX_Permissions_Name';

DROP INDEX IX_Permissions_Name ON Permissions;

-- =========================
-- PRODUCTS ROOT
-- =========================
INSERT INTO MenuItems (Title, Controller, Action, ParentId, PermissionId)
VALUES ('Products', NULL, NULL, NULL, NULL);

DECLARE @ProductsId INT = SCOPE_IDENTITY();

-- CHILDREN OF PRODUCTS
INSERT INTO MenuItems (Title, Controller, Action, ParentId, PermissionId)
VALUES 
('All Products', 'Product', 'Index', @ProductsId, 1),
('Create Product', 'Product', 'Create', @ProductsId, 3);


-- =========================
-- CART ROOT
-- =========================
INSERT INTO MenuItems (Title, Controller, Action, ParentId, PermissionId)
VALUES ('Cart', NULL, NULL, NULL, NULL);

DECLARE @CartId INT = SCOPE_IDENTITY();

-- CHILDREN OF CART
INSERT INTO MenuItems (Title, Controller, Action, ParentId, PermissionId)
VALUES
('View Cart', 'Cart', 'MyCart', @CartId, 7);


SELECT 
    p.Id,
    p.Name,
    p.Price,
    CASE 
        WHEN c.ProductId IS NOT NULL THEN 1 
        ELSE 0 
    END AS IsInCart
FROM Products p
LEFT JOIN Carts c 
    ON c.ProductId = p.Id
    AND c.UserId = '1fe9759b-2a32-4a2f-b24a-c254707d3767'


SELECT 
    p.Id,
    p.Name,
    p.Price,
    c.ProductId AS InCart
FROM Products p
LEFT JOIN Carts c
    ON c.ProductId = p.Id
    AND c.UserId = '1fe9759b-2a32-4a2f-b24a-c254707d3767'


SELECT p.Id , p.Name , p.Price , c.ProductId as inCart
FROM Products p
LEFT JOIN Carts c 
    ON p.Id = c.ProductId and c.UserId = '1fe9759b-2a32-4a2f-b24a-c254707d3767'




INSERT INTO Permissions (Name) VALUES
-- Role Management
('View Roles'),
('Create Role'),
('Edit Role'),
('Delete Role'),

-- Permission Management
('View Permissions'),
('Create Permission'),
('Edit Permission'),
('Delete Permission'),

-- User Management
('View Users'),
('Create User'),
('Edit User'),
('Delete User');

INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT '505c99d2-a131-43c1-b4f9-ecd8106dd9d5', Id
FROM Permissions
WHERE Id >= 8 AND Id <= 19;




-- Parent: Admin
INSERT INTO MenuItems (Title, Controller, Action, PermissionId, ParentId)
VALUES ('Admin', NULL, NULL, NULL, NULL);

-- Roles
INSERT INTO MenuItems VALUES
('Roles', 'Admin', 'Roles', 8, 6),
('Create Role', 'Admin', 'CreateRole', 9, 6);

-- Permissions
INSERT INTO MenuItems VALUES
('Permissions', 'Admin', 'Permissions', 12, 6),
('Create Permission', 'Admin', 'CreatePermission', 13, 6);

-- Users
INSERT INTO MenuItems VALUES
('Users', 'Admin', 'Users', 16, 6),
('Create User', 'Admin', 'CreateUser', 17, 6);



INSERT INTO Permissions (Name)
VALUES ('Create Menu Item');


INSERT INTO RolePermissions (RoleId, PermissionId)
VALUES ('505c99d2-a131-43c1-b4f9-ecd8106dd9d5', 20);


INSERT INTO MenuItems (Title, Controller, Action, PermissionId, ParentId)
VALUES ('Menu Items', 'Admin', 'CreateMenuItem', 20, NULL);

INSERT INTO MenuItems (Title, Controller, Action, PermissionId, ParentId)
VALUES ('Create Menu Item', 'Admin', 'CreateMenuItem', 20, 
       (SELECT Id FROM MenuItems WHERE Title = 'Menu Items'));

select * from AspNetRoles

select * from Permissions
order by Id

select * from RolePermissions

select * from AspNetUserRoles

select * from MenuItems



update MenuItems
set Controller = NULL , Action = NULL , PermissionId = NULL
where Id = 13


update MenuItems
set ParentId = NULL where Id = 13


update MenuItems
set ParentId = 6 where Id = 14

delete from MenuItems where id in(
select id from MenuItems where Title = 'Menu Items'
)








