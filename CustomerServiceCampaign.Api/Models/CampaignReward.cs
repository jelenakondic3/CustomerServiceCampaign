namespace CustomerServiceCampaign.Api.Models
{
    public class CampaignReward
    {
            public int Id { get; set; }

            public int CustomerId { get; set; }

            public string AgentId { get; set; } = string.Empty;

            public DateTime RewardDate { get; set; }

            public bool PurchaseSuccessful { get; set; }
    }
}
