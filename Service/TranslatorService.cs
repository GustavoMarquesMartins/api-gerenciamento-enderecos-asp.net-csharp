using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Text.Json;
using System.Text;
using System.Runtime.CompilerServices;

namespace GerenciamentoDeEndereco.Service
{
    public class TranslatorService
    {
        private string _endpoint = "https://api.apilayer.com/language_translation/translate";
        public string targetLanguage { get; set; }
        public string key { get; set; }

        public TranslatorService config(Action<TranslatorService> config)
        {
            config(this);
            return this;
        }

        public async Task<String> translatorAsync(string message)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-KEY", key);

                var content = new StringContent(message, Encoding.UTF8, "text/plain");
                var response = await client.PostAsync($"{_endpoint}?target={targetLanguage}", content);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                using (var doc = JsonDocument.Parse(responseBody))
                {
                    if (doc.RootElement.TryGetProperty("translations", out JsonElement translations) &&
                        translations[0].TryGetProperty("text", out JsonElement textElement))
                    {
                        return textElement.GetString() ?? "Translation not found";
                    }
                }
                return "Translation not found";
            }
        }
    }
}
