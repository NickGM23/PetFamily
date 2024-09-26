
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.VolunteersManagement;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteersRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateVolunteerCommand> _validator;
        private readonly ILogger<CreateVolunteerHandler> _logger;

        public CreateVolunteerHandler(IVolunteersRepository repository,
            IUnitOfWork unitOfWork,
            IValidator<CreateVolunteerCommand> validator,
            ILogger<CreateVolunteerHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>>  Handle(CreateVolunteerCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var volunteerId = VolunteerId.NewVolunteerId();

            var fullNameResult = FullName.Create(command.FullName.LastName,
                command.FullName.FirstName, command.FullName.Patronymic).Value;

            var emailResult = Email.Create(command.Email).Value;

            var descriptionResult = Description.Create(command.Description).Value;

            var yearsExperience = YearsExperience.Create(command.YearsExperience).Value;

            var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber).Value;

            SocialNetworkList socialNetworksList = new(command.SocialNetworks.
                Select(nw => SocialNetwork.Create(nw.Name, nw.Link).Value));

            RequisiteList requisitesList = new(command.Requisites.
                Select(r => Requisite.Create(r.Name, r.Description).Value));

            var volunteerResult = Volunteer.Create(volunteerId, fullNameResult,
                emailResult, descriptionResult, yearsExperience,
                phoneNumberResult, socialNetworksList, requisitesList);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;

            await _repository.Add(volunteer);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Created Volunteer {FullName} with id {VolunteerId}", fullNameResult, volunteer.Id);

            return (Guid)volunteer.Id;
            
        }
    }
}
