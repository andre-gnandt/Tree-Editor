using Microsoft.AspNetCore.Mvc;
using LocalTreeData.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LocalTreeData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigTypesController : ControllerBase
    {
        private readonly EfCore.AppContext _context;

        public ConfigTypesController(EfCore.AppContext context)
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
                throw new NotImplementedException("No Database Record Configuration! Missing the configuration of the country-region JSON data in the ConfigTypes table. See 'SQLDataBaseRecordConfigs' repository on my github for the insertion query.");
            }
     
            return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(countries.Value);
        }

    }
}

