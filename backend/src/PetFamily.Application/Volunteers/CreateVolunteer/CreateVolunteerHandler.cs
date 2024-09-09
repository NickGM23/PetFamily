﻿
using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<CreateVolunteerRequest> _validator;

        public CreateVolunteerHandler(IVolunteersRepository repository, IValidator<CreateVolunteerRequest> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<Result<Guid, ErrorList>>  Handle(CreateVolunteerRequest request,
            CancellationToken cancellationToken = default)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToList();
            }

            var volunteerId = VolunteerId.NewVolunteerId();

            var fullNameResult = FullName.Create(request.FullName.LastName,
                request.FullName.FirstName, request.FullName.Patronymic).Value;

            var emailResult = Email.Create(request.Email).Value;

            var descriptionResult = Description.Create(request.Description).Value;

            var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber).Value;

            SocialNetworkList socialNetworksList = new(request.SocialNetworksDTO.
                Select(nw => SocialNetwork.Create(nw.Name, nw.Link).Value));

            RequisiteList requisitesList = new(request.RequisitesDTO.
                Select(r => Requisite.Create(r.Name, r.Description).Value));

            var volunteerResult = Volunteer.Create(volunteerId, fullNameResult,
                emailResult, descriptionResult, request.YearsOfExperience,
                phoneNumberResult, socialNetworksList, requisitesList);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;

            await _repository.Add(volunteer);

            return (Guid)volunteer.Id;
            
        }
    }
}
