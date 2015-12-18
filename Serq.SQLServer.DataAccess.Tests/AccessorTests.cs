using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Serq.SQLServer.DataAccess.Tests
{
    [TestClass]
    public class AccessorTests
    {
        [TestMethod]
        public void Read()
        {
            Accessor accessor = new Accessor(@"Server=(localdb)\v11.0;Integrated Security=true;Initial Catalog=Deneme");
            var objects = accessor.Read("Select * from Names");
        }

        [TestMethod]
        public void Put()
        {
            Accessor accessor = new Accessor(@"Server=(localdb)\v11.0;Integrated Security=true;Initial Catalog=Deneme");
            var result = accessor.Put(@"INSERT INTO [dbo].[Names]
           ([Name]
           ,[LastName]
           ,[Number])
     VALUES
           ('Ahmet'
           , 'Senol'
           , 8)");
        }
    }
}
