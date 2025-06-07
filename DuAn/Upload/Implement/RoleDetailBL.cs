using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Interface;
using Upload.Models;

namespace Upload.Implement
{
    public class RoleDetailBL: BaseBL, IRoleDetailBL
    {
        public RoleDetailBL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {

        }

        public async Task<object> GetListRoleDetail(string id)
        {
            string sql = $"SELECT * FROM role_detail WHERE RoleID = {id};";
            var res = await QueryCommandTexListtAsync<RoleDetail>(sql);
            return res;
        }
        public async Task<object> AddRoleDetail(Role role)
        {
            var _role = new Role();
            _role.RoleName = role.RoleName;
            _role.Note = role.Note;
            _role.TenantID = role.TenantID;
            var parameters_1 = GetParameters(_role, typeof(Role));
            var resul = await _dBConnection.ExecuteScalarAsync<int>("Proc_Role_Insert", parameters_1, commandType: CommandType.StoredProcedure);

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO role_detail (RoleID, SubCode, SubName, Action, ActionName, Value, TenantID) VALUES ");

            var values = new List<string>();

            foreach (var item in role.ListRole)
            {
                item.RoleID = resul;
                item.TenantID = role.TenantID;

                values.Add($"({item.RoleID}, '{item.SubCode}', '{item.SubName}', '{item.Action}', '{item.ActionName}', {item.Value}, '{item.TenantID}')");
            }

            sql.AppendLine(string.Join(", ", values) + ";");
            string test = sql.ToString();
            return await QueryAsync<Models.RoleDetail>(sql.ToString());
        }
    }
}
