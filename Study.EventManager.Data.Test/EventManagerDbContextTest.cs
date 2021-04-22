using Microsoft.VisualStudio.TestTools.UnitTesting;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Study.EventManager.Data.Test
{
    [TestClass]
    public class EventManagerDbContextTest
    {
        [TestMethod]
        public void TestMethod1()
        {
         //   var context = new EventManagerDbContext("Server=SHYI;Database=EventManager;User Id=sa;Password=masterkey");

            
           // context.Database.EnsureCreated();

         //   var newCompany = new Company { Id = 2, Name = "test2" };
          //  DateTime EvebtCreatedt = new DateTime(2021, 04, 12, 0, 10, 10);
           // DateTime EventHoldingDt = new DateTime(2021, 04, 20, 17, 0, 0);
           // var newEvent = new Event { Id = 1, Name = "Drink bear", CreateDt = EvebtCreatedt, HoldingDt = EventHoldingDt, TypeId = 1, UserId = 1, 
          //                              Description = "meet" };


            //context.Set<Company>().Add(newCompany);
           /// context.Set<Event>().Add(newEvent);
           // context.SaveChanges();
            
          //  var dataCompany = context.Set<Company>().FirstOrDefault(x => x.Name == "test2" && x.Id == 2);
          //  var dataEvent = context.Set<Event>().FirstOrDefault(x => x.Name == "Drink bear" && x.HoldingDt == EventHoldingDt);
          //  Assert.IsNotNull(dataCompany);
          //  Assert.IsNotNull(dataEvent);
        }
    }
}
