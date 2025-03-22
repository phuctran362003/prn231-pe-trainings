﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repository.Entities;
using Service.Interface;

namespace PE_PRN231_SP24_123890_SE172360_BE.Controllers
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

        [HttpGet("search")]
        [Authorize(Roles = "1,2")]
        public async Task<IEnumerable<WatercolorsPainting>> Get(string? author, int? date)
        {
            return await _watercolorsPaintingService.Search(date, author);
        }
        [HttpGet]
        [Authorize(Roles = "1,2")]
        [EnableQuery]
        public async Task<IEnumerable<WatercolorsPainting>> Get()
        {
            return await _watercolorsPaintingService.GetAll();
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "1,2")]

        public async Task<WatercolorsPainting> Get(string id)
        {
            return await _watercolorsPaintingService.GetById(id);
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Post(WatercolorsPainting transaction)
        {
            var result = await _watercolorsPaintingService.CreateWithValidation(transaction);
            if (result.Contains("Thêm Thành công"))
            {
                return Ok(new
                {
                    Message = "Create successful",
                    Data = result
                });
            }
            return BadRequest(new
            {
                Message = "Validation failed",
                Errors = result
            });
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
        public async Task<bool> Delete(string id)
        {
            return await _watercolorsPaintingService.Delete(id);
        }
    }
}
