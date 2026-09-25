using CustomerServiceCampaign.Api.Data;
using CustomerServiceCampaign.Api.Services;
using Microsoft.AspNetCore.Mvc;
using CustomerServiceCampaign.Api.DataTransferObjects;
using CustomerServiceCampaign.Api.Models;
using Microsoft.EntityFrameworkCore;

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
        public CampaignController(CampaignDbContext context, PersonService personService)
        {
            _context = context;
            _personService = personService;
        }
        // POST /api/Campaign/rewards
        // Dodeljuje nagradu izabranom korisniku.
        [HttpPost("rewards")]
        public async Task<IActionResult> CreateReward(CreateRewardRequest request)
        {
            // Preko eksternog SOAP servisa proveravamo
            // da li korisnik sa prosleđenim ID-em postoji.
            var person = await _personService.FindPersonAsync(
                request.CustomerId.ToString());

            // Ako korisnik ne postoji, nagrada ne može biti dodeljena.
            if (person == null)
            {
                return NotFound("Korisnik nije pronađen.");
            }
            // Određujemo početak i kraj današnjeg dana.
            // Koristimo UTC vreme jer i RewardDate čuvamo kao UTC.
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            // Brojimo koliko je nagrada ovaj agent već dodelio danas.
            var rewardsToday = await _context.CampaignRewards.CountAsync(r =>
                r.AgentId == request.AgentId &&
                r.RewardDate >= today &&
                r.RewardDate < tomorrow);

            // Jedan agent može da nagradi najviše 5 korisnika dnevno.
            if (rewardsToday >= 5)
            {
                return BadRequest("Agent je dostigao dnevni limit od 5 korisnika.");
            }
            // Kreiramo zapis koji će biti sačuvan u našoj bazi.
            var reward = new CampaignReward
            {
                CustomerId = request.CustomerId,
                AgentId = request.AgentId,

                // Datum dodele određuje server, a ne agent.
                RewardDate = DateTime.UtcNow,

                // Za sada ne znamo da li je korisnik obavio kupovinu.
                // Taj podatak ćemo kasnije dobiti iz CSV izveštaja.
                PurchaseSuccessful = false
            };

            // Dodajemo novi zapis u Entity Framework.
            _context.CampaignRewards.Add(reward);

            // Čuvamo promenu u SQLite bazi.
            await _context.SaveChangesAsync();

            // Vraćamo HTTP 200 i kreirani zapis.
            return Ok(reward);
        }
    }
}