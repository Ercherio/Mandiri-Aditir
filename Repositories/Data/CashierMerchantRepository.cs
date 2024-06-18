using MerchantService.Context;
using MerchantService.Models;

namespace MerchantService.Repositories.Data
{
    public class CashierMerchantRepository : GeneralRepository<MerchantContext, CashierMerchant, int>
    {
        public CashierMerchantRepository (MerchantContext merchantContext) : base(merchantContext) { }
    }
}
