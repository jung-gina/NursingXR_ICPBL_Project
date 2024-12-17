using System;

namespace MedicalSimulation
{
    // Represents a medical instrument known as a stethoscope
    public class Stethoscope
    {
        // Property to store the brand of the stethoscope
        public string Brand { get; set; }
        
        // Constructor to initialize the stethoscope with a specific brand
        public Stethoscope(string brand)
        {
            Brand = brand;
        }

        // Method to simulate using the stethoscope
        public void Use()
        {
            Console.WriteLine("The stethoscope is being used.");
        }
    }

    // Represents a nurse in the medical simulation
    public class Nurse
    {
        // Property to store the name of the nurse
        public string Name { get; set; }
        
        // Constructor to initialize the nurse with a name
        public Nurse(string name)
        {
            Name = name;
        }

        // Method for the nurse to pick up a stethoscope and use it
        public void PickUpStethoscope(Stethoscope stethoscope)
        {
            Console.WriteLine($"{Name} has picked up the {stethoscope.Brand} stethoscope.");
            stethoscope.Use();  // Calling the Use method of the stethoscope
        }
    }

    // The entry point of the program
    class Program
    {
        // Main method where the execution starts
        static void Main(string[] args)
        {
            // Creating a new stethoscope instance with the brand "Littmann"
            Stethoscope stethoscope = new Stethoscope("Littmann");

            // Creating a new nurse instance with the name "Emily"
            Nurse nurse = new Nurse("Emily");

            // Simulating the nurse picking up and using the stethoscope
            nurse.PickUpStethoscope(stethoscope);
        }
    }
}