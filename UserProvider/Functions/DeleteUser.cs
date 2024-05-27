using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserProvider.Functions
{
    public class DeleteUser(ILogger<DeleteUser> logger, DataContext context)
    {
        private readonly ILogger<DeleteUser> _logger = logger;
        private readonly DataContext _context = context;

        [Function("DeleteUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deleteuser")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to delete a user.");

            string email = req.Query["email"];

            if (string.IsNullOrEmpty(email))
            {
                return new BadRequestObjectResult("Invalid email address.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return new NotFoundObjectResult("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult("User deleted successfully.");
        }
    }
}
