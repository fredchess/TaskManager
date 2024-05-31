using Microsoft.AspNetCore.Identity;
using TaskManager.Enums;

namespace TaskManager.Models;

public class User : IdentityUser
{
    public UserType UserType { get; set; } = UserType.Guest;
}