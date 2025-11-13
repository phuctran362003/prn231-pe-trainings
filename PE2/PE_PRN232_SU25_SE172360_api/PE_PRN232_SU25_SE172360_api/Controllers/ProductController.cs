using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.Common;
using Service.Interfaces;

namespace PE_PRN232_SU25_SE172360_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet()]
        [EnableQuery]
        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("HB50001", "Internal server error");
                return StatusCode(500, error);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var infor = await _service.GetById(id);
                if (infor == null)
                {
                    var error = new ErrorResult("HB40401", "Resource not found");
                    return NotFound(error);
                }

                return Ok(infor);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("HB50001", $"Internal server error: {ex.Message}");
                return StatusCode(500, error);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.Delete(id);
                if (deleted == null)
                {
                    var error = new ErrorResult("HB40401", "Resource not found");
                    return NotFound(error);
                }

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("HB50001", "Internal server error");
                return StatusCode(500, error);
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Post(CreateProductDto dto)
        {
            var result = await _service.CreateWithValidation(dto);
            if (result != null)
            {
                return Ok(new
                {
                    Message = "Create successful",
                    Data = result
                });
            }
            return BadRequest(new
            {
                Message = "Validation failed"
            });
        }
    }
}