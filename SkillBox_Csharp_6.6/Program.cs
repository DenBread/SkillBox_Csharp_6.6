using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1 - Вывести данные на экран");
        Console.WriteLine("2 - Добавить новую запись в файл");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                DisplayEmployees();
                break;
            case "2":
                AddEmployee();
                break;
            default:
                Console.WriteLine("Некорректный выбор.");
                break;
        }
    }

    static void DisplayEmployees()
    {
        string filePath = "Employees.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не существует.");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length == 0)
        {
            Console.WriteLine("Справочник пуст.");
            return;
        }

        foreach (string line in lines)
        {
            string[] data = line.Split('#');
            Console.WriteLine("ID: " + data[0]);
            Console.WriteLine("Дата и время добавления записи: " + data[1]);
            Console.WriteLine("Ф. И. О.: " + data[2]);
            Console.WriteLine("Возраст: " + data[3]);
            Console.WriteLine("Рост: " + data[4]);
            Console.WriteLine("Дата рождения: " + data[5]);
            Console.WriteLine("Место рождения: " + data[6]);
            Console.WriteLine();
        }
    }

    static void AddEmployee()
    {
        string filePath = "Employees.txt";

        Console.WriteLine("Введите данные нового сотрудника:");
        Console.Write("ID: ");
        string id = Console.ReadLine();

        Console.Write("Ф. И. О.: ");
        string fullName = Console.ReadLine();

        Console.Write("Возраст: ");
        string age = Console.ReadLine();

        Console.Write("Рост: ");
        string height = Console.ReadLine();

        Console.Write("Дата рождения: ");
        string dateOfBirth = Console.ReadLine();

        Console.Write("Место рождения: ");
        string placeOfBirth = Console.ReadLine();

        string timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        string newRecord = $"{id}#{timestamp}#{fullName}#{age}#{height}#{dateOfBirth}#{placeOfBirth}";

        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(newRecord);
        }

        Console.WriteLine("Новая запись добавлена в файл.");
    }
}
