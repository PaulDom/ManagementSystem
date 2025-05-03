using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ConsoleApp1;

public class TaskManagementSystem
{
    private static TaskManagementSystem _instance;
    private AppData _data = new AppData();
    private static readonly string DataFilePath = "data.json";
    
    private bool _autoSaveEnabled = true;

    public void EnableAutoSave(bool enable)
    {
        _autoSaveEnabled = enable;
    }
    
    public static TaskManagementSystem Instance
    {
        get 
        { 
            if (_instance == null)
            {
                _instance = new TaskManagementSystem();
            }
            return _instance;
        }
    }
    

    public void AddTask(Task task)
    {
        if (task == null)
        {
            Console.WriteLine("Error: Cannot add a null task to the system.");
        }
        else
        {
            _data.Tasks.Add(task);
            SaveData();
            Console.WriteLine($"Task '{task.title}' with ID {task.projectId} has been added to the system.");
        }
    }
    
    public Task FindTaskById(int taskId)
    {
        return _data.Tasks.Find(t => t.projectId == taskId);
    }
    
    public void AddManager(Manager manager)
    {
        if (manager == null)
        {
            Console.WriteLine("Error: Cannot add a null manager to the system.");
        }
        else
        {
            _data.Managers.Add(manager);
            SaveData();
            Console.WriteLine($"Manager '{manager.username}' has been added to the system.");
        }
    }

    public Manager FindManagerByUsername(string username)
    {
        Manager manager = _data.Managers.Find(m => m.username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return manager;
    }
    
    public void AddEmployee(Employee employee)
    {
        if (employee == null)
        {
            Console.WriteLine("Error: Cannot add a null employee to the system.");
        }
        else
        {
            _data.Employees.Add(employee);
            SaveData();
            Console.WriteLine($"Employee '{employee.username}' has been added to the system.");
        }
    }

    public Employee FindEmployeeByUsername(string username)
    {
        Employee employee = _data.Employees.Find(m => m.username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return employee;
    }

    public void ViewAllTasks()
    {
        foreach (var task in _data.Tasks)
        {
            task.PrintTaskInfo();
        }
    }

    public void ViewAllEmployees()
    {
        foreach (var employee in _data.Employees)
        {
            Console.WriteLine($"- username: {employee.username}");
        }
    }

    public void SaveData()
    {
        if (!_autoSaveEnabled)
        {
            return;
        }
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
            string jsonString = JsonSerializer.Serialize(_data, options);
            File.WriteAllText(DataFilePath, jsonString);
            Console.WriteLine("Data has been saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }
    
    public void LoadData()
    {
        try
        {
            EnableAutoSave(false);
            if (!File.Exists(DataFilePath))
            {
                Console.WriteLine("Data file not found. Initializing with empty data.");
                _data = new AppData(); // Инициализация пустыми данными, если файл отсутствует
                return;
            }

            string jsonString = File.ReadAllText(DataFilePath);
            Console.WriteLine($"JSON content: {jsonString}"); // Отладочная информация

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            _data = JsonSerializer.Deserialize<AppData>(jsonString, options);
            if (_data == null)
            {
                Console.WriteLine("Deserialization returned null. Initializing with empty data.");
                _data = new AppData();
            }
            else
            {
                // Восстанавливаем ссылки между задачами и сотрудниками
                SynchronizeData();
                Console.WriteLine("Data has been loaded and synchronized successfully.");
            }
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
            _data = new AppData(); // Инициализация пустыми данными в случае ошибки десериализации
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            _data = new AppData(); // Инициализация пустыми данными в случае общей ошибки
        }
        finally
        {
            EnableAutoSave(true);
        }
    }
    
    private void SynchronizeData()
    {
        if (_data == null || _data.Tasks == null || _data.Employees == null)
            return;

        // Создаём словарь для быстрого доступа к задачам по их ID
        var taskDictionary = _data.Tasks.ToDictionary(task => task.projectId);

        // Проходим по всем сотрудникам
        foreach (var employee in _data.Employees)
        {
            // Синхронизируем задачи у каждого сотрудника
            for (int i = 0; i < employee.assignedTasks.Count; i++)
            {
                var assignedTask = employee.assignedTasks[i];
                if (taskDictionary.TryGetValue(assignedTask.projectId, out var globalTask))
                {
                    // Заменяем задачу из списка сотрудника на глобальную задачу
                    employee.assignedTasks[i] = globalTask;
                }
            }
        }
    }
}