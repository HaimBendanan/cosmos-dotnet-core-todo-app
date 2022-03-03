namespace todo.Models
{
    using Newtonsoft.Json;

    // TODO rename to Course

    public class Prescription
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "patientSSID")]
        public string PatientSSID { get; set; }

        [JsonProperty(PropertyName = "patientName")]
        public string PatientName { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "howToUse")]
        public string HowToUse { get; set; }

        [JsonProperty(PropertyName = "isPurchased")]
        public bool IsPurchased { get; set; }
    }
}
