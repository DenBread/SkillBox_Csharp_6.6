using System;
using System.IO;

// Определение структуры Worker
struct Worker
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string FIO { get; set; }
    public int Age { get; set; }
    public int Height { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PlaceOfBirth { get; set; }
}

// Класс, отвечающий за работу с экземплярами Worker
class Repository
{
    private string filePath = "Employees.txt"; // Имя файла

    // Получение всех записей
    public Worker[] GetAllWorkers()
    {
        if (!File.Exists(filePath))
        {
            // Если файл не существует, возвращаем пустой массив
            return new Worker[0];
        }

        string[] lines = File.ReadAllLines(filePath);

        Worker[] workers = new Worker[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] data = lines[i].Split('#');
            workers[i] = new Worker
            {
                Id = int.Parse(data[0]),
                Timestamp = DateTime.Parse(data[1]),
                FIO = data[2],
                Age = int.Parse(data[3]),
                Height = int.Parse(data[4]),
                DateOfBirth = DateTime.Parse(data[5]),
                PlaceOfBirth = data[6]
            };
        }

        return workers;
    }

    // Получение записи по ID
    public Worker GetWorkerById(int id)
    {
        Worker[] workers = GetAllWorkers();

        foreach (Worker worker in workers)
        {
            if (worker.Id == id)
            {
                return worker;
            }
        }

        return default(Worker); // Если запись не найдена, возвращаем значение по умолчанию
    }

    // Удаление записи по ID
    public void DeleteWorker(int id)
    {
        Worker[] workers = GetAllWorkers();

        // Фильтрация записей для удаления
        workers = workers.Where(worker => worker.Id != id).ToArray();

        // Перезапись файла с новыми данными
        File.WriteAllLines(filePath, workers.Select(worker => FormatWorkerData(worker)));
    }

    // Добавление новой записи
    public void AddWorker(Worker worker)
    {
        Worker[] workers = GetAllWorkers();

        int newId = 1;
        if (workers.Length > 0)
        {
            newId = workers.Max(w => w.Id) + 1; // Генерация нового уникального ID
        }

        worker.Id = newId;
        worker.Timestamp = DateTime.Now;

        string newRecord = FormatWorkerData(worker);

        // Добавление новой записи в конец файла
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(newRecord);
        }
    }

    // Загрузка записей в выбранном диапазоне дат
    public Worker[] GetWorkersBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
    {
        Worker[] workers = GetAllWorkers();

        Worker[] filteredWorkers = new Worker[0];
        foreach (Worker worker in workers)
        {
            if (worker.DateOfBirth >= dateFrom && worker.DateOfBirth <= dateTo)
            {
                Array.Resize(ref filteredWorkers, filteredWorkers.Length + 1);
                filteredWorkers[filteredWorkers.Length - 1] = worker;
            }
        }

        return filteredWorkers;
    }

    // Форматирование данных Worker в строку
    private string FormatWorkerData(Worker worker)
    {
        return $"{worker.Id}#{worker.Timestamp.ToString("dd.MM.yyyy HH:mm")}#{worker.FIO}#{worker.Age}#{worker.Height}#{worker.DateOfBirth.ToString("dd.MM.yyyy")}#{worker.PlaceOfBirth}";
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1 - Вывести данные на экран");
        Console.WriteLine("2 - Добавить новую запись в файл");
        Console.WriteLine("3 - Удалить запись из файла");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                DisplayEmployees();
                break;
            case "2":
                AddEmployee();
                break;
            case "3":
                DeleteEmployee();
                break;
            default:
                Console.WriteLine("Некорректный выбор.");
                break;
        }
    }

    // Вывод данных всех сотрудников на экран
    static void DisplayEmployees()
    {
        Repository repository = new Repository();
        Worker[] workers = repository.GetAllWorkers();

        if (workers.Length == 0)
        {
            Console.WriteLine("Справочник пуст.");
            return;
        }

        Console.WriteLine("Выберите поле для сортировки:");
        Console.WriteLine("1 - По ID");
        Console.WriteLine("2 - По Ф.И.О.");
        string sortChoice = Console.ReadLine();

        switch (sortChoice)
        {
            case "1":
                Array.Sort(workers, (w1, w2) => w1.Id.CompareTo(w2.Id));
                break;
            case "2":
                Array.Sort(workers, (w1, w2) => w1.FIO.CompareTo(w2.FIO));
                break;
            default:
                Console.WriteLine("Некорректный выбор сортировки.");
                return;
        }

        foreach (Worker worker in workers)
        {
            Console.WriteLine("ID: " + worker.Id);
            Console.WriteLine("Дата и время добавления записи: " + worker.Timestamp.ToString("dd.MM.yyyy HH:mm"));
            Console.WriteLine("Ф.И.О.: " + worker.FIO);
            Console.WriteLine("Возраст: " + worker.Age);
            Console.WriteLine("Рост: " + worker.Height);
            Console.WriteLine("Дата рождения: " + worker.DateOfBirth.ToString("dd.MM.yyyy"));
            Console.WriteLine("Место рождения: " + worker.PlaceOfBirth);
            Console.WriteLine();
        }
    }

    // Добавление новой записи о сотруднике
    static void AddEmployee()
    {
        Repository repository = new Repository();

        Console.WriteLine("Введите данные нового сотрудника:");
        Console.Write("ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Ф.И.О.: ");
        string fullName = Console.ReadLine();

        Console.Write("Возраст: ");
        int age = int.Parse(Console.ReadLine());

        Console.Write("Рост: ");
        int height = int.Parse(Console.ReadLine());

        Console.Write("Дата рождения (в формате дд.мм.гггг): ");
        DateTime dateOfBirth = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);

        Console.Write("Место рождения: ");
        string placeOfBirth = Console.ReadLine();

        Worker newWorker = new Worker
        {
            Id = id,
            FIO = fullName,
            Age = age,
            Height = height,
            DateOfBirth = dateOfBirth,
            PlaceOfBirth = placeOfBirth
        };

        repository.AddWorker(newWorker);

        Console.WriteLine("Новая запись добавлена в файл.");
    }

    // Удаление записи о сотруднике
    static void DeleteEmployee()
    {
        Repository repository = new Repository();

        Console.Write("Введите ID записи, которую необходимо удалить: ");
        int id = int.Parse(Console.ReadLine());

        repository.DeleteWorker(id);

        Console.WriteLine("Запись удалена из файла.");
    }
}
