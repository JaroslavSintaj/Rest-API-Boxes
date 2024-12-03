using NUnit.Framework;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System;


[TestFixture]
public class ApiTests
{
    private static string token;

    [SetUp]
    public void Setup(){
        
    }

    [Test]
    public async Task Authenticate_ShouldRetrieveAccessToken()
    {   
        string accessToken = await Alza.AuthenticateAsync();
        token = accessToken;

        Assert.IsNotNull(accessToken, "Access token is null.");
    }

    [Test]
    public async Task IsSomeReservationExists()
    {   
        Assert.IsNotNull(ApiTests.token, "Invalid access token.");

        string reservationList = await Alza.GetReservaionAsync(ApiTests.token);
        Console.WriteLine(reservationList);
        Assert.IsNotNull(reservationList, "Resevations exist.");
    }
}
