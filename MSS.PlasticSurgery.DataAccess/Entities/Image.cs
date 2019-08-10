using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MSS.PlasticSurgery.DataAccess.Constants;
using MSS.PlasticSurgery.DataAccess.Entities.Base;

namespace MSS.PlasticSurgery.DataAccess.Entities
{
    [Table("Images")]
    public class Image : IEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(DatabaseConstants.FilePathMaxLength)]
        [Required]
        public string Path { get; set; }

        #region Navigation Properties

        public int OperationId { get; set; }

        [ForeignKey("OperationId")]
        public virtual Operation Operation { get; set; }

        #endregion Navigation Properties
    }
}