using System.ComponentModel.DataAnnotations;

namespace CustomerServiceCampaign.Api.DataTransferObjects
{
    // Predstavlja podatke koje agent šalje
    // kada želi da dodeli nagradu korisniku.
    public class CreateRewardRequest
    {
        // ID korisnika mora biti veći od 0.
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId mora biti veći od 0.")]
        public int CustomerId { get; set; }

        // ID agenta je obavezan podatak.
        [Required(ErrorMessage = "AgentId je obavezan.")]
        public string AgentId { get; set; } = string.Empty;
    }
}