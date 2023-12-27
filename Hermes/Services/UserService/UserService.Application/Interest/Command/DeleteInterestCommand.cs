using MediatR;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Interests.Command
{
    public record DeleteInterestCommand(Guid Id) : IRequest<bool>;

    public class DeleteInterestCommandHandler : IRequestHandler<DeleteInterestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInterestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteInterestCommand request, CancellationToken cancellationToken)
        {
            var interest = await _unitOfWork.InterestRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id);

            if (interest is null)
            {
                throw new NotFoundException("Interest not found");
            }
            else
            {
                await _unitOfWork.InterestRepository.DeleteAsync(interest.Id);

                await _unitOfWork.CompleteAsync();

                return true;
            }
        }
    }
}
