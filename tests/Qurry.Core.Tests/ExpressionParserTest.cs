
using Microsoft.EntityFrameworkCore;

using Qurry.Core.Query;

using Xunit;

namespace Qurry.Core.Tests
{
    public class ExpressionParserTest
    {
        [Fact]
        public void Test()
        {
            var parser = new QueryParser();
            var resolver = new DbContextPropertyResolver<TestDbContext>();
            var expParser = new ExpressionParser(parser, resolver);

            System.Func<TestClass, bool>? expression = expParser.ParseExpression<TestClass>(
                 "StringValue = 'helloworld'"
                 ).Compile();

            Assert.True(expression.Invoke(new TestClass() { StringValue = "helloworld" }));
            Assert.False(expression.Invoke(new TestClass() { StringValue = "invalid_value" }));
        }

        [Fact]
        public void ComplicatedQueryTest()
        {
            var parser = new QueryParser();
            var resolver = new DbContextPropertyResolver<TestDbContext>();
            var expParser = new ExpressionParser(parser, resolver);

            System.Func<TestClass, bool>? expression = expParser.ParseExpression<TestClass>(
                "BoolValue or 10 - 1 / 5 * 2 + 1 - 5 * 10= 2  and false or false and 5.3 > 10.2 and 'asd' = 'asd'"
                ).Compile();

            Assert.True(expression.Invoke(new TestClass() { BoolValue = true }));
            Assert.False(expression.Invoke(new TestClass() { BoolValue = false }));
        }

        private class TestDbContext
        {
            public DbSet<TestClass> Table { get; set; } = null!;
        }

        private class TestClass
        {
            public string StringValue { get; set; } = "";

            public bool BoolValue { get; set; }

            public int IntValue { get; set; }

        }
    }
}
