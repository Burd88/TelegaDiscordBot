using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    class AutorizationBattleNet
    {
        // private static void OnTimerHandlerAutorizationsBattleNet(object obj)
        //  {
        //     AutorizationsBattleNet();
        // }

        public static async Task<string> OnTimerHandlerAutorizationBattleNet()
        {


            try
            {

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.ConnectionClose = true;
                    using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), "https://eu.battle.net/oauth/token"))
                    {

                        string base64authorization = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{settings.BatNetToken}:{settings.BatNetSecretKey}"));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        request.Content = new StringContent("grant_type=client_credentials");
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        Token_for_api my_token = JsonConvert.DeserializeObject<Token_for_api>(response.Content.ReadAsStringAsync().Result);
                        return my_token.access_token;

                        // Console.ForegroundColor = ConsoleColor.Green;
                        //     Console.WriteLine($"Battle.net Token success : {tokenWow}");
                        //Functions.LoadRealmAll();



                    }

                }

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {

                    string message = $"Status Code : {((HttpWebResponse)e.Response).StatusCode}" +
                        $"\nStatus Description : {((HttpWebResponse)e.Response).StatusDescription}" +
                        $"\nAutorizationsBattleNet Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                    return null;
                }
            }
            catch (Exception e)
            {

                string message = $"AutorizationsBattleNet Error: {e.Message}";
                Functions.WriteLogs(message, "error");
                return null;
            }

            return null;

        }
    }
}
