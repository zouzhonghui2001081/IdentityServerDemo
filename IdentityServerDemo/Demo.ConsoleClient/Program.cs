using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

Console.WriteLine("Console Client with resource owner password started...");

using (var httpClientWithResourceOwnerPassword = new HttpClient())
{
    await httpClientWithResourceOwnerPassword.HandleToken(
        "http://localhost:5999",
        "client.resourceowner",
        "secret",
        "ReourceOwner1",
        "Admin123",
        "resourceServerScope1 resourceServerScope2");

    var customer = new StringContent(
        JsonConvert.SerializeObject(new { Id = 1, FirstName = "Customer1", LastName = "Zou" }),
        Encoding.UTF8,
        "application/json");

    var createCustomerResponse = await httpClientWithResourceOwnerPassword.PostAsync("http://localhost:5111/api/customer", customer);
    if (createCustomerResponse.IsSuccessStatusCode)
    {
        Console.WriteLine("Create Customer successfully");
    }

    var getCustomerResponse = await httpClientWithResourceOwnerPassword.GetAsync("http://localhost:5111/api/customer");
    if (getCustomerResponse.IsSuccessStatusCode)
    {
        var customersString = await getCustomerResponse.Content.ReadAsStringAsync();
        Console.WriteLine(JArray.Parse(customersString));
    }
}


Console.WriteLine("Console Client with client credential started...");

using (var httpClientWithClientCredential = new HttpClient())
{
    await httpClientWithClientCredential.HandleToken(
        "http://localhost:5999",
        "client.clientcredential",
        "secret",
        "resourceServerScope1");

    var customer = new StringContent(
        JsonConvert.SerializeObject(new { Id = 2, FirstName = "Customer2", LastName = "Zou" }),
        Encoding.UTF8,
        "application/json");

    var createCustomerResponse = await httpClientWithClientCredential.PostAsync("http://localhost:5111/api/customer", customer);
    if (createCustomerResponse.IsSuccessStatusCode)
    {
        Console.WriteLine("Create Customer successfully");
    }

    var getCustomerResponse = await httpClientWithClientCredential.GetAsync("http://localhost:5111/api/customer");
    if (getCustomerResponse.IsSuccessStatusCode)
    {
        var customersString = await getCustomerResponse.Content.ReadAsStringAsync();
        Console.WriteLine(JArray.Parse(customersString));
    }
}


Console.ReadLine();