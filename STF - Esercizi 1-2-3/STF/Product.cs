using STF;

public class Product : ColumnFixture
{
    public float x;
    public float y;
    public float result()
    {
        /* Insert code here */
        return x * y;
    }
    public override bool Check(Row row)
    {
        this.x = (float)row[0];
        this.y = (float)row[1];
        return (result() == (float)row[2]);
    }
}
