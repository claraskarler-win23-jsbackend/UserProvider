using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Data.Entities;

namespace UserProvider.Functions
{
    public class UpdateUser(ILogger<UpdateUser> logger, DataContext context)
    {
        private readonly ILogger<UpdateUser> _logger = logger;
        private readonly DataContext _context = context;

        [Function("UpdateUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "updateuser/{id}")] HttpRequest req,
            string id)
        {
            _logger.LogInformation("Processed a request to update user.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<ApplicationUser>(requestBody);

            if (data == null)
            {
                return new BadRequestObjectResult("Invalid user data.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult("User was not found.");
            }
            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.AddressId = data.AddressId;
            user.ProfileImage = data.ProfileImage;
            user.Biography = data.Biography;

            if (data.Address != null)
            {
                if (user.Address != null)
                {
                    user.Address.AdressLine_1 = data.Address.AdressLine_1;
                    user.Address.AdressLine_2 = data.Address.AdressLine_2;
                    user.Address.PostalCode = data.Address.PostalCode;
                    user.Address.City = data.Address.City;
                }
                else
                {
                    user.Address = new AddressEntity
                    {
                        AdressLine_1 = data.Address.AdressLine_1,
                        AdressLine_2 = data.Address.AdressLine_2,
                        PostalCode = data.Address.PostalCode,
                        City = data.Address.City
                    };
                }
            }

            await _context.SaveChangesAsync();

            return new OkObjectResult("User updated successfully.");
        }
    }
}
