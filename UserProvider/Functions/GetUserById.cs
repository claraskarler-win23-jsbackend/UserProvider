using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace UserProvider.Functions;

public class GetUserById(ILogger<GetUserById> logger, DataContext context)
{
    private readonly ILogger<GetUserById> _logger = logger;
    private readonly DataContext _context = context;

    [Function("GetUserById")]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getuser/{id}")] HttpRequest req,
            string id)
    {
        _logger.LogInformation($"Processed a request to get user with ID: {id}");

        if (string.IsNullOrEmpty(id))
        {
            return new BadRequestObjectResult("Invalid user ID.");
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return new NotFoundObjectResult("User not found.");
        }

        return new OkObjectResult(user);
    }
}
