using MockTestSubject;
using NSubstitute;

namespace NUnitTests;

public class SubjectTests : TestsOf<Subject>
{
    [Test]
    public void Can_be_instantiated() => Assert.That(Subject, Is.Not.Null);

    [Test]
    public void Static_function() => Assert.That(Subject.Add(1, 2), Is.EqualTo(3));

    [Test]
    public void Interface_dependency_can_be_mocked()
    {
        GetMock<ICalculator>().Multiply(3, 4).Returns(12);
        Assert.That(Subject.Multiply(3, 4), Is.EqualTo(12));
    }
}