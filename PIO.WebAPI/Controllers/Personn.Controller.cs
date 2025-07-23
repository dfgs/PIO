using Microsoft.AspNetCore.Mvc;
using PIO.Models;

namespace PIO.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonnController : PIOController
    {
        
  
        public PersonnController(LogLib.ILogger Logger) : base(Logger) 
        {
        }

        [HttpGet(Name = "Personn")]
        public IEnumerable<Personn> Get()
        {
            LogEnter();

            return Enumerable.Range(1, 5).Select(index => new Personn()
            {
                PersonnID=(byte)index,
                FirstName=$"FirstName{index}",
                LastName=$"LastName{index}"
            })
            .ToArray();
        }
    }
}
