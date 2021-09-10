using API.Middlewares.Autentication;
using API.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Study.EventManager.Model.Enums;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;


namespace API.Controllers
{
    [Authorize]
    [Route("api/authenticate")]
    [ApiController]

    //[ApiExplorerSettings(IgnoreApi = true)]
    public class AuthenticateController : ControllerBase
    {
        private IAuthenticateService _serviceAuth;
        private AuthOptions _authOptions;
        private IAuthenticateWrapper _authenticateWrapper;
      

        public AuthenticateController(IUserService serviceUser, IAuthenticateService serviceAuth
            , IConfiguration config, IAuthenticateWrapper authenticateWrapper)
        {
            _authenticateWrapper = authenticateWrapper;
            _serviceAuth = serviceAuth;
            _authOptions = config.GetSection("AuthOptions").Get<AuthOptions>();
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate(AuthenticateRequestModel model)
        {
            try
            {
                var user = _serviceAuth.Authenticate(model.Email, model.Password);
                var response = GiveJWTToken(user.Email, user.Role.ToString());

                return Ok(response);
            }
            catch (Exception ee)
            {
                return BadRequest(ee.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("validateUser")]
        public IActionResult ValidateUser(string email, string validTo, string code)
        {
            try
            {
                var date = DateTime.Today;

                var validDT = DateTime.ParseExact(validTo, "dd.MM.yyyy", null);

                if (validDT < date)
                {
                    return BadRequest("url expired");
                }

                var IsVerified = _serviceAuth.VerifyUrlEmail(email, code);

                return Ok(IsVerified);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("sendRestoreEmail")]
        public IActionResult SendRestoreEmail(UserEmailModel model)
        {
            try
            {
                _serviceAuth.sendRestoreEmail(model.Email);
                return Ok("link to restore your password sent to email");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("restorePassword")]
        public IActionResult restorePass(UserRestorePasswordModel model)
        {
            try
            {
                var date = DateTime.Today;
                var validDT = DateTime.ParseExact(model.validTo, "dd.MM.yyyy", null);

                if (validDT < date)
                {
                    return BadRequest("url expired");
                }

                var response = _serviceAuth.restorePass(model.Email, model.Password, model.code);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("google-login")]
        public IActionResult GoogleLogin(SocialNetworkIdTokenModel data)
        {
            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
                settings.Audience = new List<string>() { "752253873246-cg9qrlhp0tmtn7cd8vpg4qrfk03br55c.apps.googleusercontent.com" };
                GoogleJsonWebSignature.Payload userGoogle = GoogleJsonWebSignature.ValidateAsync(data.IdToken, settings).Result;

                _serviceAuth.SocialNetworksAuthenticate(userGoogle.Email, userGoogle.GivenName, userGoogle.FamilyName, userGoogle.Picture);

                var response = GiveJWTToken(userGoogle.Email, "0");
                return Ok(response);
            }
            catch (Exception ee)
            {
                return BadRequest(ee.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("facebook-login")]
        public async Task<IActionResult> FacebookLogin(SocialNetworkIdTokenModel data)
        {
            var facebookAPIResponce = await _authenticateWrapper.GetFacebookToken(data.IdToken);
            if (facebookAPIResponce != null)
            {
                var facebookUserInfo = await _authenticateWrapper.GetFacebookUserData(facebookAPIResponce.id, data.IdToken);

                _serviceAuth.SocialNetworksAuthenticate(facebookUserInfo.email, facebookUserInfo.first_name,
                                                    facebookUserInfo.last_name, data.Url);

                var response = GiveJWTToken(facebookUserInfo.email, "0");

                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        private JwtTokenModel GiveJWTToken(string email, string role)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: _authOptions.Issuer,
                    audience: _authOptions.Audience,
                    notBefore: now,

                    claims: GetClaims(role),
                    expires: now.Add(TimeSpan.FromMinutes(_authOptions.LifeTime)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(_authOptions.SecretKey), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new JwtTokenModel
            {
                access_token = encodedJwt,
                email = email,

            };

            return response;
        }

        private IEnumerable<Claim> GetClaims(string role)
        {
            return new[]
            {
                new Claim("role", role)
            };            
        }
    } 
};
