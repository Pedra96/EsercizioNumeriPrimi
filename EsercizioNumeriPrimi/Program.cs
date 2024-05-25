using Autofac;
using EsercizioNumeriPrimi;
using EsercizioNumeriPrimi.Models;
using LibreriaNumeriPrimi;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;

internal class Program
{
    private static void Main(string[] args)
    {
        /*
         Scrivere una applicazione console C# che maneggi i numeri primi.L'applicazione deve prevedere:
        -stampa a video di tutti i numeri primi verificati con il momento in cui ciascun numero primo è stato verificato V
        -stampa a video dell'n-esimo numero primo con il momento in cui il numero primo è stato verificato V
        -stampa a video dei numeri primi compresi in un intervallo limitato da due interi scelti(compresi) V
         */

        var builder = new ContainerBuilder();
        builder.RegisterType<DiscoveryPrime>().As<IPrimeNumber>();
        var container = builder.Build();
        IPrimeNumber primeNumber = container.Resolve<IPrimeNumber>();

        List<PrimeNumber> primeNumbers = new();
        using (var db = new NumberContext())
        {
            primeNumbers = db.PrimeNumbers.OrderBy(x => x.Number).ToList();
        }
        bool flag = false;
        do { 
        Console.WriteLine("Quale operazione vuoi eserguire?(inserire il numero corrispondente)");
        Console.WriteLine("1.Stampa tutti i numeri primi contenuti nel database\n2.Recupera uno specifico numero primo dal database" +
            "\n3.Controlla i numeri primi in un determinato range\n4.esplora l'infinito e oltre con il discovery\n" +
            "5.Inserire manualmente un numero primo all'interno del database\n" +
            "inserire un valore non valido o nullo per non fare nessuna operazione");
        dynamic input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    foreach (var Number in primeNumbers)
                    {
                        Console.WriteLine(Number.ToString());
                    }
                    break;

                case "2":
                    try
                    {
                        while (true)
                        {
                            Console.WriteLine("Inserire il numero da recuperare dal database");
                            int retrieveNumber = int.Parse(Console.ReadLine());
                            using (var db = new NumberContext())
                            {
                                var numberFromDb = db.PrimeNumbers.Where(x => x.Number == retrieveNumber).FirstOrDefault();
                                if (numberFromDb == null)
                                {
                                    Console.WriteLine("numero non trovato");
                                } else
                                {
                                    Console.WriteLine(numberFromDb.ToString());

                                }
                            }
                            Console.WriteLine("Vuoi recuperare un altro numero?");
                            if (Console.ReadLine().ToLower() == "si")
                            {

                            } else
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("il valore inserito non è valido");
                    }
                    break;

                case "3":

                    Console.WriteLine("Inserire il valore minimo da usare:");
                    int userMin = int.Parse(Console.ReadLine());
                    Console.WriteLine("Inserire il valore massimo da usare:");
                    int userMax = int.Parse(Console.ReadLine());
                    primeNumber.PrimeNumberInRange(userMin, userMax);
                    break;

                case "4":
                    Discovery(primeNumber);
                    break;
                case "5":
                    while (true)
                    {
                        Console.WriteLine("Quanti numeri primi vogliamo inserire?");
                        try
                        {
                            input = int.Parse(Console.ReadLine());
                            for (int i = 0; i < input; i++)
                            {
                                Console.WriteLine("inserire il numero primo da aggiungere");
                                int userNumber = int.Parse(Console.ReadLine());
                                if (AddPrimeNumber(userNumber, primeNumber))
                                {
                                    Console.WriteLine($"IL numero  {userNumber} è stato inserito correttamente");
                                }

                            }
                            Console.WriteLine("Vuoi inserire un altro numero?");
                            if (Console.ReadLine().ToLower() != "si")
                            {
                                break;
                            }

                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("il valore inserito non è valido");
                        }

                    }
                    break;
                default:
                    break;

            }
            Console.WriteLine("Vuoi eseguire un altra operazione?");
            if (Console.ReadLine().ToLower() == "si")
            {
                flag = true;
            } else
            {
                flag = false;
            }
        } while (flag);

    }
    public async static void Discovery(IPrimeNumber primeNumber)
    {
        int largest = 0;
        using (var db = new NumberContext())
        {
            largest = db.PrimeNumbers.Max(x => x.Number) + 1;
            Console.WriteLine("Discovery si limita a 100 numeri primi, vuoi eliminare il limite e andare verso l'infinito e oltre? \nSi/No");
            string input = Console.ReadLine();
            try
            {
                int i = 0;
                bool InfiniteLoop = false;
                if (input == "Si")
                {
                    InfiniteLoop = true;
                }

                while (i <= 100 || InfiniteLoop)
                {

                    if (primeNumber.VerifyPrimeNumber(largest))
                    {
                        PrimeNumber newPrimeNumber = new()
                        {
                            Number = largest,
                            VerificationTime = DateTime.Now
                        };
                        db.PrimeNumbers.Add(newPrimeNumber);
                        db.SaveChanges();
                        i++;
                    }
                    largest++;
                }
                Console.WriteLine("Il discovery ha finito di eseguirsi");

            }
            catch (SqlException e)
            {
                Console.WriteLine($"{largest} già presente passo al prossimo");
                largest++;

            }

        }

    }
    public static bool AddPrimeNumber(int input, IPrimeNumber primeNumber)
    {
        if (primeNumber.VerifyPrimeNumber(input))
        {
            PrimeNumber userNumber = new()
            {
                Number = input,
                VerificationTime = DateTime.Now
            };
            using (var db = new NumberContext())
            {
                try
                {
                    db.PrimeNumbers.Add(userNumber);
                    db.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    Console.WriteLine("Numero già presente");
                    return false;
                }
            }
        } else
        {
            Console.WriteLine("il numero inserito non è primo");
            return false;
        }
    }

}