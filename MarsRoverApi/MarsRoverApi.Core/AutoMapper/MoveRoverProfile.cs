using AutoMapper;
using MarsRoverApi.Core.Handlers.CQRS.Command;
using MarsRoverApi.Messages;

namespace MarsRoverApi.Core.AutoMapper
{
    public class MoveRoverProfile : Profile
    {
        public MoveRoverProfile()
        {
            CreateMap<MoveRoverCommand, MoveRoverRequestMessage>();
        }
    }
}
