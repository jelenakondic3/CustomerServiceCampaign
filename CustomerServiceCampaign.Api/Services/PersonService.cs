// Uvozimo namespace koji je Visual Studio automatski generisao
// na osnovu WSDL-a SOAP servisa.
// U njemu se nalaze Person, SOAPDemoSoapClient, FindPersonAsync itd.
using FindPersonService;

// Namespace naše aplikacije u kome se nalaze servisi.
namespace CustomerServiceCampaign.Api.Services
{
    // Naš servis koji služi kao posrednik između aplikacije
    // i eksternog FindPerson SOAP servisa.
    public class PersonService
    {
        // Metoda prima ID korisnika i asinhrono vraća pronađenu osobu.
        // Task<Person?> znači da će rezultat biti Person,
        // ali može biti i null ako osoba nije pronađena.
        public async Task<Person?> FindPersonAsync(string id)
        {
            // Kreiramo SOAP klijenta preko kog komuniciramo
            // sa eksternim SOAP servisom.
            var client = new SOAPDemoSoapClient();

            // Pozivamo FindPerson operaciju eksternog SOAP servisa.
            // Prosleđujemo ID korisnika i čekamo odgovor asinhrono.
            var person = await client.FindPersonAsync(id);

            // Vraćamo dobijene podatke o osobi ostatku naše aplikacije.
            return person;
        }
    }
}