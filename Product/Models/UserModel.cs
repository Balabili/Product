using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Product.Models
{
    [Table("User")]
    public class UserModel
    {
        [Key]
        public Guid Id { get; set; }

        public String LoginName { get; set; }

        public String PassWord { get; set; }

        public Role Role { get; set; }

        [ForeignKey("UserInformation")]
        public Guid? UserInformationId { get; set; }

        public virtual UserInformationModel UserInformation { get; set; }

    }

    public enum Role
    {
        SuperAdmin = 0,
        Admin = 1,
        EndUser = 2,
    }
}