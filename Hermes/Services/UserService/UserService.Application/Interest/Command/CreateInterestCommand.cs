using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Interests.Command
{
    public record CreateInterestCommand(CreateInterestDto Dto) : IRequest<bool>;
    public class CreateInterestCommandHandler : IRequestHandler<CreateInterestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateInterestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateInterestCommand request, CancellationToken cancellationToken)
        {
            var interest = await _unitOfWork.InterestRepository.GetFirstOrDefaultAsync(x => x.Name.ToLower().Trim().Equals(request.Dto.Name.ToLower().Trim()), true);

            if (interest is null)
            {
                var newInterest = new Interest(request.Dto.Name);

                _unitOfWork.InterestRepository.Create(newInterest);

                await _unitOfWork.CompleteAsync();

                return true;
            }
            else
            {
                throw new InvalidOperationException($"Interest: '{interest.Name}' already exists!");
            }
        }
    }
}
