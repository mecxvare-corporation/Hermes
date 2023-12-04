using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record CreateUserCommand(CreateUserDto Dto) : IRequest<Guid>;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.Dto);

            _unitOfWork.UserRepository.Create(user);

            await _unitOfWork.CompleteAsync();

            return user.Id;
        }
    }

}
