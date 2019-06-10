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

        #region Public REST Methods

        // GET api/partmaster
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<PartMaster>> Get()
        {
            try
            {
                using (var context = new PenMgmtContext(_appLogger, _appSettingsDataStore))
                {
                    var filteredList = context.PartMasters
                                            .AsEnumerable()
                                            .Where(pm => pm.Deleted == Constants.DB.NotDeleted);
                    return filteredList.ToList();
                }
            }
            catch (System.Exception ex)
            {
                return InternalServerError(nameof(Get), Constants.Message.TitleGetObjects, ex);
            }
        }

        // GET api/partmaster/5
        [HttpGet("{id}", Name = nameof(GetObjectById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PartMaster> GetObjectById(string id)
        {
            string methodName = nameof(GetObjectById), title = Constants.Message.TitleGetObjectById;

            try
            {
                using (var context = new PenMgmtContext(_appLogger, _appSettingsDataStore))
                using (var uow = new UnitOfWork(context))
                {
                    // For testing
                    // throw new Exception($"Testing method {methodName}..................");

                    var result = GetById(uow, id);
                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        return NotFoundError(methodName, title, id);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return InternalServerError(methodName, title, ex);
            }
        }

        // POST api/partmaster
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PartMaster> Create([FromBody] PartMaster value)
        {
            string methodName = nameof(Create), title = Constants.Message.TitleCreateObject;
            List<string> validationFailureMessages;

            try
            {
                var isValid = ValidateRequest(HttpMethods.Post, value, null, out validationFailureMessages);
                if (isValid)
                {
                    // For testing
                    // throw new Exception($"Testing method {nameof(Create)}..................");

                    using (var context = new PenMgmtContext(_appLogger, _appSettingsDataStore))
                    using (var uow = new UnitOfWork(context))
                    {
                        var now = DateTime.Now;

                        // System Properties
                        value.Id = Guid.NewGuid().ToString();
                        value.Deleted = Constants.DB.DeletedDefault;
                        value.CreatedOn = now;
                        value.UpdatedOn = now;

                        uow.PartMasters.Add(value);
                        // Commit the change
                        uow.Complete();

                        // Return the created object
                        var result = GetById(uow, value.Id);
                        return CreatedAtRoute(nameof(GetObjectById), new { id = value.Id }, result);
                    }
                }
                else
                {
                    var errorMessage = GetFlattenedMessage(validationFailureMessages, Constants.Message.ValidationFailed);
                    return BadRequestError(methodName, title, null, errorMessage);
                }
            }
            catch (System.Exception ex)
            {
                return InternalServerError(methodName, title, ex);
            }
        }

        // PUT api/partmaster/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PartMaster> Update(string id, [FromBody] PartMaster value)
        {
            string methodName = nameof(Update), title = Constants.Message.TitleUpdateObject;
            List<string> validationFailureMessages;

            try
            {
                var isValid = ValidateRequest(HttpMethods.Put, value, id, out validationFailureMessages);
                if (isValid)
                {
                    using (var context = new PenMgmtContext(_appLogger, _appSettingsDataStore))
                    using (var uow = new UnitOfWork(context))
                    {
                        var result = uow.PartMasters.SingleById(id, Constants.DB.NotDeleted);
                        if (result != null)
                        {
                            // Only valid (not deleted) and un-committed object can be updated
                            if (result.Deleted == Constants.DB.NotDeleted && result.Committed == Constants.DB.NotCommitted)
                            {
                                var now = DateTime.Now;

                                // Modifiable Properties 
                                result.Description = value.Description;
                                result.Count = value.Count;
                                result.Weight = value.Weight;
                                result.Committed = value.Committed;

                                // System Properties
                                result.UpdatedOn = now;

                                // Commit the changes
                                uow.Complete();

                                // Return the updated object
                                return GetById(uow, value.Id);
                            }
                            else
                            {
                                return BadRequestError(methodName, title, id, Constants.Message.CommittedObjectCantBeModified);
                            }
                        }
                        else
                        {
                            return NotFoundError(methodName, title, id);
                        }
                    }   // End of USING
                }
                else
                {
                    var errorMessage = GetFlattenedMessage(validationFailureMessages, Constants.Message.ValidationFailed);
                    return BadRequestError(methodName, title, id, errorMessage);
                }
            }
            catch (System.Exception ex)
            {
                return InternalServerError(methodName, title, ex);
            }
        }

        // DELETE api/partmaster/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string id)
        {
            string methodName = nameof(Delete), title = Constants.Message.TitleDeleteObject;

            try
            {
                using (var context = new PenMgmtContext(_appLogger, _appSettingsDataStore))
                using (var uow = new UnitOfWork(context))
                {
                    var result = uow.PartMasters.SingleById(id, Constants.DB.NotDeleted);
                    if (result != null)
                    {
                        if (result.Deleted == Constants.DB.NotDeleted)
                        {
                            var now = DateTime.Now;

                            // System Properties
                            // Only Soft delete
                            result.Deleted = Constants.DB.Deleted;
                            result.UpdatedOn = now;

                            uow.Complete();

                            return Ok();
                        }
                        else
                        {
                            return BadRequestError(methodName, title, id, Constants.Message.ObjectAlreadyDeleted);
                        }
                    }
                    else
                    {
                        return NotFoundError(methodName, title, id);
                    }
                }   // End of USING
            }
            catch (System.Exception ex)
            {
                return InternalServerError(methodName, title, ex);
            }
        }

        #endregion //Public REST Methods

        #region Private Methods

        private PartMaster GetById(UnitOfWork uow, string id)
        {
            return uow.PartMasters.SingleById(id, Constants.DB.NotDeleted);
        }

        private bool ValidateRequest(
            string httpMethod,
            PartMaster value,
            object param1,
            out List<string> validationFailureMessages)
        {
            bool result = true;
            validationFailureMessages = new List<string>();

            if (HttpMethods.IsPost(httpMethod))
            {
                if (value.Id != null)
                {
                    result = false;
                    validationFailureMessages.Add(Constants.Message.ValidationFailedIdShouldBeNull);
                }
            }

            if (HttpMethods.IsPut(httpMethod))
            {
                if (value.Id != param1.ToString())
                {
                    result = false;
                    validationFailureMessages.Add(Constants.Message.ValidationFailedIdsShouldMatch);
                }
            }

            //_appLogger.LogError(($"PartMaster Controller::Validate() >> Result = {result}, HttpMethod: {httpMethod}, List Length: {list.Length}.");
            _appLogger.LogError($"PartMasterController::Validate(httpMethod: {httpMethod}, <value>, param1: {param1}) >> Result = {result}.");
            return result;
        }

        private ObjectResult InternalServerError(
            string methodName,
            string title,
            Exception ex)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorMessage = string.Format($"EXCEPTION: PartMasterController::{methodName}() >> StatusCode: {statusCode}, Message: '{ex.Message}'");
            var problemDetail = new ProblemDetails()
            {
                Status = statusCode,
                Instance = HttpContext.Request.Path,
                Title = title,
                Detail = errorMessage
            };

            _appLogger.LogError(errorMessage);
            return new ObjectResult(problemDetail)
            {
                ContentTypes = { Constants.Http.ContentType },
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        private NotFoundObjectResult NotFoundError(
            string methodName,
            string title,
            string id)
        {
            var statusCode = StatusCodes.Status404NotFound;
            var problemDetail = new ProblemDetails()
            {
                Status = statusCode,
                Instance = HttpContext.Request.Path,
                Title = title,
                Detail = string.Format($"PartMaster object with id '{id}' not found.")
            };

            _appLogger.LogError($"PartMasterController::{methodName}('{id}') >> StatusCode: {statusCode}");
            return new NotFoundObjectResult(problemDetail)
            {
                ContentTypes = { Constants.Http.ContentType },
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        private BadRequestObjectResult BadRequestError(
            string methodName,
            string title,
            string id,
            string messageDetail)
        {
            var statusCode = StatusCodes.Status400BadRequest;
            var problemDetail = new ProblemDetails()
            {
                Status = statusCode,
                Instance = HttpContext.Request.Path,
                Title = title,
                Detail = messageDetail
            };

            _appLogger.LogError($"PartMasterController::{methodName}('{id}') >> StatusCode: {statusCode}");
            return new BadRequestObjectResult(problemDetail)
            {
                ContentTypes = { Constants.Http.ContentType },
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        private string GetFlattenedMessage(List<string> messageList, string defaultMessage)
        {
            defaultMessage = defaultMessage ?? "(no default message set)";
            var message = (messageList == null)
                            ? defaultMessage
                            : (
                                    (messageList.Count == 0)
                                        ? defaultMessage
                                        : string.Join(", ", messageList)
                                );

            return message;
        }

        #endregion //Private Methods
    }
}
