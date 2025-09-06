using ASP.NET.DEMO.API.Data;
using ASP.NET.DEMO.API.Models.DTOS.SystemConfig.AddressBook;
using ASP.NET.DEMO.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.DEMO.API.Controllers
{
    [Route("SystemDefault")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AddressBookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: /SystemDefault/get-AddressBook-info?parent_id=1&type=Office
        [HttpGet("get-AddressBook-info")]
        public async Task<ActionResult<ApiResponse<List<DTOAddressBook>>>> GetAddressBooks(
            [FromQuery] int? parent_id,
            [FromQuery] string? type)
        {
            var query = _context.AddressBooks.AsQueryable();

            if (parent_id.HasValue)
                query = query.Where(x => x.parent_id == parent_id.Value);

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(x => x.type == type);

            var data = await query.ToListAsync();
            return new ApiResponse<List<DTOAddressBook>> { Success = true, Data = data, Message = "Loaded" };

        }


        // ✅ POST: /SystemDefault/create-AddressBook
        [HttpPost("create-AddressBook")]
        public async Task<ActionResult<ApiResponse<DTOAddressBook>>> Create([FromBody] DTOAddressBook dto)
        {
            dto.created_date = DateTime.Now;
            _context.AddressBooks.Add(dto);
            await _context.SaveChangesAsync();

            return new ApiResponse<DTOAddressBook>
            {
                Success = true,
                Message = "Address Book created successfully",
                Data = dto
            };
        }
    }
}
