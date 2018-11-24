using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

namespace QFetcher.Services.Tests
{
    public static class TestUtils
    {   
        //There is no noraml way to comapre lists in xunit. so i had to implement myself :/
        public static void AreListObjectsJsonEqual<T>(List<T> expectedList, List<T> actualList)
        {
            Assert.Equal(expectedList.Count,actualList.Count);
            foreach (var expected in expectedList)
            {
                Assert.True(actualList.Any(actual =>

                    //comparing objects without implementing "equals", by comparing their json serlization
                    JsonConvert.SerializeObject(expected) == JsonConvert.SerializeObject(actual)
                ));
            }
        }
    }
}