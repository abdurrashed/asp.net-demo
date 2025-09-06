using DEMO.RAZOR.DTO;
using DEMO.RAZOR.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DEMO.RAZOR.Pages
{
    public class AddressBookModel : PageModel
    {
        public List<DTOAddressBook> AddressBookList { get; set; } = new();

        // Dropdown data
        public List<string> ParentList { get; set; } = new();
        public List<AddressType> AddressList { get; set; } = new();

        [BindProperty]
        public DTOAddressBook AddressBook { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public AddressType? SelectedType { get; set; }

        public string ErrorMessage { get; set; }

        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public AddressBookModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient("VelocityAPI");
            foreach (AddressType type in System.Enum.GetValues(typeof(AddressType)))
            {
                AddressList.Add(type);
            }
        }

        public async Task OnGetAsync()
        {
            try
            {
                // build API url with optional filter
                var url = "SystemDefault/get-AddressBook-info";
                if (SelectedType.HasValue)
                {
                    url += $"?type={SelectedType}";
                }

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var api = await response.Content.ReadFromJsonAsync<ApiResponse<List<DTOAddressBook>>>();

                if (api is { Success: true, Data: not null })
                {
                    AddressBookList = api.Data;
                }
                else
                {
                    ErrorMessage = api?.Message ?? "Unknown error while loading Address Book.";
                    AddressBookList = new List<DTOAddressBook>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load Address Book. " + ex.Message;
                AddressBookList = new List<DTOAddressBook>();
            }
        }

        public async Task<IActionResult> OnPostSaveAddressBookAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in all required fields.";
                await OnGetAsync();
                return Page();
            }

            try
            {
                // Prepare API URL
                var url = "SystemDefault/create-AddressBook";

             

                // Log the payload for debugging
                var json = System.Text.Json.JsonSerializer.Serialize(AddressBook);
                Console.WriteLine($"Payload: {json}");

                // Create HTTP request
                var request = new HttpRequestMessage(AddressBook.address_id == 0 ? HttpMethod.Post : HttpMethod.Put, url);
                request.Content = JsonContent.Create(AddressBook);

                // Send request
                var response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error Response: {errorContent}");
                    ErrorMessage = $"API error {response.StatusCode}: {errorContent}";
                    await OnGetAsync();
                    return Page();
                }

                // Read API response
                var api = await response.Content.ReadFromJsonAsync<ApiResponse<DTOAddressBook>>();

                if (api is { Success: true })
                {
                    TempData["Success"] = "Address saved successfully.";
                    return RedirectToPage("/AddressBook");
                }
                else
                {
                    ErrorMessage = api?.Message ?? "Unknown error while saving Address Book.";
                    await OnGetAsync();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to save Address Book. " + ex.Message;
                await OnGetAsync();
                return Page();
            }
        }
    }
    }
