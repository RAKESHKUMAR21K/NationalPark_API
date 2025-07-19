using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalPark_API.Models;
using NationalPark_API.Models.DTOs;
using NationalPark_API.Repository.IRepository;
using System.Resources;

namespace NationalPark_API.Controllers
{
    [Route("api/NationalPark")]
    [ApiController]
    [Authorize]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _NationalParkRepository;
        private readonly IMapper _Mapper;
        public NationalParkController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _NationalParkRepository = nationalParkRepository;
            _Mapper = mapper;
        }
        [HttpGet]//Display
        public IActionResult GetNationalParks()
        {
            var nationalParkDto = _NationalParkRepository.GetNationalParks().
                Select(_Mapper.Map<NationalPark, NationalParkDto>);
            return Ok(nationalParkDto);//200
        }
        [HttpGet("{nationalparkId:int}", Name = "GetNationalParks")]//Find
        public IActionResult GetNationalPark(int nationalparkId)
        {
            var nationalPark = _NationalParkRepository.GetNationalPark(nationalparkId);
            if (nationalPark == null) return NotFound();//404
            var nationalParkDto = _Mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalParkDto);//200
        }
        [HttpPost]//Save
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest();//400
            if (_NationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "NationalPark in Db !!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();//400
            var nationalPark = _Mapper.Map<NationalParkDto, NationalPark>(nationalParkDto);
            if (!_NationalParkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Somthing went wrong while save data:{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();//200
        }
        [HttpPut]//Update
        public IActionResult UpdateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var nationalPark = _Mapper.Map<NationalPark>(nationalParkDto);
            if (!_NationalParkRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Somthing went wrong while Update data:{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();//204
        }
        [HttpDelete("{nationalParkId:int}")]//Delete
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if(!_NationalParkRepository.NationalParkExists(nationalParkId))return NotFound();
            var nationalPark=_NationalParkRepository.GetNationalPark(nationalParkId);
            if(nationalPark == null) return NotFound();
            if(!_NationalParkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Somthing went wrong while Delete data:{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();//200
        }
    }
}
