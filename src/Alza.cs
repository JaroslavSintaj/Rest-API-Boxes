using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;

public class Alza{

    public static string authorizationHost = "identitymanagement.phx-test.alza.cz";
    public static string boxesHost = "parcellockersconnector-test.alza.cz";

    public static async Task<string> AuthenticateAsync(SimpleLogger logger)
    {
        logger.LogInfo("run AuthenticateAsync");
        using var client = new HttpClient();
        var url = "https://"+authorizationHost+"/connect/token";

        var data = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>( "grant_type", "password" ),
            new KeyValuePair<string, string>( "scope", "konzole_access" ),
            new KeyValuePair<string, string>( "client_id", "absintaj_client" ),
            new KeyValuePair<string, string>( "client_secret", "*g@sR%uaXJCc#0yr" ),
            new KeyValuePair<string, string>( "username", "Partner AbSintaj" ),
            new KeyValuePair<string, string>( "password", "HA7^3pc@Kt8UFtQU" )
        };

        var requestContent = new FormUrlEncodedContent(data);
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = requestContent
        };
        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Host = Alza.authorizationHost; 
        var contentLength = await request.Content.ReadAsStringAsync();
        request.Content.Headers.ContentLength = contentLength.Length;  

        var response = await client.PostAsync(url, requestContent);
        try{
        response.EnsureSuccessStatusCode();
        }catch(Exception e){
            logger.LogError("AuthenticateAsync error: "+e.Message);
            string message = await response.Content.ReadAsStringAsync();
            logger.LogError("AuthenticateAsync message: "+message);
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        // Extrakce access_token (zjednodušeně, pro produkční použití doporučuji JSON deserializaci)
        var tokenStart = responseBody.IndexOf("\"access_token\":\"") + 16;
        var tokenEnd = responseBody.IndexOf("\"", tokenStart);
        
        return responseBody.Substring(tokenStart, tokenEnd - tokenStart);
    }

    public static async Task<string> GetBoxesAsync(string accessToken, SimpleLogger logger)
    {
        logger.LogInfo("run GetBoxesAsync");
        using var client = new HttpClient();
        var url = "https://"+Alza.boxesHost+"/parcel-lockers/v2/boxes?fields[box]=name&fields[box]=description&fields[box]=address&page[limit]=100&page[offset]=0";

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Host", Alza.boxesHost);

        var response = await client.GetAsync(url);
        try{
        response.EnsureSuccessStatusCode();
        }catch(Exception e){
            logger.LogError("GetBoxesAsync error: "+e.Message);
            string message = await response.Content.ReadAsStringAsync();
            logger.LogError("GetBoxesAsync message: "+message);
        }
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> GetReservationsAsync(string accessToken, SimpleLogger logger)
    {
        logger.LogInfo("run GetReservationsAsync");
        using var client = new HttpClient();
        var url = "https://"+Alza.boxesHost+"/parcel-lockers/v2/reservations?page[limit]=20&page[offset]=0&fields[reservation]=openerKey";

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Host", Alza.boxesHost);

        var response = await client.GetAsync(url);
        try{
        response.EnsureSuccessStatusCode();
        }catch(Exception e){
            logger.LogError("GetReservationsAsync error: "+e.Message);
            string message = await response.Content.ReadAsStringAsync();
            logger.LogError("GetReservationsAsync message: "+message);
        }
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> GetReservationByIDAsync(string accessToken, string resevationID, SimpleLogger logger)
    {
        logger.LogInfo("run GetReservationByIDAsync");
        using var client = new HttpClient();
        var url = "https://"+Alza.boxesHost+"/parcel-lockers/v2/reservation?filter[id]="+resevationID+"&fields[reservation]=blocker&fields[reservation]=openerKey";

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Host", Alza.boxesHost);

        var response = await client.GetAsync(url);
        try{
        response.EnsureSuccessStatusCode();
        }catch(Exception e){
            logger.LogError("GetReservationByIDAsync error: "+e.Message);
            string message = await response.Content.ReadAsStringAsync();
            logger.LogError("GetReservationByIDAsync message: "+message);
        }

        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> MakeReservationAsync(string accessToken, string data, SimpleLogger logger)
    {
        logger.LogInfo("run MakeReservationAsync");
        using var client = new HttpClient();
        var url = "https://"+Alza.boxesHost+"/parcel-lockers/v2/reservation";

        var requestContent = new StringContent(data, Encoding.UTF8);
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = requestContent
        };
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Host = Alza.authorizationHost; 
        var contentLength = await request.Content.ReadAsStringAsync();
        request.Content.Headers.ContentLength = contentLength.Length;  

        var response = await client.PostAsync(url, requestContent);
        try{
            response.EnsureSuccessStatusCode();
        }catch(Exception e){
            logger.LogError("MakeReservationAsync error: "+e.Message);
            
            string message = await response.Content.ReadAsStringAsync();
            logger.LogError("MakeReservationAsync message: "+message);
        }

        return await response.Content.ReadAsStringAsync();
    }

    //Under construction
    public static async Task<string> CancelReservationAsync(string accessToken, string dataReservationID)
    {
        using var client = new HttpClient();
        var url = "https://"+Alza.boxesHost+"/parcel-lockers/v2/reservation";

        var request = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Content = new StringContent(dataReservationID, Encoding.UTF8, "application/json")
        };
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        //request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        //request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        request.Headers.Host = Alza.authorizationHost; 
        var contentLength = await request.Content.ReadAsStringAsync();
        Console.WriteLine("Size: "+contentLength.Length);
        request.Content.Headers.ContentLength = contentLength.Length;  

        //var response = await client.PostAsync(url, requestContent);
        var response = await client.SendAsync(request);
        //response.EnsureSuccessStatusCode();
        Console.WriteLine($"Response Status Code: {response.StatusCode}");

        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
        return await response.Content.ReadAsStringAsync();
    }
}

public class ReturnData{
    public string id {get; set;}
    public string data {get; set;}
}