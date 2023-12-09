using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;

namespace UserService.Application.Interests.Command
{
    public record DeleteInterestCommand(DeleteInterestDto Dto) : IRequest<bool>;

    public class DeleteInterestCommandHanlder : IRequestHandler<DeleteInterestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInterestCommandHanlder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteInterestCommand request, CancellationToken cancellationToken)
        {
            var interest = await _unitOfWork.InterestRepository.GetFirstOrDefaultAsync(x => x.Id == request.Dto.Id);

            if (interest is null)
            {
                throw new InvalidOperationException("Interest not found");
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
