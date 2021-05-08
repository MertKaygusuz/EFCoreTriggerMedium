using DemoEFTriggerProject.DbClasses;
using DemoEFTriggerProject.Models;
using DemoEFTriggerProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEFTriggerProject.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly DemoService _demoService;
        public DemoController(DemoService demoService)
        {
            _demoService = demoService;
        }

        [HttpPost]
        public async Task UserCreate(UserDto model)
        {
            await _demoService.CreateUserAsync(model);
        }

        [HttpGet]
        public async Task<IEnumerable<Users>> GetTheUsers()
        {
            return await _demoService.GetAllUsersAsync();
        }

        [HttpPost]
        public async Task<string> Login(LoginModel model)
        {
            return await _demoService.GetJwtAsync(model);
        }

        [HttpPost]
        [Authorize]
        public async Task CreateDemoRecord(DemoClassDto model)
        {
            await _demoService.CreateDemoRecordAsync(model);
        }

        [HttpPut]
        [Authorize]
        public async Task UpdateRecordAsync(DemoClassDto model)
        {
            await _demoService.UpdateDemoRecordAsync(model);
        }

        [HttpDelete]
        [Authorize]
        public async Task DeleteDemoRecord([FromForm] int id)
        {
            await _demoService.DeleteDemoRecordAsync(id);
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<DbDemoClass>> GetAllDemoRecords()
        {
            return await _demoService.GetAllDemoRecordsAsync();
        }
    }
}
