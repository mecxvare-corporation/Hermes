using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Queries
{
    public record GetInterestsQuery : IRequest<IEnumerable<InterestDto>>;

    public class GetInterestsQueryHandler : IRequestHandler<GetInterestsQuery, IEnumerable<InterestDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetInterestsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InterestDto>> Handle(GetInterestsQuery request, CancellationToken cancellationToken)
        {
            var interests = await _uow.InterestRepository.GetAllAsync();

            if (interests.Count == 0)
            {
                throw new InvalidOperationException("Interests were not found!");
            }

            var interestsDto = interests.Select(x => _mapper.Map<InterestDto>(x));

            return interestsDto;
        }
    }
}
