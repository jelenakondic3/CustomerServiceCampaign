using FindPersonService;

namespace CustomerServiceCampaign.Api.Services
{
    // Definiše operacije koje servis za rad sa korisnicima mora da implementira.
    // Omogućava da u testovima koristimo lažnu implementaciju
    // umesto pozivanja pravog eksternog SOAP servisa.
    public interface IPersonService
    {
        // Pronalazi korisnika preko njegovog ID-a.
        Task<Person?> FindPersonAsync(string id);
    }
}