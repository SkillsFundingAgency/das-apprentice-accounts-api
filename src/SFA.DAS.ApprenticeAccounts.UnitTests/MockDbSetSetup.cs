using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.UnitTests;
public static class MockDbSetSetup
{
    public static Mock<DbSet<T>> CreateQueryableMockDbSet<T>(IEnumerable<T> data) where T : class
    {
        var queryable = data.AsQueryable();

        var mockDbSet = new Mock<DbSet<T>>();
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        return mockDbSet;
    }
}
