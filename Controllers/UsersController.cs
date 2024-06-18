using MerchantService.Base;
using MerchantService.Models;
using MerchantService.Repositories.Data;
using MerchantService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MerchantService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<User, UserRepository, int>
    {
        private readonly UserRepository userRepository;
        private readonly IConfiguration configuration;

        public UsersController(UserRepository userRepository, IConfiguration configuration) : base (userRepository)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }


        [HttpPost("/Register")]
        public async Task<ActionResult> Register(RegisterVM registerVM)
        {
            try
            {
                var result = userRepository.Register(registerVM);
                if (result == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Failed to Register."
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Succeed to Register.",
                        Data = result
                    });
                }
            }
            catch
            {
                return BadRequest(new
                {
                    StatusCode = 500,
                    Message = "Something Wrong. Please Try Again."
                });
            }
        }


        [HttpPost("Login")]

        public async Task<ActionResult> Login(LoginVM loginVM)
        {
            try {
                string emailCheck = await userRepository.CheckEmail(loginVM.Email);


                if (string.IsNullOrEmpty(emailCheck))
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Something Wrong. Please Try Again."

                    });
                }
                else
                {
                    Boolean isFirstLogin = false;
                    Boolean isValidPass = false;
                    Boolean isForcePasswordChange = false;

                    UserLoginVM userDetails = await userRepository.LoginDetailAsync(loginVM.Email);

                    if (userDetails == null)
                    {
                        // Handle case where userDetails is null (user not found or other error)
                        throw new Exception("User details not found or error retrieving user.");
                    }

                    // Check if it's the user's first login
                    if (userDetails.FirstLoginAt == null)
                    {
                        isFirstLogin = true;
                        isForcePasswordChange = true; // Assuming first login always requires password change
                    }
                    else
                    {
                        // Check if password change is required
                        if (userDetails.PasswordChange >= 90)
                        {
                            isForcePasswordChange = true;
                        }
                    }

                    if (await userRepository.CheckPassword(loginVM.Password, userDetails.Password, userDetails.Salt))
                    {


                        TokenVM generateToken = new TokenVM();
                        generateToken.Email = userDetails.Email;
                        generateToken.FullName = userDetails.FullName;
                        generateToken.Role = userDetails.Role;
                        generateToken.Authorities = userDetails.Authorize;

                        string tokenLogin = await userRepository.GenerateToken(generateToken);

                        // Update last login time or other user details
                        int updateUser = await userRepository.UpdateLastLoginAsync(userDetails.Email, tokenLogin, isFirstLogin, true, userDetails.AttemptLoginFailed);

                        if (updateUser > 0)
                        {
                            // Return token and other necessary data
                            return Ok(new
                            {
                                StatusCode = 200,
                                Message = "Login successful.",
                                Token = tokenLogin,
                                UserDetails = userDetails // You can choose to return more user details if needed
                            });
                        }
                        else {
                            // Return token and other necessary data
                            return BadRequest(new
                            {
                                StatusCode = 500,
                                Message = "Failed Update User"
                            });
                        }

                       
                    }
                    else
                    {

                        // Update last login time or other user details
                        int updateUser = await userRepository.UpdateLastLoginAsync(userDetails.Email, "", isFirstLogin, false, userDetails.AttemptLoginFailed);

                        if (updateUser > 0)
                        {
                            // Return failed login

                            return BadRequest(new
                            {
                                StatusCode = 400,
                                Message = "Incorrect password."
                            });

                        }
                        else
                        {
                            // Return token and other necessary data
                            return BadRequest(new
                            {
                                StatusCode = 500,
                                Message = "Failed Update User"
                            });
                        }

                    }



                }

            } catch (Exception ex) {
                return BadRequest(new
                {
                    StatusCode = 500,
                    Message = ex.Message,
                });
            }


        }
    }
}
