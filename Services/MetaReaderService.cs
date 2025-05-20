using System.Net.Http;

namespace Radioc.CastingUtils
{
    public class MetaReaderService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MetaReaderService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<string> GetMetaDataFromIceCastStream(string url)
        {

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Icy-MetaData", "1");
            var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            httpClient.DefaultRequestHeaders.Remove("Icy-MetaData");
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<string> headerValues;
                if (response.Headers.TryGetValues("icy-metaint", out headerValues))
                {
                    string metaIntString = headerValues.First();
                    if (!string.IsNullOrEmpty(metaIntString))
                    {
                        int metadataInterval = int.Parse(metaIntString);
                        byte[] buffer = new byte[metadataInterval + 10];
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {

                            int numBytesRead = 0;
                            int numBytesToRead = metadataInterval;
                            do
                            {

                                int n = stream.Read(buffer, numBytesRead, 10);
                                numBytesRead += n;
                                numBytesToRead -= n;
                            } while (numBytesToRead > 0);

                            int lengthOfMetaData = stream.ReadByte();
                            int metaBytesToRead = lengthOfMetaData * 16;
                            byte[] metadataBytes = new byte[metaBytesToRead];
                            var bytesRead = await stream.ReadAsync(metadataBytes, 0, metaBytesToRead);
                            var metaDataString = System.Text.Encoding.Default.GetString(metadataBytes);
                            var meta = metaDataString.Split("'")[1];

                            return meta.Length > 32 ? meta.Substring(0, 32) : meta;
                        }
                    }
                }
            }

            return "";
        }


    }
}
