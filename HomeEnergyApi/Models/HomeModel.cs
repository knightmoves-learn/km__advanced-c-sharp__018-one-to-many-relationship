using System.ComponentModel.DataAnnotations;
namespace HomeEnergyApi.Models
{
    public class Home
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OwnerLastName { get; set; }

        [StringLength(40)]
        public string? StreetAddress { get; set; }

        public string? City { get; set; }

        public HomeUsageData? HomeUsageData { get; set; }

        public Home(string ownerLastName, string? streetAddress, string? city)
        {
            OwnerLastName = ownerLastName;
            StreetAddress = streetAddress;
            City = city;
        }
    }
}