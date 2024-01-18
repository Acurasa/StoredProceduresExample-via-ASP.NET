using AutoMapper;
using StoredProcuduresTest.Data;
using Microsoft.AspNetCore.Mvc;
using StoredProcuduresTest.Dtos;
using StoredProcuduresTest.Models;
using System.Linq;
using Microsoft.Identity.Client;

namespace StoredProcuduresTest.Controller;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    IUserRepository _userRepository;
    public UserEFController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.Get();
        return Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetSingleUser(int userId)
    {
        var user = await _userRepository.GetSingle(userId);
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> EditUser(User user)
    {
        await _userRepository.UpdateEntity(user);
        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> AddUser(UserDto user)
    {
        await _userRepository.AddEntity(user);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _userRepository.DeleteEntity(userId);
        return Ok();
    }
}
