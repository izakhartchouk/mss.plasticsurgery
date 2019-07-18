using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MSS.PlasticSurgery.DataAccess.Constants;
using MSS.PlasticSurgery.DataAccess.Entities.Base;

namespace MSS.PlasticSurgery.DataAccess.Entities
{
    [Table("Operations")]
    public class Operation : IEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        [MaxLength(DatabaseConstants.EntityTitleMaxLength)]
        [Required]
        public string Title { get; set; }

        [MaxLength(DatabaseConstants.EntityTitleMaxLength)]
        [Required]
        public string Subtitle { get; set; }

        [MaxLength(DatabaseConstants.NvarcharMaxLength)]
        [Required]
        public string Description { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}