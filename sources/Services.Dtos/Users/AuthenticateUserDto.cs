﻿namespace Services.Dtos.Users
{
    public class AuthenticateUserDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}