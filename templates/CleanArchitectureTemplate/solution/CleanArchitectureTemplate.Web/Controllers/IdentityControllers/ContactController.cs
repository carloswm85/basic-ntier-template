using CleanArchitectureTemplate.ApplicationCore.Constants;
using CleanArchitectureTemplate.ApplicationCore.Entities;
using CleanArchitectureTemplate.ApplicationCore.Interfaces.ContosoInterfaces;
using CleanArchitectureTemplate.Infrastructure.Authorization.ContactAuthorization;
using CleanArchitectureTemplate.Infrastructure.Interfaces.IdentityInterfaces;
using CleanArchitectureTemplate.Web.Models.ViewModels.ContactViewModels;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.Web.Controllers.IdentityControllers;

[Authorize]
public class ContactController : Controller
{
    private readonly IContosoUniversityService _contactService;
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IUserManagerService _userManagerService;

    public ContactController(
        IContosoUniversityService contactService,
        IAuthorizationService authorizationService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        IUserManagerService userManagerService
    )
    {
        _contactService = contactService;
        _authorizationService = authorizationService;
        _logger = loggerFactory.CreateLogger<ContactController>();
        _mapper = mapper;
        _userManagerService = userManagerService;
    }

    public async Task<IActionResult> Index()
    {
        var isAuthorized =
            User.IsInRole(RoleConstants.ManagersRole)
            || User.IsInRole(RoleConstants.AdministratorsRole);

        var currentUserId = _userManagerService.GetUserId(User);

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Challenge();
        }

        var contactList = await _contactService.GetContactsAsync(isAuthorized, currentUserId);

        var listVm = new ContacListViewModel { Contacts = contactList.ToList() };

        return View(listVm);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContactViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        viewModel.OwnerID = _userManagerService.GetUserId(User);

        var contact = _mapper.Map<Contact>(viewModel);

        var isAuthorized = await _authorizationService.AuthorizeAsync(
            User,
            contact,
            ContactOperations.Create
        );

        if (!isAuthorized.Succeeded)
            return Forbid();

        await _contactService.CreateAsync(contact);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var contact = await _contactService.GetByIdAsync(id);

        if (contact == null)
            return NotFound();

        var isAuthorized = await _authorizationService.AuthorizeAsync(
            User,
            contact,
            ContactOperations.Update
        );

        if (!isAuthorized.Succeeded)
            return Forbid();

        var viewModel = _mapper.Map<ContactViewModel>(contact);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ContactViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var contact = await _contactService.GetByIdAsNoTrackingAsync(id);

        if (contact == null)
            return NotFound();

        var isAuthorized = await _authorizationService.AuthorizeAsync(
            User,
            contact,
            ContactOperations.Update
        );

        if (!isAuthorized.Succeeded)
            return Forbid();

        viewModel.OwnerID = contact.OwnerID;
        var contactMapped = _mapper.Map<Contact>(viewModel);

        if (contactMapped.Status == ContactStatus.Approved)
        {
            var canApprove = await _authorizationService.AuthorizeAsync(
                User,
                contactMapped,
                ContactOperations.Approve
            );

            if (!canApprove.Succeeded)
                contactMapped.Status = ContactStatus.Submitted;
        }

        await _contactService.UpdateAsync(contactMapped);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var contact = await _contactService.GetByIdAsync(id);

        if (contact == null)
            return NotFound();

        var isAuthorized =
            User.IsInRole(RoleConstants.ManagersRole)
            || User.IsInRole(RoleConstants.AdministratorsRole);

        var currentUserId = _userManagerService.GetUserId(User);

        if (
            !isAuthorized
            && currentUserId != contact.OwnerID
            && contact.Status != ContactStatus.Approved
        )
        {
            return Forbid();
        }

        var viewModel = _mapper.Map<ContactViewModel>(contact);
        viewModel.Contact = contact;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Details(int id, ContactStatus status)
    {
        var contact = await _contactService.GetByIdAsync(id);

        if (contact == null)
            return NotFound();

        var contactOperation =
            (status == ContactStatus.Approved)
                ? ContactOperations.Approve
                : ContactOperations.Reject;

        var isAuthorized = await _authorizationService.AuthorizeAsync(
            User,
            contact,
            contactOperation
        );

        if (!isAuthorized.Succeeded)
            return Forbid();

        await _contactService.UpdateStatusAsync(id, status);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _contactService.GetByIdAsync(id);

        if (contact == null)
            return NotFound();

        var isAuthorized = await _authorizationService.AuthorizeAsync(
            User,
            contact,
            ContactOperations.Delete
        );

        if (!isAuthorized.Succeeded)
            return Forbid();

        var viewModel = _mapper.Map<ContactViewModel>(contact);
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var contact = await _contactService.GetByIdAsNoTrackingAsync(id);

        if (contact == null)
            return NotFound();

        var isAuthorized = await _authorizationService.AuthorizeAsync(
            User,
            contact,
            ContactOperations.Delete
        );

        if (!isAuthorized.Succeeded)
            return Forbid();

        await _contactService.DeleteAsync(contact);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Instructions()
    {
        var instructions =
            @"
            == INSTRUCTIONS FOR CONTACT MANAGER EXAMPLE ==

            Link to tutorial: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-10.0
                
            - This example requires the creation of users with roles Admin, Manager, and User.
            - Works with Identity API.
        ";

        return Ok(instructions);
    }
}
