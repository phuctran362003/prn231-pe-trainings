using BOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace PRN231_SU25_SE172295.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheetahProfileController :ODataController
    {
        private readonly ICheetahProfileService _services;
        public CheetahProfileController(ICheetahProfileService cheetahProfileService)
        {
            _services = cheetahProfileService;
        }


        [EnableQuery]
        [Authorize(Policy = "4567")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _services.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("HB50001", "Internal server error");
                return StatusCode(500, error);
            }
        }


        [Authorize(Policy = "4567")]
        [HttpGet("/api/CheetahProfiles/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var infor = await _services.GetById(id);
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

        [Authorize(Policy = "56")]
        [HttpPost("/api/CheetahProfiles")]
        public async Task<IActionResult> Add([FromBody] CheetahProfile cheetahProfile)
        {
            try
            {
                var result = await _services.Add(cheetahProfile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("HB40001", "Missing or invalid input");
                return BadRequest(error);
            }
        }

        [Authorize(Policy = "56")]
        [HttpPut("/api/CheetahProfiles/{id}")]
        public async Task<IActionResult> Update([FromBody] CheetahProfile cheetahProfile, int id)
        {
            try
            {
                var updated = await _services.Update(id, cheetahProfile);
                if (updated == null)
                {
                    var error = new ErrorResult("HB40401", "Resource not found");
                    return NotFound(error);
                }

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                var error = new ErrorResult("HB40001", "Invalid input");
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("HB50001", "Internal server error");
                return StatusCode(500, error);
            }
        }

        [Authorize(Policy = "56")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _services.Delete(id);
                if (deleted == null)
                {
                    var error = new ErrorResult("HB40401", "Resource not found");
                    return NotFound(error);
                }

                return Ok(deleted);
            }
            catch (ArgumentException ex)
            {
                var error = new ErrorResult("HB40001", "Invalid input");
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("HB50001", "Internal server error");
                return StatusCode(500, error);
            }
        }

        [EnableQuery]
        [Authorize(Policy = "1234567")]
        [HttpGet("/api/CheetahProfiles/Search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                var result = await _services.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var error = new ErrorResult("HB50001", "Internal server error");
                return StatusCode(500, error);
            }
        }
    }
}