using MerchantService.Context;
using MerchantService.Models;

namespace MerchantService.Repositories.Data
{
    public class AuthItemRepository : GeneralRepository<MerchantContext, AuthItem, String>

    {
        public AuthItemRepository(MerchantContext merchantContext) : base(merchantContext)
        {
        }
    }
}
