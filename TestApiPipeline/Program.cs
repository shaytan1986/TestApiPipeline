using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TestApiPipeline
{
    class Program
    {
        static async void Main(string[] args)
        {

            //ExecutePipeline(requestArgs);
            Task<string> outputTask = TestRW();
            string output = outputTask.Wait();
            Console.WriteLine(output);
            Console.ReadLine();
            
        }

        static async Task<string> TestRW()
        {

            using (JsonReader reader = await GetJsonTextReader())
            {
                return await WriteFromReader(reader);
            }
        }
        
        static async Task<string> WriteFromReader(JsonReader reader)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                while (reader.Read())
                {
                    writer.WriteToken(reader, true);
                    Console.WriteLine(writer.ToString());
                }
            }

            return sw.ToString();
        }
        static async Task<JsonTextReader> GetJsonTextReader()
        {
            int ct = 5;
            string requestUri = $"http://localhost:3000/ct?{ct}";
            HttpClient client = new HttpClient();


            using (Stream s = client.GetStreamAsync(requestUri).Result)
            using (StreamReader sr = new StreamReader(s))
            {
                return new JsonTextReader(sr);
            }
        }

        //static async JsonReader Simple(int ct = 5)
        //{
        //    string requestUri = $"http://localhost:3000/ct?{ct}";
        //    HttpClient client = new HttpClient();


        //    using (Stream s = client.GetStreamAsync(requestUri).Result)
        //    using (StreamReader sr = new StreamReader(s))
        //    using (JsonReader reader = new JsonTextReader(sr))
        //    {
        //        bool foundResultToken = false;
        //        bool foundObjectStart = false;
        //        int objectDepth = int.MinValue;

        //        while (reader.Read())
        //        {
        //            reader.
        //            // Find the first object start, and record it's depth. then just scan baby scan.
        //            if (!foundObjectStart)
        //            {
        //                if (!foundResultToken
        //                    && reader.TokenType == JsonToken.PropertyName
        //                    && !string.IsNullOrEmpty(reader.Value.ToString())
        //                    && reader.Value.ToString() == "Result")
        //                {
        //                    foundResultToken = true;
        //                }
        //                else if (!foundObjectStart
        //                    && reader.TokenType == JsonToken.StartObject)
        //                {
        //                    objectDepth = reader.Depth;
        //                    foundObjectStart = true;
        //                }
        //            }
        //        }
        //    }
        //}

        //static async void ExecutePipeline(RequestArgs requestArgs)
        //{


        //    var formatQueryString = new TransformBlock<RequestArgs, string>( reqArgs =>
        //    {
        //        return $"{reqArgs.BaseUri}ct?{reqArgs.Ct}";
        //    });

        //    var executeGetAsync = new TransformBlock<string, JsonReader>(async requestUri =>
        //    {
        //        HttpClient client = new HttpClient();
        //        using (Stream s =  client.GetStreamAsync(requestUri).Result)
        //        using (StreamReader sr = new StreamReader(s))
        //        using (JsonReader reader = new JsonTextReader(sr))
        //        {
        //            return reader;
        //        }
        //    });

        //    var parseResponseStream = new TransformBlock<JsonReader, Stream>(response =>
        //    {
        //        while (response.Read())
        //        {

        //        }
        //    });

        //    var buffer = new BufferBlock<string>();
        //    var convertJsonToTable = new TransformBlock<string, DataTable>(jsonText =>
        //    {

        //    })

        //    formatQueryString.LinkTo(executeGetAsync);
        //    executeGetAsync.LinkTo(parseResponseStream);
        //    parseResponseStream
        //}
    }

    //public struct RequestArgs
    //{
    //    public readonly string BaseUri;
    //    public readonly int Ct;
    //    public readonly HttpMethod Method;

    //    public RequestArgs(string baseUri, int ct, HttpMethod method)
    //    {
    //        BaseUri = baseUri;
    //        Ct = ct;
    //        Method = method;
    //    }
    //}
}
