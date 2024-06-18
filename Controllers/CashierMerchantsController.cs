﻿using MerchantService.Base;
using MerchantService.Models;
using MerchantService.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MerchantService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashierMerchantsController : BaseController<CashierMerchant, CashierMerchantRepository, int>
    {
        public CashierMerchantsController(CashierMerchantRepository cashierMerchantRepository) : base(cashierMerchantRepository) { }
    }
}
