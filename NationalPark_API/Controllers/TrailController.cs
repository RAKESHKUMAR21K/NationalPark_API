using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalPark_API.Models;
using NationalPark_API.Models.DTOs;
using NationalPark_API.Repository.IRepository;

namespace NationalPark_API.Controllers
{
    [Route("api/Trail")]
    [ApiController]
    [Authorize]
    public class TrailController : Controller
    {
        private readonly ITrailRepository _TrailRepository;
        private readonly IMapper _Mapper;
        public TrailController(ITrailRepository trailRepository, IMapper mapper)
        {
            _TrailRepository = trailRepository;
            _Mapper = mapper;
        }
        [HttpGet]//Display
        public IActionResult GetTrails()
        {
            var trailDto = _TrailRepository.GetTrails().Select(_Mapper.Map<Trail, TrailDto>);
            return Ok(trailDto);//200
        }
        [HttpGet("{trailId:int}",Name = "GetTrails")]//Find
        public IActionResult GetTrail(int trailId)
        {
            var trail= _TrailRepository.GetTrail(trailId);
            if(trail==null)return NotFound();
            var trailDto = _Mapper.Map<TrailDto>(trail);
            return Ok(trailDto);
        }
        [HttpPost]
        public IActionResult CreateTrail([FromBody]TrailDto trailDto)
        {
            if (trailDto == null) return BadRequest();
            if (_TrailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail in Db !!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var trail = _Mapper.Map<TrailDto, Trail>(trailDto);
            if (!_TrailRepository.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"Somthing went wrong while save data:{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();//200
        }
        [HttpPut]
        public IActionResult UpdateTrail([FromBody]TrailDto trailDto)
        {
            if(trailDto == null)return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var trail = _Mapper.Map<Trail>(trailDto);
            if(!_TrailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Somthing went wrong while Update data:{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_TrailRepository.TrailExists(trailId)) return NotFound();
            var trail=_TrailRepository.GetTrail(trailId);
            if(trail == null) return NotFound();
            if(!_TrailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("", $"Somthing went wrong while Delete data:{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();//200
        }
    }
}
