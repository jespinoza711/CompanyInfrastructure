using System;
using System.Collections.Generic;
using System.Linq;
using Company.Infrastructure.BusinessLayer;
using Company.Infrastructure.Domain;

namespace Company.BusinessLayer.Mock
{
    public class MockProductLogic : IProductLogic
    {
        private readonly IEnumerable<Product> testStore;

        public MockProductLogic()
        {
            this.testStore = new List<Product> 
            {
                new Product
                {
                    ProductID = 1,
                    Name = "Bratwurst",
                    ProductNumber = "BR-0001",
                    MakeFlag = true,
                    FinishedGoodsFlag = true,
                    Color = "Kross-Braun",
                    SafetyStockLevel = 3000,
                    ReorderPoint = 1000,
                    StandardCost = 0.75m,
                    ListPrice = 1.75m,                        
                    WeightUnitMeasureCode = "g",
                    Weight = 125,
                    DaysToManufacture = 1,
                    ProductLine = "FastFood",
                    Class = "Food",                        
                    ProductModelID = null,
                    SellStartDate = DateTime.Now,
                    SellEndDate = DateTime.Now.AddYears(1)
                }
            };
        }

        #region IProductLogic Members

        public IEnumerable<int> GetProductIds()
        {
            return this.testStore.Select(p => p.ProductID).ToArray();
        }

        public Product GetProductById(int id)
        {
            return this.testStore.FirstOrDefault(p => p.ProductID == id);
        }

        #endregion
    }
}
