using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HomeEnergyApi.Models
{
    public class UtilityProvider
    {
        public int Id {get; set;}
        public List<string> ProvidedUtilities {get; set;} = new List<string>();
        public int HomeId {get; set;}
        [JsonIgnore]
        public Home? Home {get; set;} = null!;
    }
}