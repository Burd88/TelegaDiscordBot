using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace DiscordBot
{
    class GetToken
    {
        public Token GetTokenPrice()
        {

            try
            {
                WebRequest request = WebRequest.Create("https://eu.api.blizzard.com/data/wow/token/index?namespace=dynamic-eu&locale=ru_RU&access_token=" + Program.tokenWow);
                WebResponse responce = request.GetResponse();

                using (System.IO.Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {



                            return JsonConvert.DeserializeObject<Token>(line);
                        }
                    }
                }
                responce.Close();

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {

                    string message = $"GetTokenPrice Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                    return null;
                }
            }
            catch (Exception e)
            {

                string message = $"GetTokenPrice Error: {e.Message}";
                Functions.WriteLogs(message, "error");
                return null;
            }

            return null;
        }
    }
}
public class Self
{
    public string href { get; set; }
}

public class Links
{
    public Self self { get; set; }
}

public class Token
{
    public Links _links { get; set; }
    public long last_updated_timestamp { get; set; }
    public long price { get; set; }
}

