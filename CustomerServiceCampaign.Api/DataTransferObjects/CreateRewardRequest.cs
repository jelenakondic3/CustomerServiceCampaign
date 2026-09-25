namespace CustomerServiceCampaign.Api.DataTransferObjects
{
    // Predstavlja podatke koje agent šalje
    // kada želi da dodeli nagradu korisniku.
    public class CreateRewardRequest
    {
        // ID korisnika kome se dodeljuje nagrada.
        public int CustomerId { get; set; }

        // ID agenta koji dodeljuje nagradu.
        public string AgentId { get; set; } = string.Empty;
    }
}