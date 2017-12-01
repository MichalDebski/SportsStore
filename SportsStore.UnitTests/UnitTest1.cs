using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<IProductsRepository> GetMockRepo()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product { ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product { ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product { ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product { ProductID = 5, Name = "P5", Category = "Cat3"}
            });

            return mock;
        }

        [TestMethod]
        public void Can_Paginate()
        {
            // arrange
            Mock<IProductsRepository> mock = GetMockRepo();

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");

        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // arrange
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                ItemsPerPage = 10,
                TotalItems = 28
            };

            Func<int, string> pageUrlDelegate = i => "Strona" + i;

            // act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Strona1"">1</a>" +
                            @"<a class=""btn btn-default btn-primary selected"" href=""Strona2"">2</a>" +
                            @"<a class=""btn btn-default"" href=""Strona3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Paginaiton_View_Model()
        {
            // arrange
            Mock<IProductsRepository> mock = GetMockRepo();

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // arrange
            Mock<IProductsRepository> mock = GetMockRepo();
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // act
            ProductsListViewModel result = (ProductsListViewModel)controller.List("Cat2", 1).Model;

            // assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.IsTrue(prodArray[0].Name == "P2" && prodArray[0].Category == "Cat2");
            Assert.IsTrue(prodArray[1].Name == "P4" && prodArray[1].Category == "Cat2");

        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // arrange
            Mock<IProductsRepository> mock = GetMockRepo();
            NavController controller = new NavController(mock.Object);

            // act
            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();


            // assert
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(result[0], "Cat1");
            Assert.AreEqual(result[1], "Cat2");
            Assert.AreEqual(result[2], "Cat3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IProductsRepository> mock = GetMockRepo();

            NavController controller = new NavController(mock.Object);


            string categoryToSelect = "Cat2";

            string result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductsRepository> mock = GetMockRepo();
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            int res1 = ((ProductsListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2= ((ProductsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(2, res1);
            Assert.AreEqual(2, res2);
            Assert.AreEqual(1, res3);
            Assert.AreEqual(5, resAll);
        }
    }
}
