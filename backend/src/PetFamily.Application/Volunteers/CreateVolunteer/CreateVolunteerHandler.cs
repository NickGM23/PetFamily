﻿
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<CreateVolunteerRequest> _validator;
        private readonly ILogger<CreateVolunteerHandler> _logger;

        public CreateVolunteerHandler(IVolunteersRepository repository, 
            IValidator<CreateVolunteerRequest> validator,
            ILogger<CreateVolunteerHandler> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>>  Handle(CreateVolunteerRequest request,
            CancellationToken cancellationToken = default)
        {

            var volunteerId = VolunteerId.NewVolunteerId();

            var fullNameResult = FullName.Create(request.FullName.LastName,
                request.FullName.FirstName, request.FullName.Patronymic).Value;

            var emailResult = Email.Create(request.Email).Value;

            var descriptionResult = Description.Create(request.Description).Value;

            var yearsExperience = YearsExperience.Create(request.YearsExperience).Value;

            var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber).Value;

            SocialNetworkList socialNetworksList = new(request.SocialNetworksDto.
                Select(nw => SocialNetwork.Create(nw.Name, nw.Link).Value));

            RequisiteList requisitesList = new(request.RequisitesDto.
                Select(r => Requisite.Create(r.Name, r.Description).Value));

            var volunteerResult = Volunteer.Create(volunteerId, fullNameResult,
                emailResult, descriptionResult, yearsExperience,
                phoneNumberResult, socialNetworksList, requisitesList);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;

            await _repository.Add(volunteer);

            _logger.LogInformation("Created Volunteer {FullName} with id {VolunteerId}", fullNameResult, volunteer.Id);

            return (Guid)volunteer.Id;
            
        }
    }
}
