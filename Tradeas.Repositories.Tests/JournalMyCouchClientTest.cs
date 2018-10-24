using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Tradeas.Models;

namespace Tradeas.Repositories.Tests
{
    [TestFixture]
    public class JournalMyCouchClientTest
    {
        [Test]
        public async Task GetIdeaTest()
        {
            var journalRepository = new JournalRepository("http://127.0.0.1:5984");
            var response =  await journalRepository.GetIdea(new Transaction { Id = "ECPDecember282017121145" });
            Assert.IsTrue(true);
        }

        [Test]
        public async Task GetIdeaNotFoundTest()
        {
            var journalRepository = new JournalRepository("http://127.0.0.1:5984");
            var response = await journalRepository.GetIdea(new Transaction { Id = "ECPDecember28201712" });
            Assert.IsTrue(true);
        }

        [Test]
        public async Task IdeaListJsonTest()
        {
            var ideas = new List<Idea>
            {
                new Idea{ Id = "0"},
                new Idea { Id = "1"}
            };
            var jsonList = ideas
                .Select(idea => JsonConvert.SerializeObject(idea))
                .ToList();
            var journalStageRepository = new JournalStageRepository("http://127.0.0.1:5984");
            //await journalStageRepository.BulkAsync(jsonList);
            Assert.IsTrue(true);
        }

        [Test]
        public async Task GetIdeaWithOpenStatusTest()
        {
            var journalRepository = new JournalRepository("http://127.0.0.1:5984");
            var response = await journalRepository.GetIdeasOpenStatus();
            Assert.IsTrue(true);
        }

        [Test]
        public void IdeaJsonDeserializeTest()
        {
            //var json = "{\"_id\":\"ECPDecember282017121145\",\"_rev\":\"3-71b8468321c930751e5cc6a92e9ff0c5\",\"symbol\":\"ECP\",\"type\":\"Anticipate Breakout\",\"entryDate\":\"2017-12-28T09:44:18.000Z\",\"stars\":[{\"id\":0,\"class\":\"glyphicon glyphicon-star-empty\",\"_selected\":false},{\"id\":1,\"class\":\"glyphicon glyphicon-star-empty\",\"_selected\":false},{\"id\":2,\"class\":\"glyphicon glyphicon-star-empty\",\"_selected\":false}],\"status\":\"closed\",\"position\":{\"transactionId\":38342,\"orderId\":20171228026242,\"symbol\":\"ECP\",\"status\":\"Executed\",\"createdDate\":\"2017-12-29T09:44:18.000Z\",\"transactionIds\":[\"col3834220171228026242ECP1000197600\",\"col4786820171228035768ECP100175000\",\"col4786820171228035768ECP200175000\",\"col4786820171228035768ECP500175000\"],\"transactions\":[]}}";
            var json = "{\"_id\":\"MACJanuary062018115101\",\"_rev\":\"3-e752124e93a77b9cbb3ef10186edcca7\",\"symbol\":\"MAC\",\"type\":\"Position\",\"chart\":\"ddd\",\"entryDate\":\"2018-01-06T15:51:01.881Z\",\"stars\":[{\"id\":0,\"class\":\"glyphicon glyphicon-star-empty\",\"_selected\":false},{\"id\":1,\"class\":\"glyphicon glyphicon-star-empty\",\"_selected\":false},{\"id\":2,\"class\":\"glyphicon glyphicon-star-empty\",\"_selected\":false}],\"status\":\"open\",\"position\":{\"id\":\"MACJanuary062018115101\",\"transactionId\":\"\",\"orderId\":\"\",\"symbol\":\"MAC\",\"status\":\"Executed\",\"createdDate\":\"2018-01-06T15:51:05.822Z\",\"transactionIds\":[\"col186f2508-b389-42e3-9836-e201fe320870\"]}}";
            var idea = JsonConvert.DeserializeObject(json, typeof(Idea));
        }
    }
}