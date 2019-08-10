using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MSS.PlasticSurgery.DataAccess.Constants;
using MSS.PlasticSurgery.DataAccess.Entities.Base;

namespace MSS.PlasticSurgery.DataAccess.Entities
{
    [Table("Comments")]
    public class Comment : IEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(DatabaseConstants.NameMaxLength)]
        [Required]
        public string AuthorName { get; set; }

        [MaxLength(DatabaseConstants.DescriptionMaxLength)]
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}