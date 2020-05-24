using System;

namespace UserManager.API.Models
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserDto dto &&
                   UserId == dto.UserId &&
                   FirstName == dto.FirstName &&
                   LastName == dto.LastName &&
                   DateCreated == dto.DateCreated;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, FirstName, LastName, DateCreated);
        }
    }
}
