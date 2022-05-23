using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.imts;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public interface IEntity
    {
        int Id { get; }
    }
    public interface IEntityScope
    {
        int officeId { get; set; }
    }
    public enum OfficeRoleEnum
    {
        Super = 1,
        Administrator,
        User
    }
    public class EntityScope : IEntityScope
    {
        public int officeId { get; set; }
    }
    public class OfficeRole : IEntity
    {
        public int Id { get; set; }

        public string RoleName { get; set; }
    }

    public class AppUserOfficeRole
    {
        public int RoleId { get; set; }
        public virtual OfficeRole Role { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        //Users have roles by office
        public int ImtsOfficeId { get; set; }
        [NotMapped]
        public virtual Office ImtsOffice { get; set; }


    }
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MainOfficeId { get; set; }
        public bool IsImtsUser { get; set; }
        public string ImtsUserName { get; set; } = string.Empty;
        public int? ImtsEmployeeId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsSuperUser { get; set; } = false;
    }
}