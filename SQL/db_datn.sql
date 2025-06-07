
CREATE DATABASE db_datn;

USE db_datn;

-- Bảng OrganizationUnit
CREATE TABLE Organization_Unit (
    State INT,
    OrganizationUnitID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    OrganizationUnitName VARCHAR(255) NOT NULL,
    ParentID INT,
    Code VARCHAR(50),
    Address TEXT,
    Status INT,
    Note TEXT,
    TenantID CHAR(36)
);

-- Bảng Role
CREATE TABLE Role (
    State INT,
    RoleID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    RoleCode VARCHAR(50) NOT NULL,
    RoleName VARCHAR(255) NOT NULL,
    Note TEXT,
    TenantID CHAR(36)
);

-- Bảng Employee
CREATE TABLE Employee (
    EmployeeID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    EmployeeName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    OrganizationUnitID INT,
    OrganizationUnitName VARCHAR(255),
    TenantID CHAR(36),
    FOREIGN KEY (OrganizationUnitID) REFERENCES Organization_Unit(OrganizationUnitID)
);

-- Bảng User
CREATE TABLE User (
    State INT,
    UserID CHAR(36) NOT NULL PRIMARY KEY,
    EmployeeID INT,
    EmployeeName VARCHAR(255),
    Email VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    Password VARCHAR(255) NOT NULL,
    RoleID INT,
    RoleName VARCHAR(255),
    OrganizationUnitID INT,
    OrganizationUnitName VARCHAR(255),
    TenantID CHAR(36) NOT NULL,
    FOREIGN KEY (RoleID) REFERENCES Role(RoleID),
    FOREIGN KEY (OrganizationUnitID) REFERENCES Organization_Unit(OrganizationUnitID)
);

-- Bảng File
CREATE TABLE File (
    State INT,
    FileID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    FileName VARCHAR(255) NOT NULL,
    OrganizationUnitID INT,
    OrganizationUnitName VARCHAR(255),
    ParentID INT,
    CreatedBy CHAR(36),
    CreatedDate DATETIME NOT NULL,
    SharedIDs VARCHAR(255),
    SharedNames VARCHAR(255),
    SharedDate DATETIME,
    Note TEXT,
    NoteShared TEXT,
    NoteOrganizationUnit TEXT,
    TypeFile VARCHAR(50),
    Path VARCHAR(255),
    Content TEXT,
    TenantID CHAR(36),
    Size INT,
    TypeFileEnum INT,
    FOREIGN KEY (OrganizationUnitID) REFERENCES Organization_Unit(OrganizationUnitID),
    FOREIGN KEY (CreatedBy) REFERENCES User(UserID)
);
-- Bảng RoleDetail
CREATE TABLE Role_Detail (
    State INT,
    RoleDetailID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    RoleID INT NOT NULL,
    SubCode VARCHAR(50),
    SubName VARCHAR(255),
    Action VARCHAR(50),
    ActionName VARCHAR(255),
    Value BOOLEAN NOT NULL,
    TenantID CHAR(36),
    FOREIGN KEY (RoleID) REFERENCES Role(RoleID)
);

-- Bảng FileInformation
CREATE TABLE FileInformation (
    FileInfomationID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    FileName VARCHAR(255) NOT NULL,
    Type VARCHAR(50),
    Size INT,
    Path VARCHAR(255),
    Content TEXT
);

ALTER TABLE Employee
ADD EmployeeCode VARCHAR(50),
ADD BirthDay DATETIME,
ADD Gender VARCHAR(10),
ADD Avatar VARCHAR(255),
ADD JobPositionID INT,
ADD JobPositionName VARCHAR(100),
ADD Status INT;

SELECT *FROM User;

INSERT INTO Organization_Unit (OldData, State, OrganizationUnitName, ParentID, Code, Address, Status, Note, TenantID)
VALUES
('Dữ liệu cũ 1', 1, 'Phòng Kinh doanh', NULL, 'KD01', '123 Đường ABC', 1, 'Chú thích phòng Kinh doanh', '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 2', 1, 'Phòng Vận hành', NULL, 'VH01', '456 Đường DEF', 1, 'Chú thích phòng Vận hành', '550e8400-e29b-41d4-a716-446655440001'),
('Dữ liệu cũ 3', 1, 'Phòng Nhân sự', NULL, 'NS01', '789 Đường GHI', 1, 'Chú thích phòng Nhân sự', '550e8400-e29b-41d4-a716-446655440002');



