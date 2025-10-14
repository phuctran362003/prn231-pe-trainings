using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repository.Entities;
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

        [HttpGet]
        [Authorize(Roles = "1,2,3")]
        [EnableQuery]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1,2,3")]
        public async Task<Product> Get(int id)
        {
            return await _service.GetById(id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1,2")]
        public async Task<bool> Delete(int id)
        {
            return await _service.Delete(id);
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