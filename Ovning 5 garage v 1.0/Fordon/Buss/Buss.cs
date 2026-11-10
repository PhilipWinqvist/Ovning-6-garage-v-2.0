namespace Garage1;

public class Buss : Fordon
{
    public int AntalSaten { get; }

    public Buss(string registreringsnummer, string farg, int antalHjul, int antalSaten)
        : base(registreringsnummer, farg, antalHjul)
    {
        AntalSaten = antalSaten;
    }

    public override string HamtaSpecialInfo()
    {
        return $"{AntalSaten} säten";
    }
}
