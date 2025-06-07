using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Models;

namespace Upload.Interface
{
    public interface IBaseBL
    {
        public void SetTableName(string tableName);
        //Hàm get all
        public Task<object> GetAll<T>(Type curentType);
        //Hàm get all
        public Task<object> Paging<T>(PagingRequest pagingRequest, Type curentType);
        //Hàm get id
        public Task<object> GetByID<T>(string id, Type curentType, string table = null);
        //Hàm insert chung
        public Task<object> Insert(object param, Type curentType);
        //Hàm sửa chung
        public Task<object> Update(object param, Type curentType);
        //Hàm sửa nhiều
        public Task<object> SaveList<T>(List<T> param, Type curentType);
        //Hàm xóa chung
        public Task<object> Delete(string id, Type curentType);
        //Hàm xóa danh sách id
        public Task<object> DeleteList(List<string> listID, Type curentType);
        //Hàm query
        public Task<object> QueryCommandTextAsync<T>(string sql);

        //Hàm trả về lỗi Exception
        public ServiceResponse Error(Exception e);
    }
}
