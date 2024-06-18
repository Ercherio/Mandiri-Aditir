using MerchantService.Context;
using MerchantService.Models;

namespace MerchantService.Repositories.Data
{
    public class LoggingRepository : GeneralRepository<MerchantContext, Logging, int>
    {
        public LoggingRepository(MerchantContext merchantContext) : base(merchantContext) { }
    }
}
