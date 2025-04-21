using API.IServices;
using API.Model;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var response = await _authService.Login(model);
        if (response.Success)
        {
            return Ok(response);
        }

        return Unauthorized(response);
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        var response = await _authService.Register(model);
        if (response.Success)
        {
            return Ok(response);
        }

        return Unauthorized(response);
    }
}
