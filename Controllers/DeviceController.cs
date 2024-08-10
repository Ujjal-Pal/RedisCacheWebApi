using Microsoft.AspNetCore.Mvc;
using RedisCacheWebApi.Models;
using RedisCacheWebApi.Services;

namespace RedisCacheWebApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IRedisCacheService _redisCacheService;
        public DeviceController(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }
        
        [HttpGet("GetDevice")]
        public async Task<IActionResult> Get()
        {
            //Checking Cache Data
            var cacheData = _redisCacheService.GetData<IEnumerable<Device>>("device");

            if(cacheData != null || cacheData.Count() > 0)
            {
                return Ok(cacheData);
            }
            return NoContent();
        }

        [HttpGet("GetDevice/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cacheData = _redisCacheService.GetData<Device>($"device{id}");

            if(cacheData != null)
            {
                return Ok(cacheData);
            }

            return NoContent();
        }

        [HttpPost("AddDevice")]
        public async Task<IActionResult> SaveNewValue(Device value)
        {
            var exptime = DateTime.Now.AddSeconds(20);
            var deviceData = _redisCacheService.SetData<Device>($"device{value.id}",value,exptime);

            return Ok(value);
        }

        // [HttpPut]
        // public void UpdateValue(int id, [FromBody]string value)
        // {
        // }

        [HttpDelete("RemoveDevice")]
        public async Task<IActionResult> RemoveValue(int id)
        {
            var data = _redisCacheService.RemoveData($"device{id}");
            if(data)
            {
                return Ok("Device Removed Successfully");
            }
            return NotFound();
        }
    }
}