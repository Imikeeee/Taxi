using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FuvarApp
{
    public class Fuvar
    {
        public int TaxiId { get; set; }
        public DateTime Indulas { get; set; }
        public int Idotartam { get; set; }
        public double Tavolsag { get; set; }
        public double Viteldij { get; set; }
        public double Borravalo { get; set; }
        public string FizetesModja { get; set; }

        public Fuvar(string line)
        {
            string[] parts = line.Split(';');
            if (parts.Length >= 7)
            {
                if (int.TryParse(parts[0], out int id))
                    TaxiId = id;

                if (DateTime.TryParse(parts[1], out DateTime startTime))
                    Indulas = startTime;

                if (int.TryParse(parts[2], out int duration))
                    Idotartam = duration;

                if (double.TryParse(parts[3].Replace(',', '.'), out double distance))
                    Tavolsag = distance;

                if (double.TryParse(parts[4].Replace(',', '.'), out double fare))
                    Viteldij = fare;

                if (double.TryParse(parts[5].Replace(',', '.'), out double tip))
                    Borravalo = tip;

                FizetesModja = parts[6];
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Fuvar> fuvarList = ReadFuvarData("fuvar.csv");

            Console.WriteLine("3. feladat");
            Console.WriteLine("fuvarok: " + fuvarList.Count);

            Console.WriteLine("4. feladat");
            IncomeOfGivenEntrepreneurAndNumberOfJourneys(fuvarList);

            Console.WriteLine("5. feladat");
            PaymentMethodStatistics(fuvarList);

            Console.WriteLine("6. feladat");
            TotalKm(fuvarList);

            Console.WriteLine("7. feladat");
            LongestJourney(fuvarList);

            Console.WriteLine("8. feladat");
            ErrataGenerator(fuvarList);

            Console.ReadKey();
        }

        private static List<Fuvar> ReadFuvarData(string filePath)
        {
            var fuvarList = new List<Fuvar>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 1; i < lines.Length; i++)
                {
                    Fuvar fuvarInstance = new Fuvar(lines[i]);
                    fuvarList.Add(fuvarInstance);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading data: " + ex.Message);
            }

            return fuvarList;
        }

        private static void ErrataGenerator(List<Fuvar> fuvarList)
        {
            var errata = fuvarList
                .OrderBy(f => f.Indulas)
                .Where(f => f.Viteldij > 0 && f.Idotartam > 0 && f.Tavolsag == 0)
                .Select(f => $"{f.TaxiId};{f.Indulas};{f.Idotartam};{f.Tavolsag};{f.Viteldij};{f.Borravalo};{f.FizetesModja}");

            File.WriteAllLines("hibak.txt", new[] { "taxi_id;indulas;idotartam;tavolsag;viteldij;borravalo;fizetes_modja" }.Concat(errata));
        }

        private static void LongestJourney(List<Fuvar> fuvarList)
        {
            var longestJourney = fuvarList
                .OrderByDescending(f => f.Idotartam)
                .FirstOrDefault();

            if (longestJourney != null)
            {
                double distanceInKm = longestJourney.Tavolsag * 1.6;
                Console.WriteLine("A leghosszabb fuvar:");
                Console.WriteLine("Fuvar hossza: " + longestJourney.Idotartam);
                Console.WriteLine("Taxi azonosító: " + longestJourney.TaxiId);
                Console.WriteLine("Megtett távolság: " + longestJourney.Tavolsag + " mérföld, " + distanceInKm.ToString("0.00") + " km");
                Console.WriteLine("Viteldíj: " + longestJourney.Viteldij);
            }
            else
            {
                Console.WriteLine("No journeys found.");
            }
        }

        private static void TotalKm(List<Fuvar> fuvarList)
        {
            double totalKm = fuvarList.Sum(f => f.Tavolsag * 1.6);
            Console.WriteLine(totalKm.ToString("0.00"));
        }

        private static void PaymentMethodStatistics(List<Fuvar> fuvarList)
        {
            var paymentMethodCounts = fuvarList
                .GroupBy(f => f.FizetesModja)
                .Select(group => new { PaymentMethod = group.Key, Count = group.Count() });

            foreach (var paymentMethod in paymentMethodCounts)
            {
                Console.WriteLine($"{paymentMethod.PaymentMethod}: {paymentMethod.Count} fuvar");
            }
        }

        private static void IncomeOfGivenEntrepreneurAndNumberOfJourneys(List<Fuvar> fuvarList)
        {
            var entrepreneurJourneys = fuvarList.Where(f => f.TaxiId == 6185).ToList();

            int journeyCounter = entrepreneurJourneys.Count;
            double bevetel = entrepreneurJourneys.Sum(f => f.Viteldij);

            Console.WriteLine(journeyCounter + " fuvar alatt: " + bevetel + "$");
        }
    }
}
