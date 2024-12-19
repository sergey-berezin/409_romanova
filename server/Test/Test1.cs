using System.Diagnostics;
using System.Net;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PackagingGenetic;
using WebApp;

namespace Test
{
    public class WebAppControllerTests : IClassFixture<WebApplicationFactory<WebApp.Startup>>
    {
        readonly HttpClient _client;
        private List<Experiment> ExList;

        public WebAppControllerTests(WebApplicationFactory<WebApp.Startup> application)
        {
            _client = application.CreateClient();
            ExList = new();
        }

        [Fact]
        public async Task GET_all_test()
        {
            ExList.Clear();
            var Response = await _client.GetAsync("api/experiments/all");
            Response.StatusCode.Should().Be(HttpStatusCode.OK);

            string ResponseStr = await Response.Content.ReadAsStringAsync();
            List<string> ?Names = JsonConvert.DeserializeObject<List<string>>(ResponseStr);
         
            foreach (var Name in Names)
            {
                var ExResponse = await _client.GetAsync("api/experiments/" + Name);
                ExResponse.StatusCode.Should().Be(HttpStatusCode.OK);
                string ExResponseStr = await ExResponse.Content.ReadAsStringAsync();
                Experiment? Ex = JsonConvert.DeserializeObject<Experiment>(ExResponseStr);
                Ex.Should().NotBeNull();
                if (Ex == null)
                {
                    return;
                }
                ExList.Add(Ex);
            }
        }

        [Fact]
        public async Task PUT_test()
        {
            await GET_all_test();

            var c = new StringContent("");
            var Response = await _client.PutAsync("api/experiments/1/2/3/10", c);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);

            string ResponseStr = await Response.Content.ReadAsStringAsync();
            ResponseStr.Should().Be("ex" + ExList.Count.ToString());
        }

        [Fact]
        public async Task POST_test()
        {
            await GET_all_test();
            string Name = ExList[0].ExName;
            //string Name = "ex1";
            var c = new StringContent("");
            var Response = await _client.PostAsync("api/experiments/" + Name, c);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);

            string ResponseStr = await Response.Content.ReadAsStringAsync();
            Genotype BestGen = JsonConvert.DeserializeObject<Genotype>(ResponseStr);
            BestGen.Should().NotBeNull();
        }

        [Fact]
        public async Task DELETE_test()
        {
            await GET_all_test();
            int Cnt = ExList.Count;

            string Name = ExList[0].ExName;
            //string Name = "ex1";
            var Response = await _client.DeleteAsync("api/experiments/" + Name);
            Response.StatusCode.Should().Be(HttpStatusCode.OK);

            await GET_all_test();
            Cnt.Should().Be(ExList.Count + 1);
        }
    }
}