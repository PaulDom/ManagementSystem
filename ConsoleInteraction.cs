namespace ConsoleApp1;

public class ConsoleInteraction
{
    private static ConsoleInteraction _instance;

    private TaskManagementSystem _taskManagementSystem = TaskManagementSystem.Instance;
    private User _currentUser;

    private ConsoleInteraction() { }

    public static ConsoleInteraction Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ConsoleInteraction();
            }
            return _instance;
        }
    }

    public void Start()
    {
        Console.WriteLine("Welcome to the Task Management System!");

        while (true)
        {
            while (_currentUser == null)
            {
                Console.WriteLine("\nPlease log in:");
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                _currentUser = AuthenticateUser(username, password);

                if (_currentUser == null)
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                }
            }

            Console.WriteLine($"\nWelcome, {_currentUser.username}! You are logged in as a {_currentUser.role}.");

            if (_currentUser is Manager manager)
            {
                ManagerMenu(manager);
            }
            else if (_currentUser is Employee employee)
            {
                EmployeeMenu(employee);
            }
            else
            {
                Console.WriteLine("Unknown role. Exiting...");
            }
        }
    }

    private User AuthenticateUser(string username, string password)
    {
        Manager manager = _taskManagementSystem.FindManagerByUsername(username);
        if (manager != null && manager.password == password)
        {
            return manager;
        }
        
        Employee employee = _taskManagementSystem.FindEmployeeByUsername(username);
        if (employee != null && employee.password == password)
        {
            return employee;
        }

        return null;
    }

    private void ManagerMenu(Manager manager)
    {
        while (true)
        {
            Console.WriteLine("\nManager Menu:");
            Console.WriteLine("1. Create Task");
            Console.WriteLine("2. Assign Task");
            Console.WriteLine("3. View All Tasks");
            Console.WriteLine("4. View Employees");
            Console.WriteLine("5. Logout");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter task title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter task description: ");
                    string description = Console.ReadLine();
                    manager.CreateTask(title, description);
                    break;

                case "2":
                    Console.Write("Enter task ID to assign: ");
                    if (int.TryParse(Console.ReadLine(), out int taskId))
                    {
                        Task task = _taskManagementSystem.FindTaskById(taskId);
                        if (task == null)
                        {
                            Console.WriteLine("Task not found.");
                            break;
                        }

                        Console.Write("Enter employee username to assign task: ");
                        string employeeUsername = Console.ReadLine();
                        Employee employee = _taskManagementSystem.FindEmployeeByUsername(employeeUsername);
                        if (employee == null)
                        {
                            Console.WriteLine("Employee not found.");
                        }
                        else
                        {
                            manager.AssignTask(task, employee);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid task ID.");
                    }
                    break;

                case "3":
                    _taskManagementSystem.ViewAllTasks();
                    break;

                case "4":
                    _taskManagementSystem.ViewAllEmployees();
                    break;

                case "5":
                    Console.WriteLine("Logging out...");
                    _currentUser = null;
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void EmployeeMenu(Employee employee)
    {
        while (true)
        {
            Console.WriteLine("\nEmployee Menu:");
            Console.WriteLine("1. View Assigned Tasks");
            Console.WriteLine("2. Update Task Status");
            Console.WriteLine("3. Logout");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    employee.ViewAssignedTasks();
                    break;
                case "2":
                    Console.Write("Enter task ID to update: ");
                    if (int.TryParse(Console.ReadLine(), out int taskId))
                    {
                        Task task = employee.assignedTasks.Find(t => t.projectId == taskId);
                        if (task == null)
                        {
                            Console.WriteLine("Task not found or not assigned to you.");
                            break;
                        }

                        var statuses = Enum.GetValues(typeof(Task.taskStatus));
                        Console.WriteLine("Select new status:");
                        int index = 1;
                        
                        foreach (var status in statuses)
                        {
                            Console.WriteLine($"{index}. {status}");
                            index++;
                        }

                        Console.Write("Enter your choice: ");
                        if (int.TryParse(Console.ReadLine(), out int choiceNumber) &&
                            choiceNumber >= 1 && choiceNumber <= statuses.Length)
                        {
                            Task.taskStatus newStatus = (Task.taskStatus)statuses.GetValue(choiceNumber - 1);
                            employee.UpdateTaskStatus(task, newStatus);
                            Console.WriteLine($"Status successfully updated to '{newStatus}'.");
                        }
                        else
                        {
                            Console.WriteLine("Incorrect selection. Status not updated.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid task ID.");
                    }
                    break;

                case "3":
                    Console.WriteLine("Logging out...");
                    _currentUser = null;
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
