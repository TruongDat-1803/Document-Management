using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Interface;
using Upload.Models;

namespace Upload.Implement
{
    public class BaseBL : IBaseBL
    {
        public IConfiguration _configuration;
        public string _connectionString;
        public IDbConnection _dBConnection;
        public string _tableName = null;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid _tenantID
        {
            get
            {
                var user = (User)_httpContextAccessor.HttpContext.Items["User"];
                return user.TenantID;
            }
        }

        public BaseBL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ConnectDB");
            _dBConnection = new MySqlConnection(_connectionString);
            _httpContextAccessor = httpContextAccessor;
        }
        public void SetTableName(string tableName)
        {
            _tableName = tableName;
        }
        //Hàm get all
        public virtual async Task<object> GetAll<T>(Type curentType)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            var resul = await _dBConnection.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE TenantID = '{user.TenantID}';", commandType: CommandType.Text);
            return resul;
        }
        //Hàm paging
        public async Task<object> Paging<T>(PagingRequest pagingRequest, Type curentType)
        {
            var startIndex = (pagingRequest.PageIndex - 1) * pagingRequest.PageSize + 1;
            var endIndex = pagingRequest.PageIndex * pagingRequest.PageSize;
            string where = string.IsNullOrEmpty(pagingRequest.Filter) ? "" : pagingRequest.Filter + "AND";
            string commandText = $"SELECT * FROM (SELECT * , row_number() OVER (order by {curentType.Name}ID) AS row_num FROM {_tableName}) t WHERE {where} row_num BETWEEN {startIndex} AND {endIndex} ORDER BY {curentType.Name}ID;";
            var resul = await _dBConnection.QueryAsync<T>(commandText, commandType: CommandType.Text);
            return resul;
        }
        //Hàm get id
        public async Task<object> GetByID<T>(string id, Type curentType, string tablename = null)
        {
            if(tablename != null)
            {
                _tableName = tablename;
            }
            var resul = await _dBConnection.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE {curentType.Name}ID = '{id}';", commandType: CommandType.Text);
            return resul.FirstOrDefault();
        }
        //Hàm insert chung
        public async Task<object> Insert(object param, Type curentType)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            BeforeSave(param);
            var parameters = GetParameters(param, curentType);
            parameters.Add("v_TenantID", user.TenantID);
            var resul =  await _dBConnection.ExecuteScalarAsync<object>($"Proc_{curentType.Name}_Insert", parameters, commandType: CommandType.StoredProcedure);
            if(resul != null)
            {
                AfterSave(param);
            }
            return resul;
        }
        public virtual void BeforeSave(object param)
        {

        }
        public virtual void AfterSave(object param)
        {

        }
        //Hàm sửa chung
        public async Task<object> Update(object param, Type curentType)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            BeforeSave(param);
            var parameters = GetParameters(param, curentType);
            parameters.Add("v_TenantID", user.TenantID);
            var resul = await _dBConnection.ExecuteAsync($"Proc_{curentType.Name}_Update", parameters, commandType: CommandType.StoredProcedure);
            return resul;
        }
        //Hàm sửa danh sách
        public async Task<object> SaveList<T>(List<T> param, Type curentType)
        {
            foreach(var item in param)
            {
                var state = curentType.GetProperties().Where(x => x.Name == "State").FirstOrDefault();
                if (state != null && int.Parse(state.GetValue(item).ToString()) == 2)
                {
                    await Update(item, curentType);
                }
                else
                {
                    await Insert(item, curentType);
                }
            };
            return param;
        }
        //Hàm xóa chung
        public async Task<object> Delete(string id, Type curentType)
        {
            //Lấy ra model trước khi xóa
            var model = await GetByID<object>(id, curentType);
            if (model == null)
            {
                return null;
            }
            string commanText = $"DELETE FROM {_tableName} WHERE {curentType.Name}ID = '{id}';";
            var resul = await _dBConnection.ExecuteAsync(commanText, commandType: CommandType.Text);
            AfterDelete(model);
            return resul;
        }
        /// <summary>
        /// Hàm xử lý sau khi xóa
        /// </summary>
        /// <returns></returns>
        public virtual async Task AfterDelete(object model)
        {

        }
        //Hàm xóa danh sách id
        public async Task<object> DeleteList(List<string> listID, Type curentType)
        {
            foreach (var id in listID)
            {
                await Delete(id, curentType);
            };
            return true;
        }
        //Hàm tách param vào stored
        public DynamicParameters GetParameters(object param, Type curentType)
        {
            var parameters = new DynamicParameters();
            var properties = curentType.GetProperties();
            Type typeParam = param.GetType();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyType = property.PropertyType;
                var propertyValue = property.GetValue(param);
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    parameters.Add($"v_{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    parameters.Add($"v_{propertyName}", propertyValue);
                }
            }
            return parameters;
        }
        public async Task<object> QueryCommandTextAsync<T>(string sql)
        {
            var resul = await _dBConnection.QueryAsync<T>(sql, commandType: CommandType.Text);
            return resul.FirstOrDefault();
        }
        public async Task<object> QueryCommandTexListtAsync<T>(string sql)
        {
            var resul = await _dBConnection.QueryAsync<T>(sql, commandType: CommandType.Text);
            return resul;
        }
        public async Task<object> ExecuteAsync(string sql)
        {
            var resul = await _dBConnection.ExecuteAsync(sql, commandType: CommandType.Text);
            return resul;
        }
        //Hàm xử lý trả về khi lỗi
        public ServiceResponse Error(Exception e)
        {
            ServiceResponse res = new ServiceResponse();
            res.Success = false;
            res.Data = null;
            res.Code = 99;
            res.Message = e.Message;
            return res;
        }
        public async Task<object> QueryAsync<T>(string sql)
        {
            return await _dBConnection.QueryAsync<T>(sql);
        }
    }
}
