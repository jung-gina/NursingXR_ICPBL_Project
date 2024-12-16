using System;

public class Stethoscope
{
    public string Brand { get; set; }
    
    public Stethoscope(string brand)
    {
        Brand = brand;
    }
}

public class Nurse
{
    public string Name { get; set; }

    public Nurse(string name)
    {
        Name = name;
    }

    public void PickUpStethoscope(Stethoscope stethoscope)
    {
        Console.WriteLine($"{Name}님이 {stethoscope.Brand} 청진기를 들었습니다.");
    }
}

class Program
{
    static void Main()
    {
        Stethoscope stethoscope = new Stethoscope("리트만");
        Nurse nurse = new Nurse("지민");

        nurse.PickUpStethoscope(stethoscope);
    }
}
