using System;
using ConsoleApp1;


class Program
{
    private static void Main(string[] args)
    {
        /*
        Manager manager1 = new Manager("Manager1", "Manager1");
        Employee employee1 = new Employee("Epmloyee1", "Epmloyee1");
        Employee employee2 = new Employee("Epmloyee2", "Epmloyee2");
        Employee employee3 = new Employee("Epmloyee3", "Epmloyee3");

        manager1.CreateTask("test1Task", "simple test");
        manager1.CreateTask("test2Task", "simple 2test");
        */
        TaskManagementSystem.Instance.LoadData();
        ConsoleInteraction.Instance.Start();
    }
}
