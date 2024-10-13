
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Application.Commands.AddPet;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateMainInfo
{
    public class UpdateVolunteerMainInfoHandler : ICommandHandler<Guid, UpdateVolunteerMainInfoCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IVolunteerUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateVolunteerMainInfoCommand> _validator;
        private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger;

        public UpdateVolunteerMainInfoHandler(
            IVolunteersRepository repository,
            IVolunteerUnitOfWork unitOfWork,
            IValidator<UpdateVolunteerMainInfoCommand> validator,
            ILogger<UpdateVolunteerMainInfoHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateVolunteerMainInfoCommand command,
            CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
            {
                return validationResult.ToList();
            }

            var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error.ToErrorList();
            }

            var fullName = FullName.Create(command.UpdateVolunteerMainInfoDto.FullNameDto.LastName,
                command.UpdateVolunteerMainInfoDto.FullNameDto.FirstName,
                command.UpdateVolunteerMainInfoDto.FullNameDto.Patronymic).Value;

            var email = Email.Create(command.UpdateVolunteerMainInfoDto.Email).Value;

            var description = Description.Create(command.UpdateVolunteerMainInfoDto.Description).Value;

            var yearsExperience = YearsExperience.Create(command.UpdateVolunteerMainInfoDto.YearsExperience).Value;

            var phoneNumber = PhoneNumber.Create(command.UpdateVolunteerMainInfoDto.PhoneNumber).Value;

            volunteerResult.Value.UpdateMainInfo(
                fullName,
                email,
                description,
                yearsExperience,
                phoneNumber);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Updated {Volunteer} with id {Id}", volunteerResult.Value, command.VolunteerId);

            return command.VolunteerId;
        }
    }
}
