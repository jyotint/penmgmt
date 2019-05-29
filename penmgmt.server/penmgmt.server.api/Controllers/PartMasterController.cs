using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PenMgmt.Common.Domain;
using PenMgmt.Common.Helper;
using PenMgmt.Server.Api.Configuration;
using PenMgmt.Server.Persistence.Repository;
using PenMgmt.Server.Persistence.UnitOfWork;

namespace PenMgmt.Server.Api.Controllers
{
    // FIXME: Implement Asynchronous processing
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PartMasterController : ControllerBase
    {
        private readonly IAppLogger _appLogger;
        private AppSettingsDataStore _appSettingsDataStore;

        public PartMasterController(
            IAppLogger appLogger,
            IOptions<AppSettingsDataStore> appSettingsDataStore)
        {
            _appLogger = appLogger;
            _appSettingsDataStore = appSettingsDataStore.Value;
        }

        // GET api/partmaster
        [HttpGet]
        public ActionResult<IEnumerable<PartMaster>> Get()
        {
            // const string Message = "PartMasterController::Get()...";
            // System.Diagnostics.Debug.WriteLine(Message);

            List<PartMaster> result = null;

            using (var db = new PenMgmtContext(_appLogger, _appSettingsDataStore))
            {
                result = db.PartMasters.ToList();
            }

            return result;
        }

        // GET api/partmaster/5
        [HttpGet("{id}")]
        public ActionResult<PartMaster> Get(string id)
        {
            PartMaster result = null;

            try
            {
                using (var db = new PenMgmtContext(_appLogger, _appSettingsDataStore))
                using (var uow = new UnitOfWork(db))
                {
                    result = uow.PartMasters.SingleById(id);
                    if (result == null)
                    {
                        HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                        _appLogger.LogError($"PartMasterController::Get('{id}') >> StatusCode: {HttpContext.Response.StatusCode}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                _appLogger.LogError($"EXCEPTION: PartMasterController::Get('{id}') >> StatusCode: {HttpContext.Response.StatusCode}, Message: '{ex.Message}'");
            }

            return result;
        }

        // POST api/partmaster
        [HttpPost]
        public void Post([FromBody] PartMaster value)
        {
            var valid = Validate(HttpMethods.Post, value, null);
            if (valid)
            {
                using (var db = new PenMgmtContext(_appLogger, _appSettingsDataStore))
                using (var uow = new UnitOfWork(db))
                {
                    var now = DateTime.Now;

                    value.Id = Guid.NewGuid().ToString();
                    value.CreatedOn = now;
                    value.UpdatedOn = now;
                    uow.PartMasters.Add(value);
                    uow.Complete();
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                _appLogger.LogError($"PartMasterController::Post() >> StatusCode: {HttpContext.Response.StatusCode}");
            }
        }

        // PUT api/partmaster/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] PartMaster value)
        {
            var valid = Validate(HttpMethods.Put, value, id);
            if (valid)
            {
                using (var db = new PenMgmtContext(_appLogger, _appSettingsDataStore))
                using (var uow = new UnitOfWork(db))
                {
                    var result = uow.PartMasters.SingleById(id);
                    if (result != null)
                    {
                        var now = DateTime.Now;

                        result = value;
                        value.UpdatedOn = now;
                        uow.Complete();
                    }
                    else
                    {
                        HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                        _appLogger.LogError($"PartMasterController::Put('{id}') >> StatusCode: {HttpContext.Response.StatusCode}");
                    }
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                _appLogger.LogError($"PartMasterController::Put('{id}') >> StatusCode: {HttpContext.Response.StatusCode}");
            }
        }

        // DELETE api/partmaster/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            // FIXME: Delete should be soft delete
            using (var db = new PenMgmtContext(_appLogger, _appSettingsDataStore))
            using (var uow = new UnitOfWork(db))
            {
                var result = uow.PartMasters.SingleById(id);
                if (result != null)
                {
                    uow.PartMasters.Remove(result);
                    uow.Complete();
                }
                else
                {
                    _appLogger.LogError($"PartMasterController::Delete('{id}') >> NO ERROR THROWN [Silent Failure] StatusCode: {StatusCodes.Status400BadRequest}");
                }
            }
        }

        private bool Validate(string httpMethod, PartMaster value, object param1)//, params object[] list)
        {
            bool result = false;

            if (HttpMethods.IsPost(httpMethod))
            {
                //result = (list.Length == 0) ? true : false; 
                //result = result && ((value.Id == null) ? true : false);
                result = (value.Id == null) ? true : false;
            }

            if (HttpMethods.IsPut(httpMethod))
            {
                //result = (list.Length > 0) ? true : false; 
                // result = result && ((value.Id == list[0].ToString()) ? true : false);
                result = (value.Id == param1.ToString()) ? true : false;
            }

            //System.Diagnostics.Debug.WriteLine($"PartMaster Controller::Validate() >> Result = {result}, HttpMethod: {httpMethod}, List Length: {list.Length}.");
            System.Diagnostics.Debug.WriteLine($"PartMasterController::Validate(httpMethod: {httpMethod}, <value>, param1: {param1}) >> Result = {result}.");
            return result;
        }
    }
}
