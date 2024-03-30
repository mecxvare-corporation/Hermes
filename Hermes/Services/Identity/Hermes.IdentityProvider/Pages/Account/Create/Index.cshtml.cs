using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Test;
using Hermes.IdentityProvider.Domain;
using Hermes.IdentityProvider.Infrastructure.Database;
using MassTransit;
using MassTransit.Transports;
using Messages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Hermes.IdentityProvider.Pages.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly IdentityProviderDbContext _context;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IBus _broker;

    [BindProperty]
    public InputModel Input { get; set; }
        
    public Index(
        IdentityProviderDbContext context,
        IIdentityServerInteractionService interaction,
        IBus broker)
    {
        // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
        _context = context;
        _interaction = interaction;
        _broker = broker;
    }

    public IActionResult OnGet(string returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };
        return Page();
    }
        
    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "create")
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl);
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        //if (_users.FindByUsername(Input.Username) != null)
        //{
        //    ModelState.AddModelError("Input.Username", "Invalid username");
        //}

        if (ModelState.IsValid)
        {
            var user = await new RegisterUser(_context).CreateUser(Input.Username, Input.Email, Input.Password);

            //Sending Command to UserService
            await _broker.Publish<AddNewUser>(new
            {
                UserId = user.SubjectId,
                CommandId = Guid.NewGuid(),
                //FirstName 
                //LastName
                //DateOfBirth
            });

            // issue authentication cookie with subject ID and username
            var isuser = new IdentityServerUser(user.SubjectId)
            {
                DisplayName = user.UserName
            };

            await HttpContext.SignInAsync(isuser);

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                return Redirect(Input.ReturnUrl);
            }

            // request for a local page
            if (Url.IsLocalUrl(Input.ReturnUrl))
            {
                return Redirect(Input.ReturnUrl);
            }
            else if (string.IsNullOrEmpty(Input.ReturnUrl))
            {
                return Redirect("~/");
            }
            else
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }
        }

        return Page();
    }
}