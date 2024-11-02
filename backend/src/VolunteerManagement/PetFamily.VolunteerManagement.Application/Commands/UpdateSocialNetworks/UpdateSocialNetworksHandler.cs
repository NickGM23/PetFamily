
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateSocialNetworks
{
    public class UpdateSocialNetworksHandler : ICommandHandler<Guid, UpdateSocialNetworksCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IVolunteerUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateSocialNetworksCommand> _validator;
        private readonly ILogger<UpdateSocialNetworksHandler> _logger;

        public UpdateSocialNetworksHandler(
            IVolunteersRepository repository,
            IVolunteerUnitOfWork unitOfWork,
            IValidator<UpdateSocialNetworksCommand> validator,
            ILogger<UpdateSocialNetworksHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateSocialNetworksCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            List<SocialNetwork> socialNetworks = [];
            foreach (var socialNetwork in command.SocialNetworks)
            {
                socialNetworks.Add(SocialNetwork.Create(socialNetwork.Name, socialNetwork.Link).Value);
            }

            var socialNetworksToUpdate = new SocialNetworkList(socialNetworks);

            volunteerResult.Value.UpdateSocialNetworks(socialNetworksToUpdate);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Social medias of volunteer with {id} has been updated", command.VolunteerId);

            return command.VolunteerId;
        }
    }
}
