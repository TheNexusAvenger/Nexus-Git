/*
 * TheNexusAvenger
 * 
 * Test the NexusGit.SplitRequestHttp.PartialResponse class.
 */

using NexusGit.Http.Request;
using NexusGit.SplitRequestHttp;
using NUnit.Framework;

namespace NexusGitTests.SplitRequestHttp
{
    [TestFixture]
    public class PartialResponseTests
    {
        /*
         * Tests the SplitResponseTest method.
         */
        [Test]
        public void SplitResponseTest()
        {
            // Create the first component under testing.
            var CuT1 = PartialResponse.SplitResponse(Response.CreateSuccessResponse("Hello world!"),4);

            // Assert reading the responses.
            Assert.AreEqual(CuT1.GetNumberOfResponses(),3);
            Assert.AreEqual(CuT1.GetResponseFromId(0).GetResponseData(),"Hell");
            Assert.IsFalse(CuT1.AllResponsesSent());
            Assert.AreEqual(CuT1.GetResponseFromId(1).GetResponseData(),"o wo");
            Assert.IsFalse(CuT1.AllResponsesSent());
            Assert.AreEqual(CuT1.GetResponseFromId(2).GetResponseData(), "rld!");
            Assert.IsTrue(CuT1.AllResponsesSent());

            // Create the first component under testing.
            var CuT2 = PartialResponse.SplitResponse(Response.CreateSuccessResponse("Hello world!!!"),4);

            // Assert reading the responses.
            Assert.AreEqual(CuT2.GetNumberOfResponses(),4);
            Assert.AreEqual(CuT2.GetResponseFromId(0).GetResponseData(), "Hell");
            Assert.IsFalse(CuT2.AllResponsesSent());
            Assert.AreEqual(CuT2.GetResponseFromId(3).GetResponseData(), "!!");
            Assert.IsFalse(CuT2.AllResponsesSent());
            Assert.AreEqual(CuT2.GetResponseFromId(1).GetResponseData(), "o wo");
            Assert.IsFalse(CuT2.AllResponsesSent());
            Assert.AreEqual(CuT2.GetResponseFromId(2).GetResponseData(), "rld!");
            Assert.IsTrue(CuT2.AllResponsesSent());
        }
    }
}