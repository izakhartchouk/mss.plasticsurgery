using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using MSS.PlasticSurgery.DataAccess.Entities.Base;

namespace MSS.PlasticSurgery.DataAccess.Entities
{
    [Table("Users")]
    public class User : IdentityUser<int>, IEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
    }
}