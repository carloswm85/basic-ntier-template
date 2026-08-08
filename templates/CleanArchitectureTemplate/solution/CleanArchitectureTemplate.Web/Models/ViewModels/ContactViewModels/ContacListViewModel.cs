using CleanArchitectureTemplate.ApplicationCore.Entities;

namespace CleanArchitectureTemplate.Web.Models.ViewModels.ContactViewModels;

public class ContacListViewModel
{
    public IList<Contact> Contacts { get; set; } = [];
}
