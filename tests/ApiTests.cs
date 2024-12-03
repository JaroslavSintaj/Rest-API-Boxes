using NUnit.Framework;
using System.Threading.Tasks;
using System;
using System.IO;


[TestFixture]
public class ApiTests
{
    [SetUp]
    public void Setup(){
        
    }

    [Test]
    public async Task CanGetAccessToken()
    {   
        string accessToken = await Alza.AuthenticateAsync();
        Assert.IsNotNull(accessToken, "Access token is null.");
    }

    [Test]
    public async Task AlreadySomeReservationExists()
    {   
        string token = await Alza.AuthenticateAsync();

        string reservationList = await Alza.GetReservationsAsync(token);
        Assert.IsNotNull(reservationList, "Resevations exist.");
    }

    [Test]
    public async Task CanCreateReservationHappyCase(){
        string token = await Alza.AuthenticateAsync();

        ReturnData testData = GenerateReservationData();
        string result = await Alza.MakeReservationAsync(token, testData.data);
        Assert.That(result, Does.Contain(testData.id));
        Assert.That(result, Does.Contain("RESERVED"));
    }

    [Test]
    public async Task CanCreateReservationAlreadyExists(){
        string token = await Alza.AuthenticateAsync();

        ReturnData testData = GenerateReservationData("id_f80938f5-d089-4b37-bf75-1733231776");
        string result = await Alza.MakeReservationAsync(token, testData.data);
        Assert.That(result, Does.Contain("already exists"));
        Assert.That(result, Does.Not.Contain("RESERVED"));
    }

   static ReturnData GenerateReservationData(){
        string reservationIDBase = "id_f80938f5-d089-4b37-bf75-";
        string timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string reservationID = reservationIDBase + timeStamp;
            Console.WriteLine("generatedID: "+reservationID);
        string reservationData = GetReservationData(reservationID, timeStamp);

        return new ReturnData{
            id = reservationID,
            data = reservationData
        };
   }
    
    static ReturnData GenerateReservationData(string reservationID){
        string timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        Console.WriteLine("generatedID: "+reservationID);
        string reservationData = GetReservationData(reservationID, timeStamp);

        return new ReturnData{
            id = reservationID,
            data = reservationData
        };
    }

    static string GetReservationData(string reservationID, string barcode){
        string data = ReadFile("./data/reservationExample.txt");
        data = data.Replace("reservationID", reservationID);
        data = data.Replace("BARR", "BAR_"+barcode);

        return data;
    }
    
    static string ReadFile(string fileName){
        string filePath = fileName;

        if(File.Exists(fileName)) 
            return File.ReadAllText(filePath);
        else return "";
    }
}