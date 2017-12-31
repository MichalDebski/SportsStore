using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            // arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "sekret")).Returns(true);

            LoginViewModel model = new LoginViewModel()
            {
                UserName = "admin",
                Password = "sekret"
            };

            AccountController target = new AccountController(mock.Object);

            // act
            ActionResult result = target.Login(model, "/MyUrl");

            // assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Canot_Login_With_Invalid_Credentials()
        {
            // arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("nieprawidłowyUżytkownik", "nieprawidłoweHasło")).Returns(false);

            LoginViewModel model = new LoginViewModel()
            {
                UserName = "nieprawidłowyUżytkownik",
                Password = "nieprawidłoweHasło"
            };

            AccountController target = new AccountController(mock.Object);

            // act
            ActionResult result = target.Login(model, "/MyUrl");

            // assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
