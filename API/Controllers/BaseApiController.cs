using System;
using System.Text.Json;
using System.Threading.Tasks;
using API.Extensions.Filters;
using API.Services;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ServiceFilter(typeof(UserActionFilter))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected readonly IUserAccessor _userAccessor;
        private readonly UserService _userService;
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public BaseApiController(IUserAccessor userAccessor, 
        UserService userService
        )
        {
            _userAccessor = userAccessor;
            _userService = userService;
            
        }

        protected async Task CreateCurrentUserSettins()
        {
            var us = await _userService.CreateUserSettings();
        }
        

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }

        protected IActionResult ModelErrorHandleResult<T>(ModelErrorResult<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.ModelErrors);
        }
        protected IActionResult HandleJsonResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
            {
                string jsonString = JsonSerializer.Serialize(result.Value);
                var date = DateTime.Now;
                var dateString = date.Month.ToString() + "-" + date.Day.ToString() + "-" + date.Year.ToString();
                string fileName = "concreteDataJson-" + dateString;

                HttpContext.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                HttpContext.Response.Headers.Add("content-disposition", "attachment; filename=" + fileName + ".json");

                return new JsonResult(result.Value);
            }

            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }
        protected IActionResult HandleExelResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
            {
                string jsonString = JsonSerializer.Serialize(result.Value);
                var date = DateTime.Now;
                var dateString = date.Month.ToString() + "-" + date.Day.ToString() + "-" + date.Year.ToString();
                string fileName = "concreteDataJson-" + dateString;

                HttpContext.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                HttpContext.Response.Headers.Add("content-disposition", "attachment; filename=" + fileName + ".json");

                return new JsonResult(result.Value);
            }

            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }


    }
}