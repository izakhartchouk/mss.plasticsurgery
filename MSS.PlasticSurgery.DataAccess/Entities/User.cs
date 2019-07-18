using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSS.PlasticSurgery.DataAccess.Entities
{
    [Table("Users")]
    public class User : IdentityUser<int>
    {
    }
}