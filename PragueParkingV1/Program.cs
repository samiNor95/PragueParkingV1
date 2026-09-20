using System.Runtime.CompilerServices;

string[] parkingGarage = new string[100];
while (true)
{
    Console.WriteLine("\n== PRAGUE PARKING ==");
    Console.WriteLine("1. Parkera fordon");
    Console.WriteLine("2. Flytta forden");
    Console.WriteLine("3. Hämta ut fordon");
    Console.WriteLine("4. sök fordon");
    Console.WriteLine("5. Visa parkeringen");
    Console.WriteLine("6. Avsluta");
    Console.WriteLine(" Välj ett alternativ:");
    string val = Console.ReadLine()!;
    if (val == "1")
    {
        Console.Write("Ange fordonstyp (CAR/MC): ");
        string fordonstyp = Console.ReadLine()!.ToUpper();

        if (fordonstyp != "CAR" && fordonstyp != "MC")
        {
            Console.WriteLine("Fordonstypen måste vara CAR eller MC.");
            continue;
        }

        Console.Write("Ange registreringsnummer: ");
        string registreringsnummer = Console.ReadLine()!.ToUpper();

        if (string.IsNullOrWhiteSpace(registreringsnummer) ||
            registreringsnummer.Length > 10 ||
            registreringsnummer.Contains(" "))
        {
            Console.WriteLine(
                "Registreringsnumret måste innehålla 1-10 tecken utan mellanslag."
            );

            continue;
        }
        bool finnsRedan = false;

        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (string.IsNullOrEmpty(parkingGarage[i]))
            {
                continue;
            }

            string[] fordonPåPlatsen = parkingGarage[i].Split('|');

            foreach (string parkeratFordon in fordonPåPlatsen)
            {
                if (parkeratFordon.EndsWith("#" + registreringsnummer))
                {
                    finnsRedan = true;
                    break;
                }
            }

            if (finnsRedan)
            {
                break;
            }
        }

        if (finnsRedan)
        {
            Console.WriteLine(
                "Ett fordon med detta registreringsnummer finns redan."
            );

            continue;
        }
        string fordon = fordonstyp + "#" + registreringsnummer;

             bool parkerad = false;

        if (fordonstyp == "MC")
        {
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                if (!string.IsNullOrEmpty(parkingGarage[i]) &&
                    parkingGarage[i].StartsWith("MC#") &&
                    !parkingGarage[i].Contains("|"))
                {
                    parkingGarage[i] = parkingGarage[i] + "|" + fordon;

                    Console.WriteLine(
                        $"Motorcykeln ska parkeras på plats {i + 1} tillsammans med en annan MC."
                    );

                    parkerad = true;
                    break;
                }
            }
        }

        if (!parkerad)
        {
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                if (string.IsNullOrEmpty(parkingGarage[i]))
                {
                    parkingGarage[i] = fordon;

                    Console.WriteLine(
                        $"Fordonet ska parkeras på plats {i + 1}."
                    );

                    parkerad = true;
                    break;
                }
            }
        }

        if (!parkerad)
        {
            Console.WriteLine("Parkeringen är full.");
        }
    }

    else if (val == "2")
    {
        Console.Write("Ange registreringsnummer: ");
        string registreringsnummer = Console.ReadLine()!.ToUpper();

        Console.Write("Ange den nya platsen (1-100): ");
        int nyPlats = int.Parse(Console.ReadLine()!);

        if (nyPlats < 1 || nyPlats > 100)
        {
            Console.WriteLine("Platsnumret måste vara mellan 1 och 100.");
            continue;
        }

        int gammaltIndex = -1;
        string fordonSomFlyttas = "";

        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (string.IsNullOrEmpty(parkingGarage[i]))
            {
                continue;
            }

            string[] fordonPåPlatsen = parkingGarage[i].Split('|');

            foreach (string fordon in fordonPåPlatsen)
            {
                if (fordon.EndsWith("#" + registreringsnummer))
                {
                    gammaltIndex = i;
                    fordonSomFlyttas = fordon;
                    break;
                }
            }

            if (gammaltIndex != -1)
            {
                break;
            }
        }

        int nyttIndex = nyPlats - 1;

        if (gammaltIndex == -1)
        {
            Console.WriteLine("Fordonet hittades inte.");
        }
        else if (gammaltIndex == nyttIndex)
        {
            Console.WriteLine("Fordonet står redan på den platsen.");
        }
        else
        {
            bool kanFlyttas = false;

            if (string.IsNullOrEmpty(parkingGarage[nyttIndex]))
            {
                parkingGarage[nyttIndex] = fordonSomFlyttas;
                kanFlyttas = true;
            }
            else if (fordonSomFlyttas.StartsWith("MC#") &&
                     parkingGarage[nyttIndex].StartsWith("MC#") &&
                     !parkingGarage[nyttIndex].Contains("|"))
            {
                parkingGarage[nyttIndex] =
                    parkingGarage[nyttIndex] + "|" + fordonSomFlyttas;

                kanFlyttas = true;
            }
            else
            {
                Console.WriteLine("Den nya platsen är upptagen.");
            }

            if (kanFlyttas)
            {
                string[] gamlaFordon =
                    parkingGarage[gammaltIndex].Split('|');

                if (gamlaFordon.Length == 1)
                {
                    parkingGarage[gammaltIndex] = "";
                }
                else if (gamlaFordon[0] == fordonSomFlyttas)
                {
                    parkingGarage[gammaltIndex] = gamlaFordon[1];
                }
                else
                {
                    parkingGarage[gammaltIndex] = gamlaFordon[0];
                }

                Console.WriteLine(
                    $"Fordonet flyttades till plats {nyPlats}."
                );
            }
        }
    }
    else if (val == "3")
    {
        Console.Write("Ange registreringsnummer: ");
        string registreringsnummer = Console.ReadLine()!.ToUpper();

        bool hittad = false;

        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (string.IsNullOrEmpty(parkingGarage[i]))
            {
                continue;
            }

            string[] fordonPåPlatsen = parkingGarage[i].Split('|');

            for (int j = 0; j < fordonPåPlatsen.Length; j++)
            {
                if (fordonPåPlatsen[j].EndsWith("#" + registreringsnummer))
                {
                    Console.WriteLine(
                        $"Fordonet {fordonPåPlatsen[j]} hämtas från plats {i + 1}."
                    );

                    if (fordonPåPlatsen.Length == 1)
                    {
                        parkingGarage[i] = "";
                    }
                    else
                    {
                        if (j == 0)
                        {
                            parkingGarage[i] = fordonPåPlatsen[1];
                        }
                        else
                        {
                            parkingGarage[i] = fordonPåPlatsen[0];
                        }
                    }

                    hittad = true;
                    break;
                }
            }

            if (hittad)
            {
                break;
            }
        }

        if (!hittad)
        {
            Console.WriteLine("Fordonet hittades inte.");
        }
    }
    else if (val == "4")
    {
        Console.Write("Ange registreringsnummer: ");
        string registreringsnummer = Console.ReadLine()!.ToUpper();

        bool hittad = false;

        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (string.IsNullOrEmpty(parkingGarage[i]))
            {
                continue;
            }

            string[] fordonPåPlatsen = parkingGarage[i].Split('|');

            foreach (string fordon in fordonPåPlatsen)
            {
                if (fordon.EndsWith("#" + registreringsnummer))
                {
                    Console.WriteLine(
                        $"Fordonet {fordon} finns på plats {i + 1}."
                    );

                    hittad = true;
                    break;
                }
            }

            if (hittad)
            {
                break;
            }
        }

        if (!hittad)
        {
            Console.WriteLine("Fordonet hittades inte.");
        }
    }
    else if (val == "5")
    {
        Console.WriteLine("\n=== PARKERINGSPLATSER ===");

        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (string.IsNullOrEmpty(parkingGarage[i]))
            {
                Console.WriteLine($"Plats {i + 1}: Ledig");
            }
            else
            {
                Console.WriteLine($"Plats {i + 1}: {parkingGarage[i]}");
            }
        }
    }
    else if (val == "6")
    {
        Console.WriteLine("Progtammet avslutas.");
        break;
    }
    else
    {
        Console.WriteLine("Felaktigt val.");
    }
}
