using CustomerServiceCampaign.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceCampaign.Api.Data
{
    public class CampaignDbContext : DbContext
    {
        public CampaignDbContext(DbContextOptions<CampaignDbContext> options)
            : base(options)
        {
        }

        public DbSet<CampaignReward> CampaignRewards { get; set; }
    }
}