-- Nhập dữ liệu vào bảng Employee
INSERT INTO Employee (EmployeeName, Email, Phone, OrganizationUnitID, OrganizationUnitName, TenantID)
VALUES
('Người A', 'a@example.com', '123456789', 1, 'Phòng Kinh doanh', '550e8400-e29b-41d4-a716-446655440000'),
('Người B', 'b@example.com', '987654321', 2, 'Phòng Vận hành', '550e8400-e29b-41d4-a716-446655440001'),
('Người C', 'c@example.com', '456123789', 3, 'Phòng Nhân sự', '550e8400-e29b-41d4-a716-446655440002');

-- Nhập dữ liệu vào bảng User
INSERT INTO User (OldData, State, UserID, EmployeeID, EmployeeName, Email, Phone, Password, RoleID, RoleName, OrganizationUnitID, OrganizationUnitName, TenantID)
VALUES
('Dữ liệu cũ 1', 1, '123e4567-e89b-12d3-a456-426614174000', 1, 'Người A', 'a@example.com', '0974583183', '123456', 1, 'Quản trị', 1, 'Phòng Kinh doanh', '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 2', 1, '123e4567-e89b-12d3-a456-426614174001', 2, 'Người B', 'b@example.com', '987654321', 'password2', 2, 'Người dùng', 2, 'Phòng Vận hành', '550e8400-e29b-41d4-a716-446655440001'),
('Dữ liệu cũ 3', 1, '123e4567-e89b-12d3-a456-426614174002', 3, 'Người C', 'c@example.com', '456123789', 'password3', 3, 'Khách', 3, 'Phòng Nhân sự', '550e8400-e29b-41d4-a716-446655440002');

-- Nhập dữ liệu vào bảng File
INSERT INTO File (OldData, State, FileID, FileName, OrganizationUnitID, OrganizationUnitName, CreatedBy, CreatedDate, Note, TenantID)
VALUES
('Dữ liệu cũ 1', 1, 1, 'File1.txt', 1, 'Phòng Kinh doanh', '123e4567-e89b-12d3-a456-426614174000', NOW(), 'Ghi chú về file 1', '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 2', 1, 2, 'File2.txt', 2, 'Phòng Vận hành', '123e4567-e89b-12d3-a456-426614174001', NOW(), 'Ghi chú về file 2', '550e8400-e29b-41d4-a716-446655440001'),
('Dữ liệu cũ 3', 1, 3, 'File3.txt', 3, 'Phòng Nhân sự', '123e4567-e89b-12d3-a456-426614174002', NOW(), 'Ghi chú về file 3', '550e8400-e29b-41d4-a716-446655440002');
UPDATE File
SET 
    FileName = 'DATN.docx',
    Note = 'Ghi chú đã cập nhật về file 1',
    Path = 'C:\\Users\\Acer\\OneDrive\\Máy tính\\DATN\\Demo\\Du_An\\SQL\\File\\DATN.docx',
    TypeFile = 'Document',
    Content = 'II. Mở đầu
