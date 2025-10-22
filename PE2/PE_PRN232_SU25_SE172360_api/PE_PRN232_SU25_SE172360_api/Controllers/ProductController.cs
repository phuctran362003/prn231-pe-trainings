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
                var error = new ErrorResult("PR50001", "Internal server error");
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
                    var error = new ErrorResult("PR40401", "Resource not found");
                    return NotFound(error);
                }

                return Ok(infor);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("PR50001", $"Internal server error: {ex.Message}");
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
                    var error = new ErrorResult("PR40401", "Resource not found");
                    return NotFound(error);
                }

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("PR50001", "Internal server error");
                return StatusCode(500, error);
            }
        }

        //[HttpGet("search")]
        //[Authorize(Roles = "1,2")]
        //public async Task<IEnumerable<Product>> Get(string? author, int? date)
        //{
        //    return await _service.Search(date, author);
        //}
        //[HttpPost]

        //[Authorize(Roles = "1")]
        //public async Task<IActionResult> Post(Product transaction)
        //{
        //    var result = await _service.CreateWithValidation(transaction);
        //    if (result.Contains("Thêm Thành công"))
        //    {
        //        return Ok(new
        //        {
        //            Message = "Create successful",
        //            Data = result
        //        });
        //    }
        //    return BadRequest(new
        //    {
        //        Message = "Validation failed",
        //        Errors = result
        //    });
        //}

        //[HttpPut()]
        //[Authorize(Roles = "1")]
        //public async Task<IActionResult> Put(Product Product)
        //{
        //    var result = await _service.UpdateWithValidation(transaction);
        //    if (result.Contains("Edit thành công"))
        //    {
        //        return Ok(new
        //        {
        //            Message = "Edit successful",
        //            Data = result
        //        });
        //    }
        //    return BadRequest(new
        //    {
        //        Message = "Validation failed",
        //        Errors = result
        //    });
        //}
    }
}