namespace NUnitTests;

public class Subject(ICalculator calculator)
{
    public static int Add(int a, int b) => a + b;

    public int Multiply(int a, int b) => calculator.Multiply(a, b);
}

public interface ICalculator
{
    int Multiply(int a, int b);
}