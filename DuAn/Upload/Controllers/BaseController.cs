using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Interface;
using Upload.Models;

namespace Upload.Controllers
{
    public class BaseController<T> : Controller
    {
        public IBaseBL BL = null;
        public Type curentType = null;
        public IBaseBL _baseBL;
        public string _tableName = null;
        public BaseController(IBaseBL baseBL)
        {
            _baseBL = baseBL;
        }
        [HttpGet("get-all")]
        public async Task<ServiceResponse> GetAll()
        {
            try
            {
                ServiceResponse res = new ServiceResponse();
                res.Data = await _baseBL.GetAll<T>(curentType);
                res.Success = true;
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
        }
        [HttpPost("paging")]
        public async Task<ServiceResponse> Paging([FromBody] PagingRequest pagingRequest)
        {
            try
            {
                ServiceResponse res = new ServiceResponse();
                res.Data = await _baseBL.Paging<T>(pagingRequest, curentType);
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
        }
        [HttpGet("getByID/{id}")]
        public async Task<ServiceResponse> GetByID(string id)
        {
            try
            {
                ServiceResponse res = new ServiceResponse();
                res.Data = await _baseBL.GetByID<T>(id, curentType);
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
        }
        [HttpPost("insert")]
        public async Task<ServiceResponse> Insert([FromBody] T param)
        {
            try
            {
                ServiceResponse res = new ServiceResponse();
                res.Data = await _baseBL.Insert(param, curentType);
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
        }
        [HttpPost("update")]
        public async Task<ServiceResponse> Update([FromBody] T param)
        {
            try
            {
                ServiceResponse res = new ServiceResponse();
                res.Data = await _baseBL.Update(param, curentType);
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
        }
        [HttpPost("save-list")]
        public async Task<ServiceResponse> SaveList([FromBody] List<T> param)
        {
            try
            {
                ServiceResponse res = new ServiceResponse();
                res.Data = await _baseBL.SaveList(param, curentType);
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ServiceResponse> Delete(string id)
        {
            try
            {
                ServiceResponse res = new ServiceResponse();
                res.Data = await _baseBL.Delete(id, curentType);
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
        }
        [HttpDelete("list")]
        public async Task<ServiceResponse> DeleteList([FromBody] List<string> listID)
        {
            try
            {
                ServiceResponse res = new ServiceResponse();
                res.Data = await _baseBL.DeleteList(listID, curentType);
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
        }
    }
}
