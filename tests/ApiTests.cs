using NUnit.Framework;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq.Expressions;


[TestFixture]
public class ApiTests
{
    SimpleLogger logger;

    [SetUp]
    public void Setup(){ 
        logger = new SimpleLogger($"log-{DateTime.Now:yyyy-MM-dd HH:mm}");
        logger.LogInfo("Test start.");
    }

    [Test]
    public async Task CanGetAccessToken()
    {   
        string accessToken = await Alza.AuthenticateAsync(logger);
        try{
        Assert.IsNotNull(accessToken, "Access token is null.");
        }catch(AssertionException e){
            logger.LogError("Test - CanGetAccessToken: "+e.Message);
        }
    }

    [Test]
    public async Task AlreadySomeReservationExists()
    {   
        string token = await Alza.AuthenticateAsync(logger);

        string reservationList = await Alza.GetReservationsAsync(token, logger);
        try{
        Assert.IsNotNull(reservationList, "Resevations exist.");
        }catch(AssertionException e){
            logger.LogError("Test - AlreadySomeReservationExists: "+e.Message);
        }
    }

    [Test]
    public async Task CanCreateReservationHappyCase(){
        string token = await Alza.AuthenticateAsync(logger);

        ReturnData testData = GenerateReservationData(logger);
        string result = await Alza.MakeReservationAsync(token, testData.data, logger);
        try{
            Assert.That(result, Does.Contain(testData.id));
            Assert.That(result, Does.Contain("RESERVED"));
        }catch(AssertionException e){
            logger.LogError("Test - CanCreateReservationHappyCase: "+e.Message);
        }
    }

    [Test]
    public async Task CanCreateReservationAlreadyExists(){
        string token = await Alza.AuthenticateAsync(logger);

        ReturnData testData = GenerateReservationData("id_f80938f5-d089-4b37-bf75-1733231776", logger);
        string result = await Alza.MakeReservationAsync(token, testData.data, logger);
        try{
        Assert.That(result, Does.Contain("already exists"));
        Assert.That(result, Does.Not.Contain("RESERVED"));
        }catch(AssertionException e){
            logger.LogError("Test - CanCreateReservationAlreadyExists: "+e.Message);
        }
    }

   static ReturnData GenerateReservationData(SimpleLogger logger){
        string reservationIDBase = "id_f80938f5-d089-4b37-bf75-";
        string timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string reservationID = reservationIDBase + timeStamp;
            Console.WriteLine("generatedID: "+reservationID);
        string reservationData = GetReservationData(reservationID, timeStamp, logger);

        return new ReturnData{
            id = reservationID,
            data = reservationData
        };
   }
    
    static ReturnData GenerateReservationData(string reservationID, SimpleLogger logger){
        string timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        Console.WriteLine("generatedID: "+reservationID);
        string reservationData = GetReservationData(reservationID, timeStamp, logger);

        return new ReturnData{
            id = reservationID,
            data = reservationData
        };
    }

    static string GetReservationData(string reservationID, string barcode, SimpleLogger logger){
        string data = ReadFile("./data/reservationExample.txt");
        if(data == "") logger.LogWarning("GetReservationData - template file can't be loaded.");
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