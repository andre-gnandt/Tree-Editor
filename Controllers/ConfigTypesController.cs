using Microsoft.AspNetCore.Mvc;
using LocalTreeData.Models;
using LocalTreeData.Dtos;
using LocalTreeData.Application;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LocalTreeData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigTypesController : ControllerBase
    {
        private readonly NodeContext _context;

        public ConfigTypesController(NodeContext context)
        {
            _context = context;
        }

        [HttpGet("Countries")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetCountries()
        {
            string CountryJSONString = (await _context.ConfigTypes.FirstAsync(configType => configType.Name == "Countries")).Value;        
            return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(CountryJSONString);
        }

    }
}

