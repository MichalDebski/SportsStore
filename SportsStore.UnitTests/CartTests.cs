using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // arrange
            Cart cart = new Cart();

            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            CartLine[] results = cart.Lines.ToArray();

            // assert
            Assert.AreEqual(2, results.Length);
            Assert.AreEqual(p1, results[0].Product);
            Assert.AreEqual(p2, results[1].Product);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // arrange
            Cart cart = new Cart();

            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 1);
            CartLine[] results = cart.Lines.ToArray();

            // assert
            Assert.AreEqual(2, results.Length);
            Assert.AreEqual(2, results[0].Quantity);
            Assert.AreEqual(1, results[1].Quantity);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // arrange
            Cart cart = new Cart();

            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p3, 5);
            cart.AddItem(p2, 1);

            cart.RemoveLine(p2);

            // assert
            Assert.AreEqual(2, cart.Lines.Count());
            Assert.AreEqual(0, cart.Lines.Where(p => p.Product == p2).Count());
        }

        [TestMethod]
        public void Can_Calculate_Total()
        {
            // arrange
            Cart cart = new Cart();

            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100m };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50m };

            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);

            decimal total = cart.ComputeTotalValue();

            // assert
            Assert.AreEqual(450m, total);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // arrange
            Cart cart = new Cart();

            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100m };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50m };

            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);

            cart.Clear();

            // assert
            Assert.AreEqual(0, cart.Lines.Count());
        }
    }
}
