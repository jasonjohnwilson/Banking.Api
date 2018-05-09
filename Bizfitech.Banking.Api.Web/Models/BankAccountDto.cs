using System.ComponentModel.DataAnnotations;

namespace Bizfitech.Banking.Api.Web.Models
{
    public class BankAccountDto
    {
        [Required]
        public string BankName { get; set; }

        [Required]
        [RegularExpression("^[\\d]{8}$")]
        public string AccountNumber { get; set; }
    }
}
