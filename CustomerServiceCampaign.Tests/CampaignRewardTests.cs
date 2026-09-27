using CustomerServiceCampaign.Api.Models;

namespace CustomerServiceCampaign.Tests
{
    // Testovi vezani za CampaignReward model.
    public class CampaignRewardTests
    {
        // Fact označava metodu koju xUnit prepoznaje i izvršava kao test.
        [Fact]
        // Proverava da je PurchaseSuccessful kod nove nagrade
        // podrazumevano postavljen na false.
        public void CampaignReward_PurchaseSuccessful_DefaultValueIsFalse()
        {
            // Arrange & Act:
            // Kreiramo novu nagradu koju ćemo testirati.
            var reward = new CampaignReward();

            // Assert:
            // Proveravamo da PurchaseSuccessful ima očekivanu vrednost false.
            Assert.False(reward.PurchaseSuccessful);
        }
    }
}