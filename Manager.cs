namespace ConsoleApp1;

public class Manager : User
{
    public override string role => "Manager";

    public Manager(string username, string password) : base(username, password)
    {
        TaskManagementSystem.Instance.AddManager(this);
    }

    public Task CreateTask(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Error: Task title cannot be empty.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Error: Task description cannot be empty.");
            return null;
        }
        
        Task newTask = new Task(title, description);
        TaskManagementSystem.Instance.AddTask(newTask);
        Console.WriteLine($"Task '{title}' has been created with ID {newTask.projectId}.");
        return newTask;
    }

    public void AssignTask(Task task, Employee employee)
    {
        if (task == null)
        {
            Console.WriteLine("Error: Task cannot be null.");
            return;
        }

        if (employee == null)
        {
            Console.WriteLine("Error: Employee cannot be null.");
            return;
        }
        
        task.assignedEmployee = employee.username;
        employee.assignTask(task);
        
        Console.WriteLine($"Task '{task.title}' (ID: {task.projectId}) has been assigned to {employee.username}.");
    }
}