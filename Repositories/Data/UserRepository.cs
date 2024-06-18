using MerchantService.Context;
using MerchantService.Handlers;
using MerchantService.Models;
using MerchantService.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MerchantService.Repositories.Data
{
    public class UserRepository : GeneralRepository<MerchantContext, User, int>
    {
        private readonly MerchantContext merchantContext;
        private readonly Hashing hashing;
        public IConfiguration configuration;

        public UserRepository(IConfiguration config, MerchantContext merchantContext, Hashing hashing) : base(merchantContext)
        {
            this.merchantContext = merchantContext;
            this.hashing = hashing;
            this.configuration = config;
        }


        public async Task<int> Register(RegisterVM registerVM)
        {

            // Validate the RegisterVM
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(registerVM, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(registerVM, context, validationResults, true);

            if (!isValid)
            {
                throw new ValidationException("RegisterVM validation failed: " + string.Join(", ", validationResults.Select(vr => vr.ErrorMessage)));
            }

            int result = 0;

            String salt = hashing.GetSalt();
            String password = hashing.HashString(registerVM.Password, salt);

            // Get current date and time
            DateTime now = DateTime.Now;
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);


            var existingUser = await merchantContext.Users.FirstOrDefaultAsync(u => u.Email == registerVM.Email);
            if (existingUser != null)
            {
                // Update the existing user
                existingUser.Password = password;
                existingUser.Salt= salt;
                existingUser.UpdatedAt = now;

                merchantContext.Users.Update(existingUser);
                result = await merchantContext.SaveChangesAsync();

                // Check if the User was successfully updated
                if (result > 0)
                {
                    // Create a new Merchant object
                    Merchant merchant = new Merchant
                    {
                        Mid = registerVM.mid,
                        Nama = registerVM.NamaMerchant
                    };

                    // Add the new merchant to the database
                    await merchantContext.Merchants.AddAsync(merchant);
                    result = await merchantContext.SaveChangesAsync();

                }

                // Check if the Merchant was successfully inserted
                if (result > 0)
                {
                    if (registerVM.Role.Equals("Owner-Individu"))
                    {
                        OwnerMerchant owner = new OwnerMerchant
                        {
                            Mid = registerVM.mid,
                            UserId = existingUser.Id
                        };

                        // Add the new owner merchant to the database
                        await merchantContext.OwnerMerchants.AddAsync(owner);
                        result = await merchantContext.SaveChangesAsync();

                    }
                    else if (registerVM.Role.Equals("Cashier"))
                    {
                        CashierMerchant cashier = new CashierMerchant
                        {
                            UserId = existingUser.Id,
                            Mid = registerVM.mid
                        };

                        // Add the new owner merchant to the database
                        await merchantContext.CashierMerchants.AddAsync(cashier);
                        result = await merchantContext.SaveChangesAsync();

                    }
                    else
                    {
                        throw new Exception("Invalid Role provided.");
                    }
                }
            }
            else {

                User user = new User
                {
                    Username = registerVM.Email,
                    Fullname = registerVM.Fullname,
                    Email = registerVM.Email,
                    Salt = salt,
                    PasswordChange = 0,
                    Status = 1,
                    MobilePhone = registerVM.MobilePhone,
                    Password = password,
                    AttemptLoginFailed=0,
                    CreatedDate = DateOnly.FromDateTime(now),
                    CreatedAt = DateOnly.FromDateTime(now),
                    UpdatedAt = now
                };

                await merchantContext.Users.AddAsync(user);
                result = await merchantContext.SaveChangesAsync();


                // Check if the user was successfully inserted
                if (result > 0)
                {
                    // Create a new AuthAssignment object
                    AuthAssignment authAssignment = new AuthAssignment
                    {
                        ItemName = registerVM.Role,
                        UserId = user.Id, // UserId of the newly inserted user
                        CreatedAt = DateOnly.FromDateTime(now),
                    };

                    // Add the new auth assignment to the database
                    await merchantContext.AuthAssignments.AddAsync(authAssignment);
                    result = await merchantContext.SaveChangesAsync();
                }

                // Check if the AuthAssignment was successfully inserted
                if (result > 0)
                {
                    // Create a new Merchant object
                    Merchant merchant = new Merchant
                    {
                        Mid = registerVM.mid,
                        Nama = registerVM.NamaMerchant
                    };

                    // Add the new merchant to the database
                    await merchantContext.Merchants.AddAsync(merchant);
                    result = await merchantContext.SaveChangesAsync();

                }

                // Check if the Merchant was successfully inserted
                if (result > 0)
                {
                    if (registerVM.Role.Equals("Owner-Individu"))
                    {
                        OwnerMerchant owner = new OwnerMerchant
                        {
                            Mid = registerVM.mid,
                            UserId = user.Id
                        };

                        // Add the new owner merchant to the database
                        await merchantContext.OwnerMerchants.AddAsync(owner);
                        result = await merchantContext.SaveChangesAsync();

                    }
                    else if (registerVM.Role.Equals("Cashier"))
                    {
                        CashierMerchant cashier = new CashierMerchant
                        {
                            UserId = user.Id,
                            Mid = registerVM.mid
                        };

                        // Add the new owner merchant to the database
                        await merchantContext.CashierMerchants.AddAsync(cashier);
                        result = await merchantContext.SaveChangesAsync();

                    }
                    else
                    {
                        throw new Exception("Invalid Role provided.");
                    }
                }
            }

             return result;

        }


        public async Task<string> CheckEmail(string Email)
        {
            var userCheck = await merchantContext.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (userCheck == null)
            {
                return null;
            }
            else
            {
                return userCheck.Email;
            }
        }

        public async Task<string> GenerateToken(TokenVM tokenVM)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim("Email", tokenVM.Email),
                new Claim("FullName", tokenVM.FullName),
                new Claim("Role", tokenVM.Role)
                // Add additional claims for authorities if needed
            };

            foreach (var authority in tokenVM.Authorities)
            {
                claims.Add(new Claim("authority", authority));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiryInMinutes"])),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<Boolean> CheckPassword(string password, string passDb, string salt)
        {
            String passHash = hashing.HashString(password, salt);


            if (passDb == passHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public async Task<UserLoginVM> LoginDetailAsync(string email)
        {


            // Query to get user details
            var userDetail = await (from user in merchantContext.Users
                                    join authAssignment in merchantContext.AuthAssignments
                                        on user.Id equals authAssignment.UserId into authAssignments
                                    from authAssignment in authAssignments.DefaultIfEmpty()
                                    join authItem in merchantContext.AuthItems
                                        on authAssignment.ItemName equals authItem.Name into authItems
                                    from authItem in authItems.DefaultIfEmpty()
                                    join authItemChild in merchantContext.AuthItemChildren
                                        on authItem.Name equals authItemChild.Parent into authItemChildren
                                    from authItemChild in authItemChildren.DefaultIfEmpty()
                                    where user.Email == email
                                    select new UserLoginVM
                                    {
                                        Password = user.Password, // Assuming password retrieval is safe and secure
                                        FullName = user.Fullname, // Adjusted from Fullname to FullName
                                        MobilePhone = user.MobilePhone,
                                        Email = user.Email,
                                        Role = authAssignment.ItemName, // Assuming authAssignment is nullable, handle accordingly
                                        PasswordChange = (int)user.PasswordChange,
                                        Status = user.Status,
                                        FirstLoginAt = user.FirstLoginAt,
                                        AttemptLoginFailed = (int)user.AttemptLoginFailed,
                                        Salt = user.Salt,
                                        TokenWeb = user.Token ?? "", // Null-coalescing operator to handle null value
                                        LastLoginFailed = user.LastLoginFailed,
                                        Authorize = merchantContext.AuthItemChildren
                                                             .Where(ac => ac.Parent == authItem.Name)
                                                             .Select(ac => ac.Child)
                                                             .ToArray()
                                    })
                                .FirstOrDefaultAsync();
            if (userDetail == null)
            {
                throw new Exception("Error getting USER data");
            }

                
            return userDetail;
            
        }


        //Update Login Sukses
        public async Task<int> UpdateLastLoginAsync(string email, string token, bool isFirstLogin, bool isSuccess, int attemp)
        {
            int result = 0;

            var user = await merchantContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            // Get current date and time
            DateTime now = DateTime.Now;
            DateOnly currentDate = DateOnly.FromDateTime(now);

            if (user != null)
            {
                if (isSuccess)
                {
                    if (isFirstLogin)
                    {
                        user.Token = token;
                        user.LastLoginAt = currentDate;
                        user.FirstLoginAt = currentDate;
                        user.AttemptLoginFailed = 0;
                        user.UpdatedAt = now;
                    }
                    else
                    {
                        user.Token = token;
                        user.LastLoginAt = currentDate;
                        user.FirstLoginAt = currentDate;
                        user.AttemptLoginFailed = 0;
                        user.UpdatedAt = now;
                    }
                }
                else {

                    user.LastLoginFailed = currentDate;
                    user.AttemptLoginFailed = attemp+1;
                    user.UpdatedAt = now;
                }

                result =await merchantContext.SaveChangesAsync();
            }

            return result;
        }

    }




}
