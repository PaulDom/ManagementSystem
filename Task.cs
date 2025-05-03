namespace ConsoleApp1;

public class Task
{
    private static int _idCounter = 0;
    public enum taskStatus
    {
        toDo,
        inProgress,
        done
    }

    public string assignedEmployee { get; set; }
    public int projectId { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public taskStatus status { get; set; } = taskStatus.toDo;

    public void SetStatus(taskStatus newStatus)
    {
        status = newStatus;
        TaskManagementSystem.Instance.SaveData();
    }
    public Task(string title, string description, string assignedEmployee = "unassigned")
    {
        this.projectId = GenerateId();
        this.title = title;
        this.description = description;
        this.assignedEmployee = assignedEmployee;
    }

    private static int GenerateId()
    {
        return ++_idCounter;
    }
    public void PrintTaskInfo()
    {
        Console.WriteLine($"- Task ID: {projectId}");
        Console.WriteLine($"- Assigned to: {assignedEmployee}");
        Console.WriteLine($"  Title: {title}");
        Console.WriteLine($"  Description: {description}");
        Console.WriteLine($"  Status: {status}");
        Console.WriteLine("---------------------------------");
    }
}