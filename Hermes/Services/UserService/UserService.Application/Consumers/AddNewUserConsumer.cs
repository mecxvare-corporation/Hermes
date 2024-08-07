﻿using AutoMapper;
using MassTransit;
using Messages;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Consumers
{
    public class AddNewUserConsumer : IConsumer<AddNewUser>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddNewUserConsumer(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AddNewUser> context)
        {
            var user = _mapper.Map<User>(context.Message);

            _unitOfWork.UserRepository.Create(user);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                await context.Publish(new AddNewUserFailed { UserId = context.Message.UserId });
                //TODO logging
                throw;
            }
        }
    }
}
