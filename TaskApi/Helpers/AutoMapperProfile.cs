using AutoMapper;
using TaskApi.DTOs;
using TaskApi.Entities;

namespace TaskApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TaskDto, TaskEntity>();
        }
    }
}
