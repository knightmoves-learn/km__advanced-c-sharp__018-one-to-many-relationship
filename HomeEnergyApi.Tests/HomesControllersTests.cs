using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using System.Dynamic;
using HomeEnergyApi.Models;

[TestCaseOrderer("HomeEnergyApi.Tests.Extensions.PriorityOrderer", "HomeEnergyApi.Tests")]
public class HomesControllersTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private Home testHome1 = new Home("Test1", "123 Test St.", "Test City");
    private dynamic testUsage = new ExpandoObject();
    private dynamic testUtilProv = new ExpandoObject();


    public HomesControllersTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory, TestPriority(1)]
    [InlineData("/Homes")]
    public async Task HomeEnergyApiReturnsSuccessfulHTTPResponseCodeOnGETHomes(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        Assert.True(response.IsSuccessStatusCode,
            $"HomeEnergyApi did not return successful HTTP Response Code on GET request at {url}; instead received {(int)response.StatusCode}: {response.StatusCode}");
    }

    [Theory, TestPriority(2)]
    [InlineData("admin/Homes/Location/50313")]
    public async Task ZipLocationServiceRespondsWith200CodeAndPlaceWhenGivenValidZipCode(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync(url, null);

        Assert.True((int)response.StatusCode == 200,
             $"HomeEnergyApi did not return \"200: OK\" HTTP Response Code on POST request at {url}; instead received {(int)response.StatusCode}: {response.StatusCode}");

        string responseContent = await response.Content.ReadAsStringAsync();
        bool hasPlace = responseContent.Contains("\"place name\":\"Des Moines\"");
        bool hasState = responseContent.Contains("\"state\":\"Iowa\"");


        Assert.True(hasPlace && hasState,
            $"HomeEnergyApi did not return the expected `place name` and `state` from POST request at {url}\nExpected: `place name` of `Des Moines` and `state` of `Iowa`\nReceived: {responseContent}");
    }

    [Theory, TestPriority(3)]
    [InlineData("/admin/Homes")]
    public async Task HomeEnergyApiReturns201CreatedHTTPResponseOnAddingValidHomeThroughPOST(string url)
    {
        var client = _factory.CreateClient();

        string stringtestt = JsonSerializer.Serialize(testHome1);
        string strTestHome = BuildTestHome(testHome1, testUsage, 123, testUtilProv, new List<string> { "Gas", "Electric" });
        string strUsage = JsonSerializer.Serialize(testUsage);

        HttpRequestMessage sendRequest = new HttpRequestMessage(HttpMethod.Post, url);
        sendRequest.Content = new StringContent(strTestHome,
                                                Encoding.UTF8,
                                                "application/json");

        var response = await client.SendAsync(sendRequest);
        Assert.True((int)response.StatusCode == 201,
            $"HomeEnergyApi did not return \"201: Created\" HTTP Response Code on POST request at {url}; instead received {(int)response.StatusCode}: {response.StatusCode}");

        string responseContent = await response.Content.ReadAsStringAsync();
        responseContent = responseContent.ToLower();

        bool nameMatch = responseContent.Contains("\"ownerlastname\":\"test1\"");

        string usageResponse = responseContent.Substring(responseContent.IndexOf("homeusagedata\":{"));
        usageResponse = usageResponse[0..Math.Min(usageResponse.Length - 1, usageResponse.IndexOf("utilityproviders"))];
        bool usageHasAllProps = usageResponse.Contains("\"id\":") && usageResponse.Contains("monthlyelectricusage\":") && usageResponse.Contains("hassolar\":") && usageResponse.Contains("homeid\":");
        bool electricMatch = usageResponse.Contains("\"monthlyelectricusage\":123");

        string utilProvResponse = responseContent.Substring(responseContent.IndexOf("utilityproviders"));
        bool utilHasAllProps = utilProvResponse.Contains("\"id\":") && utilProvResponse.Contains("\"providedutilities\":") && usageResponse.Contains("\"homeid\":");
        bool providedUtilitiesMatch = utilProvResponse.Contains("\"ProvidedUtilities\":[\"Gas\",\"Electric\"]");

        Assert.True(nameMatch,
            $"On returning the added home through POST HomeEnergyApi did not return the expected home with `ownerLastName` of `test1`; \n Received : {responseContent}");
        Assert.True(usageHasAllProps,
            $"On returning the added home through POST HomeEnergyApi did not return a home where `HomeUsageData` had all the expected properties(Id, MonthlyElectricUsage, HasSolar, HomeId); Received : {responseContent}");
        Assert.True(electricMatch,
            $"On returning the added home through POST HomeEnergyApi did not return a home where `HomeUsageData.MonthlyElectricUsage` had the expected value of `123`; Received : {responseContent}");
        Assert.True(utilHasAllProps,
            $"On returning the added home through POST HomeEnergyApi did not return a home where `UtilityProviders` has all the expected properties; \n Received : {responseContent}");
        Assert.True(utilHasAllProps,
            $"On returning the added home through POST HomeEnergyApi did not return a home where `UtilityProviders.ProvidedUtilities` has the expected values of `Gas` and `Electric`; \n Received : {responseContent}");
    }

    [Theory, TestPriority(4)]
    [InlineData("/Homes")]
    public async Task HomeEnergyApiReturnsHomeUsageDataOnGETHomes(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);
        string responseContent = await response.Content.ReadAsStringAsync();
        bool containsHomeUsageData = responseContent.ToLower().Contains("homeusagedata\":{");

        Assert.True(containsHomeUsageData,
            $"HomeEnergyApi did not return Homes with `HomeUsageData` properties on GET at {url}");
    }

    public static string BuildTestHome(Home home, dynamic homeUsage, int electricValue, dynamic utilProv, List<string> provUtils)
    {
        dynamic testUsage = homeUsage;
        testUsage.MonthlyElectricUsage = electricValue;
        testUsage.HasSolar = true;

        dynamic testUtilProv = utilProv;
        testUtilProv.ProvidedUtilities = provUtils;

        Home testHome = home;
        string strTestHome = JsonSerializer.Serialize(testHome);

        string strUsage = JsonSerializer.Serialize(testUsage);
        string result = strTestHome.Replace("\"HomeUsageData\":null", $"\"HomeUsageData\":{strUsage}");

        string strUtilProv = JsonSerializer.Serialize(testUtilProv).Replace("{", "").Replace("}", "");
        result = result.Replace("\"UtilityProviders\":null", $"\"UtilityProviders\":[{{{strUtilProv}}}]");

        return result;
    }
}
