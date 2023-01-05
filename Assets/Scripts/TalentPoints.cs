public class Valuable
{
    protected double value;

   
    static public implicit operator double (Valuable valuable) => valuable.value;
}
public class TalentPoints
{
    protected double value;


    static public implicit operator double (TalentPoints points) => points.value;
}