1. Giới thiệu chung
Ngày nay, với việc công nghệ ngày càng phát triển nhanh chóng, mạnh mẽ và càng ngày càng trở nên phổ biến hơn với mọi người dân thì việc sử dụng những tài liệu mềm cùng với các thiết bị điện tử hiện đại phục vụ cho việc lưu trữ và quản lý tài liệu đóng vai trò hết sức quan trọng đối với các tổ chức, công ty và các doanh nghiệp lớn nhỏ. Hãy thử tưởng tượng con người có thể tiết kiệm đến hàng nghìn tỉ đô la mĩ khi sử dụng tài liệu mềm và số hóa để lưu trữ và quản lý chúng. 
Đầu tiên, việc sử dụng tài liệu mềm sẽ giảm thiểu đáng kể không gian và diện tích để lưu trữ so với việc sử dụng tài liệu cứng dạng giấy, chưa kể chi phí để đóng các kệ sách, giá sách cho việc lưu trữ một số lượng lớn tài liệu bản cứng là vô cùng lớn. Thứ hai là việc số hóa tài liệu dưới dạng mềm sẽ giúp cho việc bảo quản và lưu giữ cho tuổi thọ của những tài liệu truyền thống được lâu hơn. Thứ ba là dễ dàng mở rộng phạm vi cộng đồng người sử dụng nguồn tài nguyên thông tin của cơ quan thông tin, thư viện vì việc truyền tay nhau những tài liệu mềm chắc chắn luôn luôn dễ dàng và hiệu quả hơn so với tài liệu bản cứng. Thứ tư là tiện ích trong việc truy xuất tìm kiếm thông tin ở bất kỳ đâu vào bất cứ thời điểm nào một cách nhanh chóng, dễ dàng. Thật vậy, nếu so với việc thao tác tìm kiếm tài liệu trên Internet với việc tìm và tra sách bằng cách đọc từng cuốn sách trong thư viện sẽ nhanh chóng hơn, tiết kiệm thời gian hơn và hiệu quả hơn rất nhiều. Cuối cùng là thuận lợi trong việc chia sẻ nguồn tài nguyên thông tin của thư viện với các thư viện khác.
Bên cạnh những ưu điểm trên, việc sử dụng và quản lý tài liệu mềm cũng có một số khuyết điểm, hạn chế gây ra những khó khăn và thử thách không hề nhỏ đối với các công ty, tổ chức và doanh nghiệp trong việc quản lý các dạng tài liệu mềm. Đầu tiên là chi phí cho việc đầu tư máy móc, trang thiết bị hiện đại cho việc quản lý và lưu trữ một lượng lớn tài liệu là khá đắt đỏ. Tiếp theo là nguy cơ về việc tài liệu chính thống có thể bị sửa đổi và sao chép rồi phát tán trái phép nhằm bóp méo sự thật và làn truyền những thông tin giả gây ảnh hưởng tới mọi người. Và cuối cùng là vấn đề đồng bộ cũng như là bảo mật những hồ sơ, tài liệu bí mật khỏi bị rò rỉ và những cuộc tấn công nhằm mục đích chiếm đoạt đang là khó khăn cũng như là thách thức cho hàng ngàn các doanh nghiệp, công ty, tổ chức trên toàn cầu. Do đó các công ty, doanh nghiệp cần phải phát huy tối đa các ưu điểm và hạn chế nhược điểm xuống mức tối thiểu của việc quản lý tài liệu mềm để có thể tối ưu hóa các lợi ích cho doanh nghiệp của mình.
2. Mục tiêu
Hiện nay có rất nhiều các trang web và ứng dụng giúp cho việc quản lý và lưu trữ tài liệu của các doanh nghiệp trở nên đơn giản và bảo mật hơn. Tuy nhiên bên cạnh những trang web chính trực xuất hiện thêm rất nhiều các trang web và các ứng dụng lừa đảo nhằm trục lợi và thu thập trái phép thông tin cá nhân của người dùng. 
Để góp phần tăng độ uy tín cũng như hữu dụng của các ứng dụng quản lý tài liệu cũng như là đầy lùi những trang web lừa đảo, nhóm dự án xin được phép giới thiệu tới các bạn đọc một ứng dụng uy tín và bảo mật nhưng cũng rất dễ sử dụng và thân thiện với người dùng trên nền tàng web được phát triển bởi nhóm - Ứng dụng quản lý tài liệu. 
Do đó, mục tiêu của nhóm là ngoài việc hoàn thành dự án ra còn có mục tiêu muốn phát triển và triển khai sản phẩm với mong muốn sản phẩm có thể giúp ích cho công việc sử dụng tài liệu trong thực tiễn. Không những thế, nhóm còn muốn được làm việc cùng nhau một cách năng động và chuyên nghiệp. Để có thể phát triển và triển khai sản phẩm chất lượng, nhóm cần tận dụng tối đa những thế mạnh của từng thành viên trong nhóm để có thể tối ưu hiệu quả làm việc của nhóm.
3. Những khó khăn và giải pháp
Vì đây là lần đầu tiên nhóm làm việc cùng với nhau với mục đích phát triển một sản phẩm mang tính thực tế và tích hợp các công nghệ tiên tiến nên sẽ có những khó khăn nhất định cho mỗi thành viên trong nhóm trong quá trình phát triển và triển khai sản phẩm. Đầu tiên phải kể đến khó khăn trong việc lên kế hoạch, ý tưởng thiết kế cũng như là việc xây dựng nền móng cho ứng dụng và tìm tài liệu thích hợp để tham khảo. Kế tiếp đó là thời gian ngắn cho nhóm vì xét về mặt bằng chung thì kỹ năng của nhóm không phải quá cao nên thời gian cho quá trình phát triển dự án của team là khá gấp rút và hạn hẹp. Tiếp theo là bài toán làm thế nào để tìm kiếm tài liệu nhanh chóng và lưu trữ được số lớn lượng tài liệu. Và một khó khăn nữa cũng là khó khăn chung của rất nhiều cá nhân, tổ chức hiện thời đó là vấn đề dịch bệnh phức tạp có thể làm giảm khả năng team work của nhóm do phải liên lạc và làm việc với nhau bằng hình thức trực tuyến.
Sau đây là những giải pháp của nhóm đề ra để giải quyết cho những vấn đề và khó khăn kể trên. Tuy không thể giải quyết triệt để những thách thức nhưng có thể giảm thiểu được những vấn đề trên. Thứ nhất là nhóm đã tích cực tìm kiếm tài liệu cũng như đề xuất ý tưởng của từng thành viên của nhóm để xây dựng và thiết kế ứng dụng từ những bước đầu tiên. Thứ hai là nhóm đã lên kế hoạch một cách khoa học, cụ thể và chi tiết để có thể tận dụng tối đa thời gian cho việc tập trung hoàn thành dự án một cách nhanh nhất. Và để giải quyết vấn đề thứ ba, nhóm đã hoạt động và làm việc trực tuyến để đảm bảo hiệu suất và tiến độ công việc. Tuy vậy, vấn đề tìm kiếm tài liệu một cách nhanh chóng và lưu trữ tài liệu vẫn gây cản trở một phần cho nhóm do chưa tìm ra giải pháp để giải quyết triệt để cùng với việc trang thiết bị của nhóm chưa thực sự đáp ứng đầy đủ các yêu cầu về phần cứng để có thể vận hành sản phẩm một cách tối ưu.
4. Kết quả sơ bộ
Tuy gặp phải một số các khó khăn và thách thức, nhưng nhóm cũng đã đạt được kết quả nhất định. Với việc sử dụng ứng dụng của nhóm, hệ thống sẽ hỗ trợ cho người dùng tính năng upload và download các file tài liệu. ấy. Không những thế, hệ thống được tích hợp với công cụ tìm kiếm giúp cho người dùng có thể tìm kiếm toàn văn với từ khóa được nhập liệu. Cùng với việc sở hữu giao diện thân thiện, bắt mắt và phù hợp với mọi lứa tuổi, người dùng chỉ cần thao tác với chuột và bàn phím để có thể sử dụng toàn bộ mọi tính năng và lợi ích mà ứng dụng mang lại.
Ngoài những tính năng kể trên, hệ thống còn cho phép người dùng chia sẻ các tài nguyên giữa các đơn vị cũng như là thiết lập các cài đặt sao cho phù hợp và thuận tiện cho việc sử dụng của từng người. Hệ thống giúp người dùng upload tài liệu từ thiết bị lên hệ thống hay download tài liệu lưu về máy. Ngoài ra, ứng dụng còn giúp người dùng in ấn và tạo thư mục lưu trữ tài liệu. Về bảo mật, hệ thống sử dụng các cơ chế định danh và định quyền cho các vai trò của từng tài khoản để đảm bảo tối đa mức độ tin cậy và giữ an toàn tuyệt đối cho các tài liệu quan trọng. Mỗi người dùng hệ thống sẽ có một vai trò và quyền hạn riêng đối với các tài liệu để đảm bảo phù hợp cho mục đích sử dụng, chia sẻ và quản lý tài liệu riêng của mỗi cá nhân. Chẳng hạn như quyền quản trị viên hệ thống sẽ có mọi quyền quản lý và truy cập tại hệ thống, còn các thành viên thì chỉ có quyền hạn quản lý tài liệu và chia sẻ tài nguyên qua lại giữa các đơn vị.

