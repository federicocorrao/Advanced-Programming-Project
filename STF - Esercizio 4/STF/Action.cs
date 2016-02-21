using STF;

// sono stati aggiunti Accumulator e product, sqrt a posteriori

class Accumulator
{
    private float value;
    public float add(float v)
    {
        this.value += v;
        return this.value;
    }
}

public class Action : ActionFixture
{
    float product(float a, float b) { return a * b; }
    float sqrt(float x) { return (float)System.Math.Sqrt(x); }

    public override bool Run()
    {
        float _;
        Accumulator acc = new Accumulator();
        _ = product(12, 12);
        _ = acc.add(_);
        _ = product(7, 7);
        _ = acc.add(_);
        _ = sqrt(_);
        System.Console.WriteLine(_);
        return (_ == 13.8924);
    }
}
