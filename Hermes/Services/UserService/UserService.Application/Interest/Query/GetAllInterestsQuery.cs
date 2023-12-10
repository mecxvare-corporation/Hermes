using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;

namespace UserService.Application.Interests.Query
{
    public record GetAllInterestsQuery : IRequest<IEnumerable<InterestDto>>;

    public class GetAllInterestsQueryHandler : IRequestHandler<GetAllInterestsQuery, IEnumerable<InterestDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllInterestsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InterestDto>> Handle(GetAllInterestsQuery request, CancellationToken cancellationToken)
        {
            var interests = await _uow.InterestRepository.GetAllAsync();

            if (interests is null || (interests is not null && interests.Count == 0))
            {
                throw new InvalidOperationException("Interests were not found!");
            }

            var interestsDto = interests.Select(x => _mapper.Map<InterestDto>(x));

            return interestsDto;
        }
    }
}
