using Newtonsoft.Json;

namespace IK_Project.UI.Areas.Admin.Models.ViewModels
{
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }



        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
