using MerchantService.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MerchantService.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController <Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository: IRepository<Entity, Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;  
        }



        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await repository.GetAll();
            if (result == null)
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "success (empty set)",
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "success",
                    Data = result
                });
            }
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<ActionResult> GetById(Key key)
        {
            try
            {
                var result = await repository.GetById(key);
                if (result == null)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "success (empty set)",
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "success",
                        Data = result
                    });
                }
            }
            catch
            {

                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "failed"
                });
            }

        }

        [HttpPost]
        public async Task<ActionResult> Insert(Entity entity)
        {
            try
            {
                var result = await repository.Insert(entity);
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "failed"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "success"
                    });
                }
            }
            catch (Exception e)
            {

                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = e.Message
                });
            }

        }

        [HttpPut]
        public async Task<ActionResult> Update(Entity entity)
        {
            try
            {
                var result = await repository.Update(entity);
                if (result == 0)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "failed"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "success"
                    });
                }
            }
            catch
            {

                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "failed"
                });
            }
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult> Delete(Key key)
        {
            int? result = await repository.Delete(key);
            if (result == null)
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Massage = "success (no data were found)",
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "success",
                    Data = result
                });
            }
        }


    }
}
