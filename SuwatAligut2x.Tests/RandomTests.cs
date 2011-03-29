using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuwatAligut2x.Models;
using SuwatAligut2x.Helpers;

namespace SuwatAligut2x.Tests
{
    /// <summary>
    /// Summary description for RandomTests
    /// </summary>
    [TestClass]
    public class RandomTests
    {
        public RandomTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateNewUserTest()
        {
            string openid = "myopenid";
            string friendlyopenid = "friendlyopenid";
            string email = "hunter012hk@gmail.com";

            UsersModels user = new UsersModels();
            user = user.CreateNewUser(openid, friendlyopenid, email);

            Assert.AreEqual(openid, user.OpenId[0].OpenId);
            Assert.AreEqual(friendlyopenid, user.OpenId[0].FriendlyOpenId);
        }

        [TestMethod]
        public void GetRandomStringTest()
        {
            int length = 10;
            string str = PostHelper.GetRandomString(length);

            Assert.AreEqual(length, str.Length);
        }
    }
}
