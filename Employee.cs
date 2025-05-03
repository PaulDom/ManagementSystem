namespace ConsoleApp1;

public class Employee : User
{
    public override string role => "Employee";

    public Employee(string username, string password) : base(username, password)
    {
        TaskManagementSystem.Instance.AddEmployee(this); 
    }
    public List<Task> assignedTasks { get; set; } = new List<Task>();

    public void assignTask(Task task)
    {
        if (!assignedTasks.Contains(task))
        {
            assignedTasks.Add(task); 
            TaskManagementSystem.Instance.SaveData();
        }
    }
    
    public void UpdateTaskStatus(Task task, Task.taskStatus newStatus)
    {
        if (assignedTasks.Contains(task))
        {
            task.SetStatus(newStatus);
            Console.WriteLine($"Task '{task.title}' status updated to {newStatus}.");
        }
        else
        {
            Console.WriteLine("You cannot update the status of a task that is not assigned to you.");
        }
    }

    public void ViewAssignedTasks()
    {
        if (assignedTasks.Count == 0)
        {
            Console.WriteLine("No tasks have been assigned to you.");
        }
        else
        {
            Console.WriteLine("Assigned Tasks:");
            foreach (var task in assignedTasks)
            {
                task.PrintTaskInfo();
            }
        }
    }
}
