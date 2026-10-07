using System;

class Program
{
    static string[] parkingGarage = new string[100];

    static void Main()
    {
        while (true)
        {
            DisplayMenu();
            string choice = GetMenuChoice();

            if (choice == "1")
            {
                ParkVehicle();
            }
            else if (choice == "2")
            {
                MoveVehicle();
            }
            else if (choice == "3")
            {
                RemoveVehicle();
            }
            else if (choice == "4")
            {
                SearchVehicle();
            }
            else if (choice == "5")
            {
                DisplayGarageContents();
            }
            else if (choice == "6")
            {
                Console.WriteLine("Programmet avslutas.");
                break;
            }
            else
            {
                Console.WriteLine("Felaktigt val.");
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("\n=== PRAGUE PARKING ===");
        Console.WriteLine("1. Parkera fordon");
        Console.WriteLine("2. Flytta fordon");
        Console.WriteLine("3. Hämta ut fordon");
        Console.WriteLine("4. Sök fordon");
        Console.WriteLine("5. Visa parkeringen");
        Console.WriteLine("6. Avsluta");
    }

    static string GetMenuChoice()
    {
        Console.Write("Välj ett alternativ: ");
        return Console.ReadLine()!;
    }

    static void ParkVehicle()
    {
        Console.Write("Ange fordonstyp (CAR/MC): ");
        string vehicleType = Console.ReadLine()!.ToUpper();

        if (vehicleType != "CAR" && vehicleType != "MC")
        {
            Console.WriteLine("Fordonstypen måste vara CAR eller MC.");
            return;
        }

        Console.Write("Ange registreringsnummer: ");
        string regNumber = Console.ReadLine()!.ToUpper();

        if (!IsValidRegistrationNumber(regNumber))
        {
            Console.WriteLine("Registreringsnumret måste innehålla 1-10 tecken utan mellanslag.");
            return;
        }

        if (FindVehicle(regNumber, out int existingSlot))
        {
            Console.WriteLine($"Fordonet finns redan på plats {existingSlot + 1}.");
            return;
        }

        int freeSlot = SearchFreeSpot(vehicleType);

        if (freeSlot == -1)
        {
            Console.WriteLine("Parkeringen är full.");
            return;
        }

        string vehicle = vehicleType + "#" + regNumber;
        bool sharedSpot = !string.IsNullOrEmpty(parkingGarage[freeSlot]);

        InsertVehicleIntoGarage(vehicle, freeSlot);

        if (sharedSpot)
        {
            Console.WriteLine($"Motorcykeln parkerades på plats {freeSlot + 1} tillsammans med en annan MC.");
        }
        else
        {
            Console.WriteLine($"Fordonet parkerades på plats {freeSlot + 1}.");
        }
    }

    static bool IsValidRegistrationNumber(string regNumber)
    {
        return !string.IsNullOrWhiteSpace(regNumber) &&
               regNumber.Length <= 10 &&
               !regNumber.Contains(" ");
    }

    static void InsertVehicleIntoGarage(string vehicle, int slotNumber)
    {
        if (string.IsNullOrEmpty(parkingGarage[slotNumber]))
        {
            parkingGarage[slotNumber] = vehicle;
        }
        else
        {
            parkingGarage[slotNumber] += "|" + vehicle;
        }
    }

    static int SearchFreeSpot(string vehicleType)
    {
        if (vehicleType == "MC")
        {
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                if (!string.IsNullOrEmpty(parkingGarage[i]) &&
                    parkingGarage[i].StartsWith("MC#") &&
                    !parkingGarage[i].Contains("|"))
                {
                    return i;
                }
            }
        }

        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (string.IsNullOrEmpty(parkingGarage[i]))
            {
                return i;
            }
        }

        return -1;
    }

