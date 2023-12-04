using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record UpdateUserCommand(UpdateUserDto Dto) : IRequest<Guid>;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Id == request.Dto.Id, true);

            if(user == null)
            {
                throw new InvalidOperationException("User was not found!");
            }

            user.Update(request.Dto.FirstName, request.Dto.LastName, request.Dto.DateOfBirth);

            await _unitOfWork.CompleteAsync();

            return user.Id;
        }
    }
}
