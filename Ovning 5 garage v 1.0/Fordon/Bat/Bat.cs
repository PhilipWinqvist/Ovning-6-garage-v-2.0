namespace Garage1;

public class Bat : Fordon
{
    public double Langd { get; }   

    public Bat(string registreringsnummer, string farg, int antalHjul, double langd)
        : base(registreringsnummer, farg, antalHjul)
    {
        Langd = langd;
    }

    public override string HamtaSpecialInfo()
    {
        return $"längd {Langd:0.0} m";
    }
}
