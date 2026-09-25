using CustomerServiceCampaign.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServiceCampaign.Api.Controllers
{
    // Osnovna ruta ovog controllera biće /api/person.
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        // Čuvamo referencu na PersonService koji će controller koristiti.
        private readonly PersonService _personService;

        // ASP.NET Core preko Dependency Injection-a automatski
        // prosleđuje PersonService koji smo registrovali u Program.cs.
        public PersonController(PersonService personService)
        {
            _personService = personService;
        }

        // GET /api/person/{id}
        // Na primer: GET /api/person/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(string id)
        {
            // Pozivamo naš PersonService, koji zatim poziva SOAP FindPerson.
            var person = await _personService.FindPersonAsync(id);

            // Ako SOAP servis ne vrati osobu, vraćamo HTTP 404 Not Found.
            if (person == null)
            {
                return NotFound();
            }

            // Ako je osoba pronađena, vraćamo HTTP 200 OK
            // zajedno sa podacima o osobi.
            return Ok(person);
        }
    }
}