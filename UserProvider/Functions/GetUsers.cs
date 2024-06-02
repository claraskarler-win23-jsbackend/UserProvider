using Data.Contexts;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserProvider.Functions;

public class GetUsers(ILogger<GetUsers> logger, DataContext context)
{
    private readonly ILogger<GetUsers> _logger = logger;
    private readonly DataContext _context = context;

    [Function("GetUsers")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        _logger.LogInformation("Processing request to get users.");

        var users = await _context.Users.ToListAsync();

        var userModels = users.Select(user => new UserModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Biography = user.Biography,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        }).ToList();

        return new OkObjectResult(userModels);
    }
}
