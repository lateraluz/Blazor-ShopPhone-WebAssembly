using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Util;

public static class Util
{
    public static bool IsValidJson(string json)
    {
        try
        {
            var utf8 = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
            JsonDocument.TryParseValue(ref utf8, out JsonDocument? document);
            return document != null;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public static string GetStandarErrorMessages(string json)
    {
        try
        {
            var items = JsonNode.Parse(json);
            string title = items!["title"]!.ToString();
            string traceId = items!["traceId"]!.ToString();
            string jsonErrors = "";
            if (json.Contains("errors"))
                jsonErrors = items!["errors"]!.ToJsonString();

            return $"{title} - {jsonErrors}";
        }
        catch (Exception e)
        {
            Exception ex = e;
            throw;
        }
    }

}
