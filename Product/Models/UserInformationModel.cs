using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Models
{
    [Table("UserInformation")]
    public class UserInformationModel
    {
        [Key]
        public Guid Id { get; set; }

        public String  Name { get; set; }

        public Gender Gender { get; set; }

        public Int32 Age { get; set; }

        public String Email { get; set; }
    }

    public enum Gender
    {
        Woman = 0,
        Man = 1
    }
}