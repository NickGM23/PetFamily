using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Application.Commands.RefreshToken;
using PetFamily.Accounts.Application.Commands.Register;
using PetFamily.Accounts.Application.Commands.UpdateAccountRequisites;
using PetFamily.Accounts.Application.Commands.UpdateAccountSocialNetworks;
using PetFamily.Accounts.Application.Commands.UpdateFullName;
using PetFamily.Accounts.Presentation.Requests;
using PetFamily.Core.Dtos;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.SharedKernel;
using System.Threading;

namespace PetFamily.Accounts.Presentation
{
    public class AccountsController : ApplicationController
    {

        [Authorize]
        [HttpPost("atest")]
        public IActionResult CreateIssue()
        {
            return Ok();
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request.ToCommand(), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginUserRequest request,
            [FromServices] LoginUserHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request.ToCommand(), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequest request,
            [FromServices] RefreshTokenHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(new RefreshTokenCommand(request.AccessToken, request.RefreshToken),
                cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();
            return Ok(result.Value);
        }

        [Permission(Permissions.User.UpdateSocialNetworks)]
        [HttpPatch("{userId:guid}/social-links")]
        public async Task<IActionResult> UpdateAccountSocialLinks(
        [FromBody] IEnumerable<SocialNetworkDto> request,
        [FromRoute] Guid userId,
        [FromServices] UpdateAccountSocialNetworksHandler handler,
        CancellationToken cancellationToken)
        {
            var result = await handler.Handle(new UpdateAccountSocialNetworksCommand(userId, request), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }


        [Permission(Permissions.Volunteer.UpdateRequisites)]
        [HttpPatch("{userId:guid}/requisites")]
        public async Task<IActionResult> UpdateVolunteerRequisites(
            [FromBody] IEnumerable<RequisiteDto> request,
            [FromRoute] Guid userId,
            [FromServices] UpdateAccountRequisitesHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(new UpdateAccountRequisitesCommand(userId, request), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }

        [Permission(Permissions.User.UpdateFullName)]
        [HttpPatch("{userId:guid}/full-name")]
        public async Task<IActionResult> UpdateFullName(
            [FromBody] FullNameDto request,
            [FromRoute] Guid userId,
            [FromServices] UpdateFullNameHandler handler,
            CancellationToken cancellationToken
        )
        {
            var result = await handler.Handle(new UpdateFullNameCommand(userId, request), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }
    }
}
