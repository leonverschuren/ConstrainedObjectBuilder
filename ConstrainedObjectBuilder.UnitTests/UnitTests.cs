using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConstrainedObjectBuilder.UnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void ShouldBeGreater_MultipleConstraints_ConstraintsAreResolved()
        {
            var result = new DataBuilder()
                .ShouldBeGreater(d => d.Date2, d => d.Date1)
                .ShouldBeGreater(d => d.Date3, d => d.Date1)
                .ShouldBeGreater(d => d.Date3, d => d.Date2)
                .Build();

            Assert.IsTrue(result.Date2 > result.Date1);
            Assert.IsTrue(result.Date3 > result.Date1);
            Assert.IsTrue(result.Date3 > result.Date2);
        }

        [TestMethod]
        public void Constraint_MultipleGreaterThanConstraintExpressions_ConstraintsAreResolved()
        {
            var result = new DataBuilder()
                .Constraint(d => d.Date2 > d.Date1)
                .Constraint(d => d.Date3 > d.Date1)
                .Build();

            Assert.IsTrue(result.Date2 > result.Date1);
            Assert.IsTrue(result.Date3 > result.Date1);
        }

        [TestMethod]
        public void Constraint_SingleSmallerThanConstraintExpression_ConstraintsAreResolved()
        {
            var result = new DataBuilder().Constraint(d => d.Date2 < d.Date1).Build();

            Assert.IsTrue(result.Date2 < result.Date1);
        }
    }
}
