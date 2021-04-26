using System;

namespace Qurry.Core.Tests
{
    public class TestFooClass
    {
        public string StringValue { get; set; } = "";

        public bool BoolValue { get; set; }

        public int IntValue { get; set; }

        public DateTime DateTimeField { get; set; }

        public TestBarClass BarValue { get; set; } = new TestBarClass();
    }
}