III. Công nghệ sử dụng
	Về phần frontend, công nghệ chính nhóm sử dụng là Vuejs, ngoài ra có một số công nghệ sử dụng hỗ trợ như HTML, CSS, SCSS, Javascript, … Còn về phía phía backend, nhóm sử dụng .NET core. Cơ sở dữ liệu để lưu trữ thông tin sử dụng mysql, elasticsearch. Cùng với đó là tích hợp một số module của npm cho việc config và vận hành ứng dụng trên nền tảng website.
	Về phần review và quản lý source code, nhóm sử dụng git và lưu trữ, bảo trì mã nguồn trên trang web github.
IV.  Phân tích và thiết kế
1. Tổng quan
Về phần phân tích và thiết kế ứng dụng, nhóm đã lên kế hoạch phát triển dựa trên các tiêu chí:
Đa dạng hóa các loại và kiểu tài liệu mềm.
Áp dụng và tích hợp những công nghệ mới, những ý tượng độc đáo vào việc xử lý và đặt nền móng cho việc thiết kế ứng dụng.
Sự hữu ích, đa dạng, mới mẻ của ứng dụng.
Có tính thực tế, khả thi và có thể triển khai với nhiều hướng phát triển trong tương lai.
An toàn, bảo mật, tạo sự uy tín và thân thiện tới người dùng.
Thiết kế giao diện thân thiện, phù hợp với mọi lứa tuổi người dùng và dễ dùng, dễ sử dụng.
'
WHERE FileID = 1;

