using MerchantService.Context;
using MerchantService.Models;

namespace MerchantService.Repositories.Data
{
    public class AuthItemChildRepository : GeneralRepository<MerchantContext, AuthItemChild, int>
    {
        public AuthItemChildRepository(MerchantContext merchantContext) : base(merchantContext) { }

    }
}
