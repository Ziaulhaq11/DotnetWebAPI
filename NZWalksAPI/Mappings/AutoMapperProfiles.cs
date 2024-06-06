using AutoMapper;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Mappings
{
    public class AutoMapperProfiles : Profile //It need this type which comes from Automapper
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap(); //It also work for List of these types
            CreateMap<AddRegionDto, Region>().ReverseMap();
            CreateMap<UpdateRegionDto, Region>().ReverseMap();
            /*This will work if properties names are similar
            //CreateMap<UserDTO, UserDomain>(); //With just this we can do one way of Automapping.
            //CreateMap<UserDTO, UserDomain>().ReverseMap(); //You can do this reverse as well now.

            //Now we have different property names. So we do like this
            //x.UserName is Destination. MapFrom x.FullName is the source
            //CreateMap<UserDTO, UserDomain>().ForMember(x => x.UserName, opt => opt.MapFrom(x => x.FullName)).ReverseMap();*/
            CreateMap<AddWalkRequestDTO, Walk>().ReverseMap();
            CreateMap<WalkDto, Walk>().ReverseMap();    
        }
    }
    /*public class UserDTO
    {
        public string FullName { get; set; }
    }
    public class UserDomain
    {
        public string UserName { get; set; }
    }*/
}
