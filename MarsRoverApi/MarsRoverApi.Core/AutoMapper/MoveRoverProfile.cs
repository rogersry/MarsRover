using AutoMapper;
using MarsRoverApi.Core.Handlers.CQRS.Command;
using MarsRoverMessages;

namespace MarsRoverApi.Core.AutoMapper
{
    public class MoveRoverProfile : Profile
    {
        public MoveRoverProfile()
        {
            CreateMap<MoveRoverCommandStep, MoveRoverRequestMessageStep>();
        }
    }
}
