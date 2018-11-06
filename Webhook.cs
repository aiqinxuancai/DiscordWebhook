using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordWebhook
{
    [JsonObject]
    public class Webhook
    {
        private readonly HttpClient _httpClient;
        private readonly string _webhookUrl;

        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        // ReSharper disable once InconsistentNaming
        [JsonProperty("tts")]
        public bool IsTTS { get; set; }
        [JsonProperty("embeds")]
        public List<Embed> Embeds { get; set; } = new List<Embed>();


        private List<string> Files { get; set; } = new List<string>();

        public Webhook(string webhookUrl, string proxy = null)
        {
            if (proxy == null)
            {
                var cookies = new CookieContainer();

                var handler = new HttpClientHandler
                {
                    CookieContainer = cookies,
                    UseCookies = true,
                    UseDefaultCredentials = false,
                    Proxy = new WebProxy("http://localhost:1080", false, new string[] { }),
                    UseProxy = true,
                };
                _httpClient = new HttpClient(handler);
            }
            else
            {
                _httpClient = new HttpClient();
            }
            _webhookUrl = webhookUrl;
        }

        public Webhook(ulong id, string token) : this($"https://discordapp.com/api/webhooks/{id}/{token}")
        {

        }

        public async Task<HttpResponseMessage> Send()
        {

            var contentPayloadJson = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");

            if (Files == null)
            {
                return await _httpClient.PostAsync(_webhookUrl, contentPayloadJson);
            }
            else
            {
                var contentMultipartFormData = new MultipartFormDataContent();
                contentMultipartFormData.Add(contentPayloadJson, "payload_json");

                foreach (var item in Files)
                {
                    var name = Path.GetFileName(item);
                    var contentFile = new StreamContent(new FileStream(item, FileMode.Open));
                    contentMultipartFormData.Add(contentFile, name, name);
                }

                return await _httpClient.PostAsync(_webhookUrl, contentMultipartFormData);
            }

        }

        // ReSharper disable once InconsistentNaming
        public async Task<HttpResponseMessage> Send(string content, string username = null, string avatarUrl = null, bool isTTS = false, IEnumerable<Embed> embeds = null , List<string> uploadFiles = null)
        {
            Content = content;
            Username = username;
            AvatarUrl = avatarUrl;
            IsTTS = isTTS;
            Files = uploadFiles;
            Embeds.Clear();
            if (embeds != null)
            {
                Embeds.AddRange(embeds);
            }

            return await Send();
        }
    }
}
