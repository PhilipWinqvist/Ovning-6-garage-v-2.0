using System.Globalization;

namespace Garage1;

public class Program
{

    private static readonly string[] Typnamn = { "Motorcykel", "Flygplan", "Bil", "Buss", "Bat" };

    private static readonly string[] Testfarger = { "Svart", "Röd", "Blå", "Vit", "Rosa", "Grön" };

    private static readonly Random slump = new Random();

    private static int testRaknare = 100;

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Garage? garage = null;   
        bool kor = true;

        while (kor)
        {
            VisaMeny(garage);
            int val = LasHeltal("Ditt val", 0, 8);
            Console.WriteLine();


            if (val >= 2 && garage == null)
            {
                Console.WriteLine("Du måste skapa ett garage först.");
                continue;
            }

            switch (val)
            {
                case 1:
                    garage = SkapaGarage(garage);
                    break;
                case 2:
                    SkrivUtFordon(garage!.HamtaAlla());
                    break;
                case 3:
                    ListaTyper(garage!);
                    break;
                case 4:
                    ParkeraFordon(garage!);
                    break;
                case 5:
                    TaUtFordon(garage!);
                    break;
                case 6:
                    HittaFordon(garage!);
                    break;
                case 7:
                    SokFordon(garage!);
                    break;
                case 8:
                    FyllMedTestfordon(garage!);
                    break;
                case 0:
                    Console.WriteLine("Programmet avslutas. Hej då!");
                    kor = false;
                    break;
            }
        }
    }


    private static void VisaMeny(Garage? garage)
    {
        Console.WriteLine();
        Console.WriteLine("=== GARAGE 1.0 ===");
        if (garage != null)
        {
            Console.WriteLine($"Garage: {garage.AntalFordon} av {garage.Kapacitet} platser används");
        }
        else
        {
            Console.WriteLine("Inget garage skapat ännu");
        }
        Console.WriteLine("1. Skapa nytt garage");
        Console.WriteLine("2. Lista alla parkerade fordon");
        Console.WriteLine("3. Lista fordonstyper och antal");
        Console.WriteLine("4. Parkera ett fordon");
        Console.WriteLine("5. Checka ut ett fordon");
        Console.WriteLine("6. Sök via registreringsnummer");
        Console.WriteLine("7. Sök fordon (färg / antal hjul / typ)");
        Console.WriteLine("8. Fyll garaget med testfordon");
        Console.WriteLine("0. Avsluta");
    }

    private static Garage? SkapaGarage(Garage? befintligt)
    {
        if (befintligt != null)
        {
            Console.WriteLine("Det finns redan ett garage. Om du skapar ett nytt försvinner alla fordon i det gamla.");
            if (!LasJaNej("Vill du fortsätta?"))
            {
                Console.WriteLine("Avbrutet. Du behåller det gamla garaget.");
                return befintligt;
            }
        }

        int kapacitet = LasHeltal("Hur många parkeringsplatser ska garaget ha", 1, 1000);
        Garage nyttGarage = new Garage(kapacitet);
        Console.WriteLine($"Ett garage med {kapacitet} platser är skapat.");

        if (LasJaNej("Vill du fylla det med några testfordon direkt?"))
        {
            FyllMedTestfordon(nyttGarage);
        }
        return nyttGarage;
    }

    private static void ListaTyper(Garage garage)
    {
        Console.WriteLine("Fordonstyper i garaget:");
        foreach (string typ in Typnamn)
        {
            Console.WriteLine($"  {typ,-11} {garage.AntalAvTyp(typ)} st");
        }
    }

    private static void ParkeraFordon(Garage garage)
    {
        if (garage.AntalFordon >= garage.Kapacitet)
        {
            Console.WriteLine("Garaget är fullt – du kan inte parkera fler fordon.");
            return;
        }

        Console.WriteLine("Vilken typ av fordon vill du parkera?");
        for (int i = 0; i < Typnamn.Length; i++)
        {
            Console.WriteLine($"  {i + 1}. {Typnamn[i]}");
        }
        int typ = LasHeltal("Typ", 1, Typnamn.Length);

        string regnr = LasRegistreringsnummer();


        if (garage.Hitta(regnr) != null)
        {
            Console.WriteLine($"Kunde inte parkera: {regnr.ToUpper()} finns redan i garaget.");
            return;
        }

        string farg = LasText("Färg");
        int antalHjul = LasHeltal("Antal hjul", 0, 20);

        Fordon fordon;
        switch (typ)
        {
            case 1:
                int cc = LasHeltal("Cylindervolym i cc", 1, 5000);
                fordon = new Motorcykel(regnr, farg, antalHjul, cc);
                break;
            case 2:
                int motorer = LasHeltal("Antal motorer", 1, 10);
                fordon = new Flygplan(regnr, farg, antalHjul, motorer);
                break;
            case 3:
                Console.WriteLine("Bränsle: 1. Bensin  2. Diesel");
                int bransleVal = LasHeltal("Bränsle", 1, 2);
                Bransletyp bransle = bransleVal == 1 ? Bransletyp.Bensin : Bransletyp.Diesel;
                fordon = new Bil(regnr, farg, antalHjul, bransle);
                break;
            case 4:
                int saten = LasHeltal("Antal säten", 1, 200);
                fordon = new Buss(regnr, farg, antalHjul, saten);
                break;
            default:
                double langd = LasDecimaltal("Längd i meter", 0.5, 400);
                fordon = new Bat(regnr, farg, antalHjul, langd);
                break;
        }

        garage.Parkera(fordon, out string meddelande);
        Console.WriteLine(meddelande);
    }

    private static void TaUtFordon(Garage garage)
    {
        if (garage.AntalFordon == 0)
        {
            Console.WriteLine("Garaget är tomt – det finns inget att ta ut.");
            return;
        }
        string regnr = LasText("Registreringsnummer på fordonet som ska tas ut");
        garage.TaBort(regnr, out string meddelande);
        Console.WriteLine(meddelande);
    }

    private static void HittaFordon(Garage garage)
    {
        string regnr = LasText("Registreringsnummer");
        Fordon? fordon = garage.Hitta(regnr);
        if (fordon == null)
        {
            Console.WriteLine($"Hittade inget fordon med registreringsnummer {regnr.Trim().ToUpper()}.");
        }
        else
        {
            Console.WriteLine("Hittade:");
            Console.WriteLine("  " + fordon);
        }
    }

    private static void SokFordon(Garage garage)
    {
        Console.WriteLine("Sök fordon. Tryck bara Enter för de kriterier du vill hoppa över.");

        Console.Write("Färg: ");
        string fargInmatning = (Console.ReadLine() ?? "").Trim();
        string? farg = fargInmatning == "" ? null : fargInmatning;

        int? antalHjul = LasValfrittHeltal("Antal hjul", 0, 20);

        Console.WriteLine("Typ: 0. Alla typer");
        for (int i = 0; i < Typnamn.Length; i++)
        {
            Console.WriteLine($"     {i + 1}. {Typnamn[i]}");
        }
        int typVal = LasHeltal("Typ", 0, Typnamn.Length);
        string? typnamn = typVal == 0 ? null : Typnamn[typVal - 1];

        if (farg == null && antalHjul == null && typnamn == null)
        {
            Console.WriteLine("Du angav inga kriterier, så alla fordon visas:");
        }

        SkrivUtFordon(garage.Sok(farg, antalHjul, typnamn));
    }

    private static void FyllMedTestfordon(Garage garage)
    {
        int lediga = garage.Kapacitet - garage.AntalFordon;
        if (lediga == 0)
        {
            Console.WriteLine("Garaget är fullt – inga testfordon kan läggas till.");
            return;
        }

        int antal = LasHeltal("Hur många testfordon vill du lägga till", 1, lediga);
        int tillagda = 0;

        while (tillagda < antal && testRaknare <= 999)
        {
            string regnr = "TST" + testRaknare;
            testRaknare++;

            Fordon fordon = SkapaSlumpatFordon(regnr);
            if (garage.Parkera(fordon, out _))
            {
                tillagda++;
            }
        }
        Console.WriteLine($"{tillagda} testfordon lades till i garaget.");
    }

    private static Fordon SkapaSlumpatFordon(string regnr)
    {
        string farg = Testfarger[slump.Next(Testfarger.Length)];

        switch (slump.Next(5))
        {
            case 0:
                return new Motorcykel(regnr, farg, slump.Next(2, 4), slump.Next(125, 1201));
            case 1:
                return new Flygplan(regnr, farg, 3, slump.Next(1, 5));
            case 2:
                Bransletyp bransle = slump.Next(2) == 0 ? Bransletyp.Bensin : Bransletyp.Diesel;
                return new Bil(regnr, farg, 4, bransle);
            case 3:
                return new Buss(regnr, farg, slump.Next(4, 7), slump.Next(20, 61));
            default:
                return new Bat(regnr, farg, 0, Math.Round(slump.NextDouble() * 15 + 3, 1));
        }
    }

    private static void SkrivUtFordon(List<Fordon> fordonLista)
    {
        if (fordonLista.Count == 0)
        {
            Console.WriteLine("Inga fordon hittades.");
            return;
        }

        Console.WriteLine($"{fordonLista.Count} fordon:");
        foreach (Fordon f in fordonLista)
        {
            Console.WriteLine("  " + f);
        }
    }

    private static int LasHeltal(string fraga, int min, int max)
    {
        while (true)
        {
            Console.Write($"{fraga} ({min}-{max}): ");
            string inmatning = Console.ReadLine() ?? "";

            if (int.TryParse(inmatning, out int varde) && varde >= min && varde <= max)
            {
                return varde;
            }
            Console.WriteLine($"Ogiltig inmatning. Skriv ett heltal mellan {min} och {max}.");
        }
    }

    private static int? LasValfrittHeltal(string fraga, int min, int max)
    {
        while (true)
        {
            Console.Write($"{fraga} ({min}-{max}, Enter = hoppa över): ");
            string inmatning = (Console.ReadLine() ?? "").Trim();

            if (inmatning == "")
            {
                return null;
            }
            if (int.TryParse(inmatning, out int varde) && varde >= min && varde <= max)
            {
                return varde;
            }
            Console.WriteLine($"Ogiltig inmatning. Skriv ett heltal mellan {min} och {max}, eller tryck Enter.");
        }
    }

    private static double LasDecimaltal(string fraga, double min, double max)
    {
        while (true)
        {
            Console.Write($"{fraga} ({min}-{max}): ");
            string inmatning = (Console.ReadLine() ?? "").Trim().Replace(',', '.');

            if (double.TryParse(inmatning, NumberStyles.Float, CultureInfo.InvariantCulture, out double varde)
                && varde >= min && varde <= max)
            {
                return varde;
            }
            Console.WriteLine($"Ogiltig inmatning. Skriv ett tal mellan {min} och {max}.");
        }
    }

    private static string LasText(string fraga)
    {
        while (true)
        {
            Console.Write(fraga + ": ");
            string inmatning = (Console.ReadLine() ?? "").Trim();
            if (inmatning != "")
            {
                return inmatning;
            }
            Console.WriteLine("Du måste skriva något.");
        }
    }

    private static string LasRegistreringsnummer()
    {
        while (true)
        {
            string inmatning = LasText("Registreringsnummer (3 bokstäver + 3 siffror, t.ex. ABC123)");
            if (ArGiltigtRegnr(inmatning))
            {
                return inmatning;
            }
            Console.WriteLine("Ogiltigt registreringsnummer. Det ska vara 3 bokstäver följt av 3 siffror.");
        }
    }

    private static bool ArGiltigtRegnr(string regnr)
    {
        if (regnr.Length != 6)
        {
            return false;
        }
        for (int i = 0; i < 3; i++)
        {
            if (!char.IsLetter(regnr[i]))
            {
                return false;
            }
        }
        for (int i = 3; i < 6; i++)
        {
            if (!char.IsDigit(regnr[i]))
            {
                return false;
            }
        }
        return true;
    }

    private static bool LasJaNej(string fraga)
    {
        while (true)
        {
            Console.Write(fraga + " (j/n): ");
            string inmatning = (Console.ReadLine() ?? "").Trim().ToLower();
            if (inmatning == "j" || inmatning == "ja")
            {
                return true;
            }
            if (inmatning == "n" || inmatning == "nej")
            {
                return false;
            }
            Console.WriteLine("Svara j eller n.");
        }
    }
}
