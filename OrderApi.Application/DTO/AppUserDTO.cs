using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTO
{
    public record AppUserDTO
        (
            int Id,
            [Required] string Name,
            [Required] string TelephoneNumber,
            [Required , EmailAddress] string Email,
            [Required] string Address,
            [Required] string Password,
            [Required] string Role
        );
    
}
