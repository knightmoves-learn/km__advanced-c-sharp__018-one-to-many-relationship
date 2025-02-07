using System.Text.Json.Serialization;

namespace HomeEnergyApi.Models
{
    public class UtilityProvider
    {
        public int id { get; set; }
        public List<string> ProvidedUtilities { get; set; }
        public int HomeId { get; set; }
        
        [JsonIgnore]
        public Home? Home { get; set; } = null!;
    }
}