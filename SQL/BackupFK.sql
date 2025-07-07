ALTER TABLE user 
  ADD CONSTRAINT fk_employee FOREIGN KEY (EmployeeID)
    REFERENCES employee(EmployeeID);

ALTER TABLE user
DROP CONSTRAINT fk_employee;

ALTER TABLE user 
  ADD CONSTRAINT user_ibfk_1 FOREIGN KEY (RoleID)
    REFERENCES role(RoleID);

ALTER TABLE user 
  ADD CONSTRAINT user_ibfk_2 FOREIGN KEY (OrganizationUnitID)
    REFERENCES organization_unit(OrganizationUnitID);

ALTER TABLE role_detail 
  ADD CONSTRAINT role_detail_ibfk_1 FOREIGN KEY (RoleID)
    REFERENCES role(RoleID);

ALTER TABLE employee 
  ADD CONSTRAINT employee_ibfk_1 FOREIGN KEY (OrganizationUnitID)
    REFERENCES organization_unit(OrganizationUnitID);

ALTER TABLE file 
  ADD CONSTRAINT file_ibfk_1 FOREIGN KEY (OrganizationUnitID)
    REFERENCES organization_unit(OrganizationUnitID);

ALTER TABLE file 
  ADD CONSTRAINT fk_CreatedBy FOREIGN KEY (CreatedBy)
    REFERENCES employee(EmployeeID);


INSERT INTO organization_unit (
    OrganizationUnitName, 
    ParentID, 
    Code, 
    Address, 
    Status, 
    Note, 
    TenantID
) VALUES (
    'Test', 
    NULL, 
    'SD001', 
    '123 Main St, City', 
    1, 
    'Main sales unit', 
    '123e4567-e89b-12d3-a456-426614174000'
);

INSERT INTO backup_datn.employee (
    EmployeeName, 
    Email, 
    Phone, 
    OrganizationUnitID, 
    OrganizationUnitName, 
    TenantID, 
    EmployeeCode, 
    BirthDay, 
    Gender, 
    Avatar, 
    JobPositionID, 
    JobPositionName, 
    Status
) VALUES (
    'Jane Smith', 
    'jane.smith@example.com', 
    '0987654321', 
    11, 
    'Sales', 
    '123e4567-e89b-12d3-a456-426614174000', 
    'EMP001', 
    '1990-05-15', 
    'Female', 
    'avatar.png', 
    2, 
    'Sales Manager', 
    1
);

INSERT INTO backup_datn.role (
    RoleCode, 
    RoleName, 
    Note, 
    TenantID
) VALUES (
    'ADMIN', 
    'Administrator', 
    'Has full access to the system', 
    '123e4567-e89b-12d3-a456-426614174000'
);

INSERT INTO backup_datn.role_detail (RoleID, SubCode, SubName, Action, ActionName, Value, TenantID) VALUES
(31, 'OrganizationUnit', 'Đơn vị', 'View', 'Xem', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'OrganizationUnit', 'Đơn vị', 'Add', 'Thêm', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'OrganizationUnit', 'Đơn vị', 'Delete', 'Xóa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'OrganizationUnit', 'Đơn vị', 'Download', 'Tải xuống', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Personal', 'Cá nhân', 'View', 'Xem', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Personal', 'Cá nhân', 'Add', 'Thêm', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Personal', 'Cá nhân', 'Delete', 'Xóa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Personal', 'Cá nhân', 'Download', 'Tải xuống', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Shared', 'Chia sẻ', 'View', 'Xem', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Shared', 'Chia sẻ', 'Add', 'Thêm', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Shared', 'Chia sẻ', 'Delete', 'Xóa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Shared', 'Chia sẻ', 'Download', 'Tải xuống', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Employee', 'Thiết lập/Nhân viên', 'View', 'Xem', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Employee', 'Thiết lập/Nhân viên', 'Add', 'Thêm', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Employee', 'Thiết lập/Nhân viên', 'Edit', 'Sửa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Employee', 'Thiết lập/Nhân viên', 'Delete', 'Xóa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Unit', 'Thiết lập/Đơn vị', 'View', 'Xem', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Unit', 'Thiết lập/Đơn vị', 'Add', 'Thêm', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Unit', 'Thiết lập/Đơn vị', 'Edit', 'Sửa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Unit', 'Thiết lập/Đơn vị', 'Delete', 'Xóa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'User', 'Thiết lập/Người dùng', 'View', 'Xem', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'User', 'Thiết lập/Người dùng', 'Add', 'Thêm', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'User', 'Thiết lập/Người dùng', 'Edit', 'Sửa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'User', 'Thiết lập/Người dùng', 'Delete', 'Xóa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Role', 'Thiết lập/Vai trò', 'View', 'Xem', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Role', 'Thiết lập/Vai trò', 'Add', 'Thêm', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Role', 'Thiết lập/Vai trò', 'Edit', 'Sửa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Role', 'Thiết lập/Vai trò', 'Delete', 'Xóa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Email', 'Thiết lập/Email hệ thống', 'View', 'Xem', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Email', 'Thiết lập/Email hệ thống', 'Edit', 'Sửa', 1, '123e4567-e89b-12d3-a456-426614174000'),
(31, 'Log', 'Thiết lập/Nhật ký hoạt động', 'View', 'Xem', 1, '123e4567-e89b-12d3-a456-426614174000');

INSERT INTO backup_datn.user (
    UserID, 
    EmployeeID, 
    EmployeeName, 
    Email, 
    Phone, 
    Password, 
    RoleID, 
    RoleName, 
    OrganizationUnitID, 
    OrganizationUnitName, 
    TenantID
) VALUES (
    '123e4567-e89b-12d3-a456-426614174000',  -- UserID
    20,                                          -- EmployeeID
    '',                                 -- EmployeeName
    'john.doe@example.com',                     -- Email
    '0974583183',                               -- Phone
    '1',                        -- Password
    31,                                         -- RoleID
    'Administrator',                            -- RoleName
    11,                                          -- OrganizationUnitID
    'Sales Department',                         -- OrganizationUnitName
    '123e4567-e89b-12d3-a456-426614174000'     -- TenantID
);

use database backup_datn

USE backup_datn; 

SELECT * FROM role_detail


DELETE FROM role_detail
WHERE RoleID = 34;

ALTER TABLE role_detail
DROP FOREIGN KEY RoleID;

SHOW CREATE TABLE role_detail;