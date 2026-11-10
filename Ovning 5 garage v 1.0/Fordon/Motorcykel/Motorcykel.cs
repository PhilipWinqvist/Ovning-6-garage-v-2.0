namespace Garage1;

public class Motorcykel : Fordon
{
    public int Cylindervolym { get; }   

    public Motorcykel(string registreringsnummer, string farg, int antalHjul, int cylindervolym)
        : base(registreringsnummer, farg, antalHjul)
    {
        Cylindervolym = cylindervolym;
    }

    public override string HamtaSpecialInfo()
    {
        return $"cylindervolym {Cylindervolym} cc";
    }
}
