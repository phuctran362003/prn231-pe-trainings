using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.Common;
using Service.Interfaces;

namespace PE_PRN232_SU25_SE172360_api.Controllers;

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

    [HttpGet("search")]
    [EnableQuery]
    [Authorize(Roles = "1,2,3")]
    public async Task<IActionResult> Search(string? name, int? categoryId)
    {
        try
        {
            var result = await _service.SearchAsync(name, categoryId);
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
        try
        {
            var result = await _service.CreateWithValidation(dto);

            return Ok(new
            {
                Message = "Create successful",
                Data = result
            });
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("validation", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new ErrorResult("HB40001", ex.Message));

            return StatusCode(500, new ErrorResult("HB50001", "Internal server error"));
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> Put(int id, UpdateProductDto dto)
    {
        try
        {
            dto.ProductId = id;

            var result = await _service.UpdateWithValidation(dto);

            return Ok(new
            {
                Message = "Update successful",
                Data = result
            });
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFound(new ErrorResult("HB40401", ex.Message));

            return StatusCode(500, new ErrorResult("HB50001", ex.Message));
        }
    }
}