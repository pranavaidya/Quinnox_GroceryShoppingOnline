using GroceryOnlineShopModelLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RCHS.MS.App.Services;
using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShop.App.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApiRequestProvider _apiRequest;

    [BindProperty]
    public List<CustomerDto>? Customers { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please select valid customer")]
    public int SelectedCustomer { get; set; } = new();

    public IndexModel(ILogger<IndexModel> logger, ApiRequestProvider apiRequest)
    {
        _logger = logger;
        _apiRequest = apiRequest;
    }

    public IActionResult OnGet()
    {
        var customers = _apiRequest.Request<GeneralResponseDto<List<CustomerDto>>>("api/groceryshop/customers").GetAwaiter().GetResult();
        Customers = customers!.Value;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (SelectedCustomer == -1)
        {
            ModelState["SelectedCustomer"]?.Errors.Clear();
            ModelState.AddModelError("SelectedCustomer", "Please select valid customer");
        }

        if (!ModelState.IsValid)
            return OnGet();

        return RedirectToPage("/GroceryShoppingOnline/Index", new { customerId = SelectedCustomer });
    }

}
