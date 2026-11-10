namespace Garage1;

public abstract class Fordon{

    public string Registreringsnummer { get; }
    public string Farg { get; }
    public int AntalHjul { get; }

    protected Fordon(string registreringsnummer, string farg, int antalHjul)
    {

        Registreringsnummer = registreringsnummer.Trim().ToUpper();
        Farg = farg.Trim();
        AntalHjul = antalHjul;
    }

    public abstract string HamtaSpecialInfo();

    public override string ToString()
    {
        return $"{GetType().Name,-11} {Registreringsnummer,-7} {Farg,-8} {AntalHjul} hjul, {HamtaSpecialInfo()}";
    }
}
