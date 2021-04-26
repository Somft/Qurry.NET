
using Microsoft.EntityFrameworkCore;

namespace Qurry.Core.Tests
{
    public class TestDbContext
    {
        public DbSet<TestFooClass> Foos { get; set; } = null!;

        public DbSet<TestBarClass> Bars { get; set; } = null!;
    }
}
