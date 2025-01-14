
using System;

namespace OOD_Assignment
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Grid dotGrid = new Grid(100000);
            CodeChecker checker = new CodeChecker(dotGrid);

            while (true) {

                Console.WriteLine("Select one of the following options");
                Console.WriteLine("g) Add 'Guard' obstacle");
                Console.WriteLine("f) Add 'Fence' obstacle");
                Console.WriteLine("s) Add 'Sensor' obstacle");
                Console.WriteLine("c) Add 'Camera' obstacle");
                Console.WriteLine("d) Show safe directions");
                Console.WriteLine("m) Display obstacle map");
                Console.WriteLine("p) Find safe path");
                Console.WriteLine("x) Exit");
                Console.WriteLine("Enter code:");

                string code = Console.ReadLine();
                checker.Check(code);
            }
        }
    }
}


