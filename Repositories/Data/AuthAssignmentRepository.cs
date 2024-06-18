using MerchantService.Context;
using MerchantService.Models;

namespace MerchantService.Repositories.Data
{
    public class AuthAssignmentRepository : GeneralRepository<MerchantContext, AuthAssignment, int>
    {
        public AuthAssignmentRepository(MerchantContext merchantContext) : base(merchantContext) { }
    }
}
