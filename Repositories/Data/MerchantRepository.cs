using MerchantService.Context;
using MerchantService.Models;

namespace MerchantService.Repositories.Data
{
    public class MerchantRepository : GeneralRepository<MerchantContext, Merchant, String>
    {
        public MerchantRepository(MerchantContext merchantContext) : base(merchantContext) { }
    }
}
