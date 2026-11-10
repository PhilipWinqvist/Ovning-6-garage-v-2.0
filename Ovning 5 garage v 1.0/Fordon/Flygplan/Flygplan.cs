namespace Garage1;

public class Flygplan : Fordon
{
    public int AntalMotorer { get; }

    public Flygplan(string registreringsnummer, string farg, int antalHjul, int antalMotorer)
        : base(registreringsnummer, farg, antalHjul)
    {
        AntalMotorer = antalMotorer;
    }

    public override string HamtaSpecialInfo()
    {
        return $"{AntalMotorer} motorer";
    }
}
