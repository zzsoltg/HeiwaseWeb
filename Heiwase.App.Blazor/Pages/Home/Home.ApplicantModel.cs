using System.ComponentModel.DataAnnotations;
namespace Heiwase.App.Blazor.Pages.Home;

public partial class Home
{
    public class ApplicantModel
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
