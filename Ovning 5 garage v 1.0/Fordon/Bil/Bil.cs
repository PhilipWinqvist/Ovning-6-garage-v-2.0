namespace Garage1;

public enum Bransletyp
{
    Bensin,
    Diesel
}

public class Bil : Fordon
{
    public Bransletyp Bransle { get; }

    public Bil(string registreringsnummer, string farg, int antalHjul, Bransletyp bransle)
        : base(registreringsnummer, farg, antalHjul)
    {
        Bransle = bransle;
    }

    public override string HamtaSpecialInfo()
    {
        return $"bränsle {Bransle}";
    }
}
