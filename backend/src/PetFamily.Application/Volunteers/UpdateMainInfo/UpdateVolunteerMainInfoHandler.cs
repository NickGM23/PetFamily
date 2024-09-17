
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public class UpdateVolunteerMainInfoHandler
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<UpdateVolunteerMainInfoCommand> _validator;
        private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger;

        public UpdateVolunteerMainInfoHandler(
            IVolunteersRepository repository,
            IValidator<UpdateVolunteerMainInfoCommand> validator,
            ILogger<UpdateVolunteerMainInfoHandler> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateVolunteerMainInfoCommand command,
            CancellationToken cancellationToken)
        {

            var validationResult1 = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult1.IsValid == false)
            {
                return validationResult1.ToList();
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

            await _repository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Updated {Volunteer} with id {Id}", volunteerResult.Value, command.VolunteerId);

            return command.VolunteerId;
        }
    }
}
