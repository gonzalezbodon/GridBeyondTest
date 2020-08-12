using GridBeyondTest.Controllers;
using GridBeyondTest.Controllers.Interfaces;
using GridBeyondTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace GridBeyondTest_Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Statistical calculation test in an empty database
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Statistics_EmptyDatabase()
        {
            var options = new DbContextOptionsBuilder<GridBeyondDBContext>()
                .UseSqlServer("[REAL_STRING_CONNECTION]").Options;

            using (var context = new GridBeyondDBContext(options))
            {
                AnalyzerController analyzer = new AnalyzerController(context);
                //var statistics = await analyzer.GetCommonStatistics();


                Assert.ThrowsAsync<InvalidOperationException>( () => analyzer.GetCommonStatistics());
            }
        }

        /// <summary>
        /// Statistical calculation test with a wrong connection
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Statistics_BadConnection()
        {
            var options = new DbContextOptionsBuilder<GridBeyondDBContext>()
                .UseSqlServer("Bad string connection").Options;

            using (var context = new GridBeyondDBContext(options))
            {
                AnalyzerController analyzer = new AnalyzerController(context);
                //var statistics = await analyzer.GetCommonStatistics();


                Assert.ThrowsAsync<ArgumentException>(() => analyzer.GetCommonStatistics());
            }
        }

        /// <summary>
        /// Statistical calculation test in a correct environment 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Statistics_FullDatabase()
        {
            var options = new DbContextOptionsBuilder<GridBeyondDBContext>()
                .UseSqlServer("[REAL_STRING_CONNECTION]").Options;

            using (var context = new GridBeyondDBContext(options))
            {
                AnalyzerController analyzer = new AnalyzerController(context);
                var statistics = await analyzer.GetCommonStatistics();


                Assert.IsTrue(statistics.TotalItems == 3133);
            }
        }
    }
}