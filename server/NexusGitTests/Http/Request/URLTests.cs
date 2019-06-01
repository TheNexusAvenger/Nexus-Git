/*
 * TheNexusAvenger
 * 
 * Test the NexusGit.Http.Request.URL class.
 */

using NexusGit.Http.Request;
using NUnit.Framework;

namespace NexusGitTests.Http.Request
{
    [TestFixture]
    public class URLTests
    {
        private URL CuT1;
        private URL CuT2;
        private URL CuT3;
        private URL CuT4;
        private URL CuT5;

        /*
         * Sets up the components under testing for the test.
         */
        [SetUp]
        public void SetUpComponentsUnderTesting()
        {
            CuT1 = URL.FromString("www.google.com/");
            CuT2 = URL.FromString("www.google.com/arguments?thing0&thing1=true");
            CuT3 = URL.FromString("/");
            CuT4 = URL.FromString("/?thing1=true&thing0&thing2=false&thing3");
            CuT5 = URL.FromString("/test/");
        }

        /*
         * Tests the GetBaseURL method.
         */
        [Test]
        public void GetBaseURLTest()
        {
            // Assert the base URLs are valid.
            Assert.AreEqual(CuT1.GetBaseURL(),"www.google.com");
            Assert.AreEqual(CuT2.GetBaseURL(),"www.google.com/arguments");
            Assert.AreEqual(CuT3.GetBaseURL(),"");
            Assert.AreEqual(CuT4.GetBaseURL(),"");
            Assert.AreEqual(CuT5.GetBaseURL(),"test");
        }

        /*
         * Tests the ParameterExistsTest method.
         */
        [Test]
        public void ParameterExistsTest()
        {
            // Assert the parameters for CuT1.
            Assert.IsFalse(CuT1.ParameterExists("thing0"));
            Assert.IsFalse(CuT1.ParameterExists("thing1"));
            Assert.IsFalse(CuT1.ParameterExists("thing2"));
            Assert.IsFalse(CuT1.ParameterExists("thing3"));

            // Assert the parameters for CuT2.
            Assert.IsTrue(CuT2.ParameterExists("thing0"));
            Assert.IsTrue(CuT2.ParameterExists("thing1"));
            Assert.IsFalse(CuT2.ParameterExists("thing2"));
            Assert.IsFalse(CuT2.ParameterExists("thing3"));

            // Assert the parameters for CuT3.
            Assert.IsFalse(CuT3.ParameterExists("thing0"));
            Assert.IsFalse(CuT3.ParameterExists("thing1"));
            Assert.IsFalse(CuT3.ParameterExists("thing2"));
            Assert.IsFalse(CuT3.ParameterExists("thing3"));

            // Assert the parameters for CuT4.
            Assert.IsTrue(CuT4.ParameterExists("thing0"));
            Assert.IsTrue(CuT4.ParameterExists("thing1"));
            Assert.IsTrue(CuT4.ParameterExists("thing2"));
            Assert.IsTrue(CuT4.ParameterExists("thing3"));
        }

        /*
         * Tests the GetParameterTest method.
         */
        [Test]
        public void GetParameterTest()
        {
            // Assert the parameters for CuT1.
            Assert.AreEqual(CuT1.GetParameter("thing0"),null);
            Assert.AreEqual(CuT1.GetParameter("thing1"),null);
            Assert.AreEqual(CuT1.GetParameter("thing2"),null);
            Assert.AreEqual(CuT1.GetParameter("thing3"),null);

            // Assert the parameters for CuT2.
            Assert.AreEqual(CuT2.GetParameter("thing0"),null);
            Assert.AreEqual(CuT2.GetParameter("thing1"),"true");
            Assert.AreEqual(CuT2.GetParameter("thing2"),null);
            Assert.AreEqual(CuT2.GetParameter("thing3"),null);

            // Assert the parameters for CuT3.
            Assert.AreEqual(CuT3.GetParameter("thing0"),null);
            Assert.AreEqual(CuT3.GetParameter("thing1"),null);
            Assert.AreEqual(CuT3.GetParameter("thing2"),null);
            Assert.AreEqual(CuT3.GetParameter("thing3"),null);

            // Assert the parameters for CuT4.
            Assert.AreEqual(CuT4.GetParameter("thing0"),null);
            Assert.AreEqual(CuT4.GetParameter("thing1"),"true");
            Assert.AreEqual(CuT4.GetParameter("thing2"),"false");
            Assert.AreEqual(CuT4.GetParameter("thing3"),null);
        }
    }
}