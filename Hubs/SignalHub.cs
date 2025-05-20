using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Radioc.CastingUtils;

public class SignalHub(MetaReaderService mReader) : Hub
{
    private readonly MetaReaderService mReader = mReader;

    public async Task MetaRequest(string url)
    {
        var meta = await mReader.GetMetaDataFromIceCastStream(url);
        Console.WriteLine("SignalHub: Received url: " + url + "\n           Received meta: " + meta);
        if (meta.IsNullOrEmpty())
        {
            meta = "No additional info...";
        }
        await Clients.Caller.SendAsync("MetaResponse", meta);
    }
}