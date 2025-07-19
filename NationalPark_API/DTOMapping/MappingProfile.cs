using AutoMapper;
using NationalPark_API.Models;
using NationalPark_API.Models.DTOs;

namespace NationalPark_API.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<TrailDto, Trail>().ReverseMap();
        }
    }
}
//DB --- MODEL --- REPOSITORY --- DTO --- CLIENT
//CLIENT --- DTO --- REPOSITORY --- MODEL --- DB
