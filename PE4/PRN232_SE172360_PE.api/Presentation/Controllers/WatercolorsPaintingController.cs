using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repo.Entities;
using Service.Interfaces;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatercolorsPaintingController : Controller
    {
        private readonly IWatercolorsPaintingService _watercolorsPaintingService;
        public WatercolorsPaintingController(IWatercolorsPaintingService watercolorsPaintingService)
        {
            _watercolorsPaintingService = watercolorsPaintingService;
        }

        //[HttpGet("search")]
        //[Authorize(Roles = "1,2")]
        //public async Task<IEnumerable<WatercolorsPainting>> Get(string? author, int? date)
        //{
        //    return await _watercolorsPaintingService.Search(date, author);
        //}
        [HttpGet]
        [Authorize(Roles = "1,2")]
        [EnableQuery]
        public async Task<IEnumerable<WatercolorsPainting>> Get()
        {
            try
            {
                return await _watercolorsPaintingService.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all paintings: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1,2")]
        public async Task<WatercolorsPainting> Get(string id)
        {
            try
            {
                return await _watercolorsPaintingService.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting painting by id: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Post(WatercolorsPainting transaction)
        {
            try
            {
                var result = await _watercolorsPaintingService.CreateWithValidation(transaction);
                if (result.Success)
                {
                    return Ok(new
                    {
                        Message = result.Message ?? "Create successful",
                        Data = result
                    });
                }
                return BadRequest(new
                {
                    Message = result.Message ?? "Validation failed",
                    Errors = result.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Error creating painting",
                    Error = ex.Message
                });
            }
        }

        //[HttpPut()]
        //[Authorize(Roles = "1")]
        //public async Task<IActionResult> Put(WatercolorsPainting watercolorsPainting)
        //{
        //    var result = await _watercolorsPaintingService.UpdateWithValidation(transaction);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _watercolorsPaintingService.Delete(id);
                return Ok(new { Success = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Error deleting painting",
                    Error = ex.Message
                });
            }
        }
    }
}
