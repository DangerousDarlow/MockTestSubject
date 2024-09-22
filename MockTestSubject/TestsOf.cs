using System.Reflection;
using NSubstitute;

namespace MockTestSubject;

public class TestsOf<T> where T : class
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo ForMethod;

    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly Dictionary<Type, object> Mocks = new();

    static TestsOf()
    {
        ForMethod = typeof(Substitute)
                        .GetMethods()
                        .FirstOrDefault(m => m is { Name: "For", IsGenericMethod: true } && m.GetParameters().Length == 1) ??
                    throw new Exception("Failed to get Substitute.For method");
    }

    protected TestsOf()
    {
        var type = typeof(T);

        // Get constructor with most parameters
        var constructor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
        if (constructor == null)
            throw new Exception($"No constructor found for type '{type}'");

        var parameters = constructor.GetParameters();
        foreach (var parameter in parameters)
            Mocks.Add(parameter.ParameterType, CreateSubstituteFor(parameter.ParameterType));

        Subject = (T) Activator.CreateInstance(typeof(T), Mocks.Values.ToArray())!;
    }

    protected T Subject { get; }

    protected TMock GetMock<TMock>() => (TMock) Mocks[typeof(TMock)];

    private static object CreateSubstituteFor(Type type)
    {
        var genericForMethod = ForMethod.MakeGenericMethod(type);
        if (genericForMethod == null)
            throw new Exception($"Failed to get Substitute.For<T> method for type '{type}'");

        var mock = genericForMethod.Invoke(null, [Array.Empty<object>()]);
        if (mock == null)
            throw new Exception($"Failed to instantiate mock for type '{type}'");

        return mock;
    }
}