using MerchantService.Base;
using MerchantService.Models;
using MerchantService.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MerchantService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAssignmentsController : BaseController <AuthAssignment, AuthAssignmentRepository, int>
    {
        public AuthAssignmentsController(AuthAssignmentRepository authAssigmentRepository) : base(authAssigmentRepository) { }
    }
}
