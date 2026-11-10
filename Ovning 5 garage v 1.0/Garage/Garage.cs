namespace Garage1;

public class Garage
{
    private readonly List<Fordon> fordon = new List<Fordon>();
    private readonly int kapacitet;

    public int Kapacitet => kapacitet;
    public int AntalFordon => fordon.Count;

    public Garage(int kapacitet)
    {
        if (kapacitet < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(kapacitet), "Kapaciteten måste vara minst 1.");
        }
        this.kapacitet = kapacitet;
    }

    public bool Parkera(Fordon nyttFordon, out string meddelande)
    {
        if (fordon.Count >= kapacitet)
        {
            meddelande = "Kunde inte parkera: garaget är fullt.";
            return false;
        }
        if (HittaIndex(nyttFordon.Registreringsnummer) != -1)
        {
            meddelande = $"Kunde inte parkera: det finns redan ett fordon med registreringsnummer {nyttFordon.Registreringsnummer}.";
            return false;
        }

        fordon.Add(nyttFordon);
        meddelande = $"{nyttFordon.GetType().Name} {nyttFordon.Registreringsnummer} är nu parkerad. Lediga platser: {Kapacitet - AntalFordon}.";
        return true;
    }

    public bool TaBort(string registreringsnummer, out string meddelande)
    {
        int index = HittaIndex(registreringsnummer);
        if (index == -1)
        {
            meddelande = $"Kunde inte ta ut fordonet: hittar inget fordon med registreringsnummer {registreringsnummer.Trim().ToUpper()}.";
            return false;
        }

        string borttagetRegnr = fordon[index].Registreringsnummer;
        fordon.RemoveAt(index);

        meddelande = $"Fordonet {borttagetRegnr} är utkört ur garaget.";
        return true;
    }

    public Fordon? Hitta(string registreringsnummer)
    {
        int index = HittaIndex(registreringsnummer);
        if (index == -1)
        {
            return null;
        }
        return fordon[index];
    }

    public List<Fordon> HamtaAlla()
    {
        return new List<Fordon>(fordon);
    }

    public int AntalAvTyp(string typnamn)
    {
        int antal = 0;
        foreach (Fordon f in fordon)
        {
            if (f.GetType().Name.Equals(typnamn, StringComparison.OrdinalIgnoreCase))
            {
                antal++;
            }
        }
        return antal;
    }

    public List<Fordon> Sok(string? farg, int? antalHjul, string? typnamn)
    {
        List<Fordon> resultat = new List<Fordon>();

        foreach (Fordon f in fordon)
        {
            bool fargOk = farg == null || f.Farg.Equals(farg, StringComparison.OrdinalIgnoreCase);
            bool hjulOk = antalHjul == null || f.AntalHjul == antalHjul;
            bool typOk = typnamn == null || f.GetType().Name.Equals(typnamn, StringComparison.OrdinalIgnoreCase);

            if (fargOk && hjulOk && typOk)
            {
                resultat.Add(f);
            }
        }

        return resultat;
    }

    private int HittaIndex(string registreringsnummer)
    {
        string sokt = registreringsnummer.Trim().ToUpper();
        for (int i = 0; i < fordon.Count; i++)
        {
            if (fordon[i].Registreringsnummer == sokt)
            {
                return i;
            }
        }
        return -1;
    }
}