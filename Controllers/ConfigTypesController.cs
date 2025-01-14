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
        
        //This requires a configuration insertion of the country region json into the ConfigTypes table
        //See SQLDataBaseRecordConfigs
        [HttpGet("Countries")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetCountries()
        {
            ConfigType countries = ( await _context.ConfigTypes.AnyAsync(q => q.Name == "Countries")) ? await _context.ConfigTypes.FirstAsync(configType => configType.Name == "Countries") : null;
            if (countries == null) 
            {
                throw new NotImplementedException("No Configuration! Missing the configuration insertion of the country-region JSON data into the ConfigTypes table. See 'SQLDataBaseRecordConfigs' for the insertion query.");
            }
     
            return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(countries.Value);
        }

    }
}