-- Nhập dữ liệu vào bảng Role
INSERT INTO Role (OldData, State, RoleCode, RoleName, Note, TenantID)
VALUES
('Dữ liệu cũ 1', 1, 'ROLE_ADMIN', 'Quản trị', 'Chú thích vai trò quản trị', '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 2', 1, 'ROLE_USER', 'Người dùng', 'Chú thích vai trò người dùng', '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 3', 1, 'ROLE_GUEST', 'Khách', 'Chú thích vai trò khách', '550e8400-e29b-41d4-a716-446655440000');

DELETE FROM RoleDetail;
SHOW TABLES;
-- Nhập dữ liệu vào bảng Role_Detail
INSERT INTO Role_Detail (OldData, State, RoleDetailID, RoleID, SubCode, SubName, Action, ActionName, Value, TenantID)
VALUES
('Dữ liệu cũ 1', 1, 1, 1, 'OrganizationUnit', 'Đơn vị', 'View', 'Xem', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 2, 1, 'OrganizationUnit', 'Đơn vị', 'Add', 'Thêm', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 3, 1, 'OrganizationUnit', 'Đơn vị', 'Delete', 'Xóa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 4, 1, 'OrganizationUnit', 'Đơn vị', 'Download', 'Tải xuống', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 5, 1, 'Personal', 'Cá nhân', 'View', 'Xem', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 6, 1, 'Personal', 'Cá nhân', 'Add', 'Thêm', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 7, 1, 'Personal', 'Cá nhân', 'Delete', 'Xóa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 8, 1, 'Personal', 'Cá nhân', 'Edit', 'Sửa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 9, 1, 'Shared', 'Chia sẻ', 'View', 'Xem', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 10, 1, 'Shared', 'Chia sẻ', 'Add', 'Thêm', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 11, 1, 'Shared', 'Chia sẻ', 'Delete', 'Xóa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 12, 1, 'Shared', 'Chia sẻ', 'Edit', 'Sửa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 13, 1, 'Employee', 'Thiết lập/Nhân viên', 'View', 'Xem', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 14, 1, 'Employee', 'Thiết lập/Nhân viên', 'Add', 'Thêm', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 15, 1, 'Employee', 'Thiết lập/Nhân viên', 'Delete', 'Xóa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 16, 1, 'Employee', 'Thiết lập/Nhân viên', 'Edit', 'Sửa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 17, 1, 'Unit', 'Thiết lập/Đơn vị', 'View', 'Xem', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 18, 1, 'Unit', 'Thiết lập/Đơn vị', 'Add', 'Thêm', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 19, 1, 'Unit', 'Thiết lập/Đơn vị', 'Delete', 'Xóa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 20, 1, 'Unit', 'Thiết lập/Đơn vị', 'Edit', 'Sửa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 21, 1, 'User', 'Thiết lập/Người dùng', 'View', 'Xem', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 22, 1, 'User', 'Thiết lập/Người dùng', 'Add', 'Thêm', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 23, 1, 'User', 'Thiết lập/Người dùng', 'Delete', 'Xóa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 24, 1, 'User', 'Thiết lập/Người dùng', 'Edit', 'Sửa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 25, 1, 'Role', 'Thiết lập/Vai trò', 'View', 'Xem', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 26, 1, 'Role', 'Thiết lập/Vai trò', 'Add', 'Thêm', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 27, 1, 'Role', 'Thiết lập/Vai trò', 'Delete', 'Xóa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 28, 1, 'Role', 'Thiết lập/Vai trò', 'Edit', 'Sửa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 29, 1, 'Email', 'Thiết lập/Email hệ thống', 'Edit', 'Sửa', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 30, 1, 'Email', 'Thiết lập/Email hệ thống', 'View', 'Xem', TRUE, '550e8400-e29b-41d4-a716-446655440000'),
('Dữ liệu cũ 1', 1, 31, 1, 'Log', 'Thiết lập/Nhật ký hoạt động', 'View', 'Xem', TRUE, '550e8400-e29b-41d4-a716-446655440000');

DROP TABLE RoleDetail;

-- Nhập dữ liệu vào bảng FileInformation
INSERT INTO FileInformation (FileInfomationID, FileName, Type, Size, Path, Content)
VALUES
(1, 'FileInfo1.txt', 'Text', 1024, '/path/to/fileinfo1.txt', 'Nội dung file thông tin 1'),
(2, 'FileInfo2.txt', 'Text', 2048, '/path/to/fileinfo2.txt', 'Nội dung file thông tin 2'),
(3, 'FileInfo3.txt', 'Text', 3072, '/path/to/fileinfo3.txt', 'Nội dung file thông tin 3');





