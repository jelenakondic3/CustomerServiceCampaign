using CustomerServiceCampaign.Api.Controllers;
using CustomerServiceCampaign.Api.Data;
using CustomerServiceCampaign.Api.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using CustomerServiceCampaign.Api.Services;
using FindPersonService;
using Microsoft.AspNetCore.Mvc;


namespace CustomerServiceCampaign.Tests
{
    // Testovi poslovne logike CampaignController-a.
    public class CampaignControllerTests
    {
        // Kreira novu privremenu bazu u memoriji za svaki test.
        // Na taj način testovi ne koriste niti menjaju stvarnu campaign.db bazu.
        private CampaignDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new CampaignDbContext(options);
        }
        // Kreira lažni PersonService za potrebe testiranja.
        // Na ovaj način test ne poziva pravi eksterni SOAP servis.
        private Mock<IPersonService> CreatePersonServiceMock()
        {
            return new Mock<IPersonService>();
        }
        [Fact]
        public async Task CreateReward_WhenAgentAlreadyHasFiveRewards_ReturnsBadRequest()
        {
            // Arrange:
            // Kreiramo privremenu InMemory bazu za ovaj test.
            using var context = CreateContext();

            // Dodajemo 5 nagrada koje je isti agent već dodelio danas.
            for (int i = 1; i <= 5; i++)
            {
                context.CampaignRewards.Add(new CampaignReward
                {
                    CustomerId = i,
                    AgentId = "AGENT01",
                    RewardDate = DateTime.UtcNow,
                    PurchaseSuccessful = false
                });
            }

            // Čuvamo pripremljene podatke u testnoj bazi.
            await context.SaveChangesAsync();

            // Proveravamo da je zaista upisano 5 nagrada.
            var rewardsToday = await context.CampaignRewards.CountAsync();

            Assert.Equal(5, rewardsToday);
            // Kreiramo lažni PersonService.
            var personServiceMock = CreatePersonServiceMock();

            // Podešavamo mock da se ponaša kao da je korisnik pronađen.
            personServiceMock
                .Setup(service => service.FindPersonAsync("6"))
                .ReturnsAsync(new Person());

            // Kreiramo CampaignController i prosleđujemo mu
            // testnu bazu i lažni PersonService.
            var controller = new CampaignController(
                context,
                personServiceMock.Object);

            // Kreiramo zahtev za šestu nagradu istog agenta.
            var request = new CustomerServiceCampaign.Api.DataTransferObjects.CreateRewardRequest
            {
                CustomerId = 6,
                AgentId = "AGENT01"
            };

            // Act:
            // Pokušavamo da dodelimo šestu nagradu.
            var result = await controller.CreateReward(request);

            // Assert:
            // Očekujemo BadRequest jer je agent već dostigao dnevni limit.
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateReward_WhenAgentHasLessThanFiveRewards_ReturnsOk()
        {
            // Arrange:
            // Kreiramo novu privremenu InMemory bazu.
            using var context = CreateContext();

            // Kreiramo lažni PersonService.
            var personServiceMock = CreatePersonServiceMock();

            // Simuliramo da je korisnik pronađen preko SOAP servisa.
            personServiceMock
                .Setup(service => service.FindPersonAsync("1"))
                .ReturnsAsync(new Person());

            // Kreiramo controller sa testnom bazom i lažnim servisom.
            var controller = new CampaignController(
                context,
                personServiceMock.Object);

            // Agent još nema nijednu nagradu, pa pokušavamo da dodelimo prvu.
            var request = new CustomerServiceCampaign.Api.DataTransferObjects.CreateRewardRequest
            {
                CustomerId = 1,
                AgentId = "AGENT01"
            };

            // Act:
            // Pozivamo metodu za dodelu nagrade.
            var result = await controller.CreateReward(request);

            // Assert:
            // Očekujemo HTTP 200 OK.
            Assert.IsType<OkObjectResult>(result);

            // Proveravamo da je nagrada zaista sačuvana u testnoj bazi.
            var rewardsCount = await context.CampaignRewards.CountAsync();

            Assert.Equal(1, rewardsCount);
        }
    }
}