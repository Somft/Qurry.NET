
using Qurry.Core.Query;

using System;
using System.Linq.Expressions;

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

            Func<TestFooClass, bool> expression = expParser.ParseExpression<TestFooClass>(
                 "StringValue = 'hello_world'"
                 ).Compile();

            Assert.True(expression.Invoke(new TestFooClass() { StringValue = "hello_world" }));
            Assert.False(expression.Invoke(new TestFooClass() { StringValue = "invalid_value" }));
        }

        [Fact]
        public void ComplicatedQueryTest()
        {
            var parser = new QueryParser();
            var resolver = new DbContextPropertyResolver<TestDbContext>();
            var expParser = new ExpressionParser(parser, resolver);

            Func<TestFooClass, bool> expression = expParser.ParseExpression<TestFooClass>(
                "BoolValue or 10 - (1 / 5 * 2) + 1 - (5 + 8 - 2 * (2 + 2)) * 10= 2  and false or false and 5.3 > 10.2 and 'asd' = \"asd\""
                ).Compile();

            Assert.True(expression.Invoke(new TestFooClass() { BoolValue = true }));
            Assert.False(expression.Invoke(new TestFooClass() { BoolValue = false }));
        }

        [Fact]
        public void NestedFieldsQueryTest()
        {
            var parser = new QueryParser();
            var resolver = new DbContextPropertyResolver<TestDbContext>();
            var expParser = new ExpressionParser(parser, resolver);

            Expression<Func<TestFooClass, bool>> expression = expParser.ParseExpression<TestFooClass>(
              "BarValue.StringField = 'TEST'"
              );

            Func<TestFooClass, bool> lambda = expression.Compile();
            
            Assert.True(lambda.Invoke(new TestFooClass { BarValue = new TestBarClass
            {
                StringField = "TEST",
            } }));
            Assert.False(lambda.Invoke(new TestFooClass { BarValue = new TestBarClass
            {
                StringField = "NOT_TEST",
            } }));
        }

        [Fact]
        public void DateTimeQueryTest()
        {
            var parser = new QueryParser();
            var resolver = new DbContextPropertyResolver<TestDbContext>();
            var expParser = new ExpressionParser(parser, resolver);

            string dateTimeValue = "2020-12-20";

            Expression<Func<TestFooClass, bool>> expression = expParser.ParseExpression<TestFooClass>(
              $"DateTimeField >= '{dateTimeValue}'"
              );

            Func<TestFooClass, bool> lambda = expression.Compile();

            Assert.True(lambda.Invoke(new TestFooClass
            {
                DateTimeField = DateTime.Parse(dateTimeValue)
            }));
            Assert.True(lambda.Invoke(new TestFooClass
            {
                DateTimeField = DateTime.Parse(dateTimeValue).AddHours(1)
            }));
            Assert.False(lambda.Invoke(new TestFooClass
            {
                DateTimeField = DateTime.Parse(dateTimeValue).AddHours(-1)
            }));
        }
    }
}
