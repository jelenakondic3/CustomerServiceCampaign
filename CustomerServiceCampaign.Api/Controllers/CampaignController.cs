using CustomerServiceCampaign.Api.Data;
using CustomerServiceCampaign.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServiceCampaign.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        // Omogućava controlleru pristup bazi podataka.
        private readonly CampaignDbContext _context;

        // Omogućava controlleru da proveri korisnika
        // preko eksternog FindPerson SOAP servisa.
        private readonly PersonService _personService;

        // ASP.NET Core preko Dependency Injection-a
        // automatski prosleđuje CampaignDbContext i PersonService
        // konstruktoru CampaignController-a.
        public CampaignController(CampaignDbContext context,PersonService personService)
        {
            _context = context;
            _personService = personService;
        }
    }
}