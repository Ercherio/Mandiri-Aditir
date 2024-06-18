using MerchantService.Context;
using MerchantService.Models;

namespace MerchantService.Repositories.Data
{
    public class OwnerMerchantRepository : GeneralRepository<MerchantContext, OwnerMerchant, String>
    {
        public OwnerMerchantRepository(MerchantContext merchantContext) : base(merchantContext) { }
    }
}