    static void SearchVehicle()
    {
        Console.Write("Ange registreringsnummer: ");
        string regNumber = Console.ReadLine()!.ToUpper();

        if (FindVehicle(regNumber, out int slotNumber))
        {
            Console.WriteLine($"Fordonet finns på plats {slotNumber + 1}.");
        }
        else
        {
            Console.WriteLine("Fordonet hittades inte.");
        }
    }

    static bool FindVehicle(string regNumber, out int slotNumber)
    {
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (string.IsNullOrEmpty(parkingGarage[i]))
            {
                continue;
            }

            string[] vehicles = parkingGarage[i].Split('|');

            foreach (string vehicle in vehicles)
            {
                if (vehicle.EndsWith("#" + regNumber))
                {
                    slotNumber = i;
                    return true;
                }
            }
        }

        slotNumber = -1;
        return false;
    }

    static void RemoveVehicle()
    {
        Console.Write("Ange registreringsnummer: ");
        string regNumber = Console.ReadLine()!.ToUpper();

        if (!FindVehicle(regNumber, out int slotNumber))
        {
            Console.WriteLine("Fordonet hittades inte.");
            return;
        }

        string vehicle = GetVehicleFromSlot(regNumber, slotNumber);
        RemoveVehicleFromGarage(regNumber, slotNumber);

        Console.WriteLine($"Fordonet {vehicle} hämtades från plats {slotNumber + 1}.");
    }

    static void RemoveVehicleFromGarage(string regNumber, int slotNumber)
    {
        string[] vehicles = parkingGarage[slotNumber].Split('|');

        if (vehicles.Length == 1)
        {
            parkingGarage[slotNumber] = "";
        }
        else if (vehicles[0].EndsWith("#" + regNumber))
        {
            parkingGarage[slotNumber] = vehicles[1];
        }
        else
        {
            parkingGarage[slotNumber] = vehicles[0];
        }
    }

    static string GetVehicleFromSlot(string regNumber, int slotNumber)
    {
        string[] vehicles = parkingGarage[slotNumber].Split('|');

        foreach (string vehicle in vehicles)
        {
            if (vehicle.EndsWith("#" + regNumber))
            {
                return vehicle;
            }
        }

        return "";
    }

    static void MoveVehicle()
    {
        Console.Write("Ange registreringsnummer: ");
        string regNumber = Console.ReadLine()!.ToUpper();

        if (!FindVehicle(regNumber, out int oldSlot))
        {
            Console.WriteLine("Fordonet hittades inte.");
            return;
        }

        Console.Write("Ange den nya platsen (1-100): ");

        if (!int.TryParse(Console.ReadLine(), out int newPlace) ||
            newPlace < 1 || newPlace > parkingGarage.Length)
        {
            Console.WriteLine("Platsnumret måste vara mellan 1 och 100.");
            return;
        }

        int newSlot = newPlace - 1;

        if (oldSlot == newSlot)
        {
            Console.WriteLine("Fordonet står redan på den platsen.");
            return;
        }

        string vehicle = GetVehicleFromSlot(regNumber, oldSlot);

        if (!CanParkAtSlot(vehicle, newSlot))
        {
            Console.WriteLine("Den nya platsen är upptagen.");
            return;
        }

        MoveVehicleInGarage(regNumber, vehicle, oldSlot, newSlot);
        Console.WriteLine($"Fordonet flyttades till plats {newPlace}.");
    }

    static bool CanParkAtSlot(string vehicle, int slotNumber)
    {
        if (string.IsNullOrEmpty(parkingGarage[slotNumber]))
        {
            return true;
        }

        return vehicle.StartsWith("MC#") &&
               parkingGarage[slotNumber].StartsWith("MC#") &&
               !parkingGarage[slotNumber].Contains("|");
    }

    static void MoveVehicleInGarage(
        string regNumber,
        string vehicle,
        int oldSlot,
        int newSlot)
    {
        InsertVehicleIntoGarage(vehicle, newSlot);
        RemoveVehicleFromGarage(regNumber, oldSlot);
    }

    static void DisplayGarageContents()
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
}
