using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Interface;
using Upload.Models;

namespace Upload.Implement
{
    public class UserBL : BaseBL, IUserBL
    {
        private readonly AppSettings _appSettings;
        private readonly IRoleDetailBL _roleDetailBL;

        public UserBL(IConfiguration configuration, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor, IRoleDetailBL roleDetailBL) : base(configuration, httpContextAccessor)
        {
            _appSettings = appSettings.Value;
            _roleDetailBL = roleDetailBL;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            //Kiểm tra dưới database có tài khoản này không
            string sql = $"SELECT * FROM user WHERE (Email = '{model.Username}' OR Phone = '{model.Username}') AND Password = '{model.Password}'";
            var res = await QueryCommandTextAsync<User>(sql);
            // return null if user not found
            if (res == null) return null;
            // authentication successful so generate jwt token
            var token = generateJwtToken((User)res);
            return new AuthenticateResponse((User)res, token);
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("UserID", user.UserID.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<object> UserInfo()
        {
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            Employee employee = (Employee)await GetByID<Employee>(user.EmployeeID.ToString(),typeof(Employee),"employee");
            userInfo.Add("EmployeeID", user.EmployeeID);
            userInfo.Add("EmployeeName", employee.EmployeeName);
            userInfo.Add("OrganizationUnitID", user.OrganizationUnitID);
            userInfo.Add("OrganizationUnitName", user.OrganizationUnitName);
            //Lấy ra danh sách quyền
            var listRole = await _roleDetailBL.GetListRoleDetail(user.RoleID.ToString());
            userInfo.Add("ListRole", listRole);
            return userInfo;
        }
        public async Task<object> UserProfile()
        {
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            Employee employee = (Employee)await GetByID<Employee>(user.EmployeeID.ToString(), typeof(Employee), "employee");
            userInfo.Add("EmployeeName", employee.EmployeeName);
            userInfo.Add("Email", user.Email);
            userInfo.Add("TenantID", user.TenantID);
            return userInfo;
        }

        public async Task<object> Signup(Dictionary<string, object> param)
        {
            Guid _userID = Guid.NewGuid();

            var _unit = new OrganizationUnit
            {
                OrganizationUnitName = param["company"].ToString(),
                Code = "DV" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                Status = 0,
                TenantID = _userID
            };
            var parameters_2 = GetParameters(_unit, typeof(Models.OrganizationUnit));
            var resul_2 = await _dBConnection.ExecuteScalarAsync<int>($"Proc_OrganizationUnit_Insert", parameters_2, commandType: CommandType.StoredProcedure);


            var _employee = new Employee
            {
                EmployeeName = param["fullName"].ToString(),
                Email = param["username"].ToString(),
                Phone = param["phone"].ToString(),
                EmployeeCode = "NV" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                Gender = param["gender"].ToString(),
                OrganizationUnitName = param["company"].ToString(),
                OrganizationUnitID = resul_2,
                BirthDay = DateTime.Parse(param["dateOfBirth"].ToString()),
                JobPositionName = param["position"].ToString(),
                TenantID = _userID
            };
            var parameters_1 = GetParameters(_employee, typeof(Models.Employee));
            var resul_1 = await _dBConnection.ExecuteScalarAsync<int>($"Proc_Employee_Insert", parameters_1, commandType: CommandType.StoredProcedure); 

            var _role = new Role();
            _role.RoleName = "Admin";
            _role.Note = "Quản trị hệ thống";
            _role.TenantID = _userID;
            var parameters_role = GetParameters(_role, typeof(Role));
            var resul = await _dBConnection.ExecuteScalarAsync<int>("Proc_Role_Insert", parameters_role, commandType: CommandType.StoredProcedure);
            string sql = $"INSERT INTO role_detail (RoleID, SubCode, SubName, Action, ActionName, Value, TenantID) VALUES " +
                $"({resul}, 'OrganizationUnit', 'Đơn vị', 'View', 'Xem', 1, '{_userID}')," +
                $"({resul}, 'OrganizationUnit', 'Đơn vị', 'Add', 'Thêm', 1, '{_userID}')," +
                $"({resul}, 'OrganizationUnit', 'Đơn vị', 'Delete', 'Xóa', 1, '{_userID}'), " +
                $"({resul}, 'OrganizationUnit', 'Đơn vị', 'Download', 'Tải xuống', 1, '{_userID}'), " +
                $"({resul}, 'Personal', 'Cá nhân', 'View', 'Xem', 1, '{_userID}'), " +
                $"({resul}, 'Personal', 'Cá nhân', 'Add', 'Thêm', 1, '{_userID}')," +
                $"({resul}, 'Personal', 'Cá nhân', 'Delete', 'Xóa', 1, '{_userID}')," +
                $"({resul}, 'Personal', 'Cá nhân', 'Download', 'Tải xuống', 1, '{_userID}')," +
                $"({resul}, 'Shared', 'Chia sẻ', 'View', 'Xem', 1, '{_userID}')," +
                $"({resul}, 'Shared', 'Chia sẻ', 'Add', 'Thêm', 1, '{_userID}')," +
                $"({resul}, 'Shared', 'Chia sẻ', 'Delete', 'Xóa', 1, '{_userID}')," +
                $"({resul}, 'Shared', 'Chia sẻ', 'Download', 'Tải xuống', 1, '{_userID}')," +
                $"({resul}, 'Employee', 'Thiết lập/Nhân viên', 'View', 'Xem', 1, '{_userID}')," +
                $"({resul}, 'Employee', 'Thiết lập/Nhân viên', 'Add', 'Thêm', 1, '{_userID}')," +
                $"({resul}, 'Employee', 'Thiết lập/Nhân viên', 'Edit', 'Sửa', 1, '{_userID}'), " +
                $"({resul}, 'Employee', 'Thiết lập/Nhân viên', 'Delete', 'Xóa', 1, '{_userID}')," +
                $"({resul}, 'Unit', 'Thiết lập/Đơn vị', 'View', 'Xem', 1, '{_userID}')," +
                $"({resul}, 'Unit', 'Thiết lập/Đơn vị', 'Add', 'Thêm', 1, '{_userID}')," +
                $"({resul}, 'Unit', 'Thiết lập/Đơn vị', 'Edit', 'Sửa', 1, '{_userID}')," +
                $"({resul}, 'Unit', 'Thiết lập/Đơn vị', 'Delete', 'Xóa', 1, '{_userID}')," +
                $"({resul}, 'User', 'Thiết lập/Người dùng', 'View', 'Xem', 1, '{_userID}')," +
                $"({resul}, 'User', 'Thiết lập/Người dùng', 'Add', 'Thêm', 1, '{_userID}')," +
                $"({resul}, 'User', 'Thiết lập/Người dùng', 'Edit', 'Sửa', 1, '{_userID}')," +
                $"({resul}, 'User', 'Thiết lập/Người dùng', 'Delete', 'Xóa', 1, '{_userID}')," +
                $"({resul}, 'Role', 'Thiết lập/Vai trò', 'View', 'Xem', 1, '{_userID}')," +
                $"({resul}, 'Role', 'Thiết lập/Vai trò', 'Add', 'Thêm', 1, '{_userID}'), " +
                $"({resul}, 'Role', 'Thiết lập/Vai trò', 'Edit', 'Sửa', 1, '{_userID}'), " +
                $"({resul}, 'Role', 'Thiết lập/Vai trò', 'Delete', 'Xóa', 1, '{_userID}'), " +
                $"({resul}, 'Email', 'Thiết lập/Email hệ thống', 'View', 'Xem', 1, '{_userID}'), " +
                $"({resul}, 'Email', 'Thiết lập/Email hệ thống', 'Edit', 'Sửa', 1, '{_userID}')," +
                $"({resul}, 'Log', 'Thiết lập/Nhật ký hoạt động', 'View', 'Xem', 1, '{_userID}');";

            await QueryAsync<Models.RoleDetail>(sql);

            var _user = new User
            {
                UserID = _userID,
                EmployeeName = param["fullName"].ToString(),
                EmployeeID = resul_1,
                Email = param["username"].ToString(),
                Phone = param["phone"].ToString(),
                Password = param["password"].ToString(),
                RoleID = resul,
                RoleName = "Admin",
                OrganizationUnitID = resul_2,
                OrganizationUnitName = param["company"].ToString(),
                TenantID = _userID
            };
            var parameters = GetParameters(_user, typeof(Models.User));
            var resul_FN = await _dBConnection.ExecuteScalarAsync<object>($"Proc_User_Insert", parameters, commandType: CommandType.StoredProcedure);
            
            return resul_FN;
        }

        public async Task<object> InsertUser(User _user)
        {
            Guid _userID = Guid.NewGuid();

            //string sql = $"SELECT * FROM file WHERE TenantID = '{_tenantID}'";

            //var test = await QueryAsync<Models.File>(sql);
            
            _user.UserID = _userID;
            _user.Password = "http0604";
            var parameters = GetParameters(_user, typeof(Models.User));
            var result = await _dBConnection.ExecuteScalarAsync<object>("Proc_User_Insert", parameters, commandType: CommandType.StoredProcedure);
            return result; 
        }
        public async Task<object> Delete_User(int id)
        {
            string sql = $"DELETE FROM user WHERE EmployeeID = {id};";
            return await QueryAsync<Models.User>(sql);
        }
    }
}
