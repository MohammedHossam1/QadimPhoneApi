using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.ServiceContracts;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IToken _token;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IToken token,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmaillExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email is already exist"));
            }
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber=model.PhoneNumber
            };
           var res= await _userManager.CreateAsync(user,model.Password);
            if (!res.Succeeded) return BadRequest(new ApiResponse(400));
            var resturnDto = new UserDto()
            {
                DisplayName=user.DisplayName,
                Email=user.Email,
                Token = await _token.CreateTokenAsync(user, _userManager)
            };
            return Ok(resturnDto);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
          var user=await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));
           var res=await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);
            if (!res.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _token.CreateTokenAsync(user,_userManager)
            });
        }

        [Authorize(AuthenticationSchemes ="Bearer")]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
           var userEmail= User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
        return Ok(new UserDto()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = await _token.CreateTokenAsync(user, _userManager)
        });
        
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("address")]
        public async Task<ActionResult<Address>> GetUserWithAddress()
        {
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(userEmail);
            var user = await _userManager.FindUserWithAddressAsync(User);
            var userAdressDto=_mapper.Map<Address,AddressToReturnDto>(user.Address);
            return Ok(userAdressDto);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("address")]
        public async Task<ActionResult<Address>> UpdateUserAddress(AddressToReturnDto model)
        {
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(userEmail);
            var user = await _userManager.FindUserWithAddressAsync(User);

            var userAdressDto = _mapper.Map<AddressToReturnDto, Address>(model);
            user.Address=userAdressDto;
           var res=await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(model);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("checkemail")]
        public async Task<ActionResult<bool>> CheckEmaillExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
