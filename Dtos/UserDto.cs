﻿namespace StoredProcuduresTest.Dtos
{
    public partial class UserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}
