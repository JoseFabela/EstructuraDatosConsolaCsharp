using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Queue<string> tareasPendientes = new Queue<string>();
        List<string> tareasCompletadas = new List<string>();
        PriorityTree arbolPrioridad = new PriorityTree("Proyecto");
        TaskGraph grafoTareas = new TaskGraph();

        while (true)
        {
            Console.WriteLine("\nMenú:");
            Console.WriteLine("1. Agregar tarea pendiente (Cola)");
            Console.WriteLine("2. Completar tarea (Cola y Lista)");
            Console.WriteLine("3. Ver tareas pendientes (Cola)");
            Console.WriteLine("4. Ver tareas completadas (Lista)");
            Console.WriteLine("5. Agregar tarea al árbol de prioridad");
            Console.WriteLine("6. Ver árbol de prioridad");
            Console.WriteLine("7. Agregar tarea al grafo de tareas");
            Console.WriteLine("8. Ver grafo de tareas (BFS)");
            Console.WriteLine("9. Salir");

            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Ingrese la nueva tarea pendiente: ");
                    string nuevaTarea = Console.ReadLine();
                    tareasPendientes.Enqueue(nuevaTarea);
                    Console.WriteLine($"Tarea \"{nuevaTarea}\" agregada correctamente a la Cola.");
                    break;

                case "2":
                    if (tareasPendientes.Count > 0)
                    {
                        string tareaCompleta = tareasPendientes.Dequeue();
                        tareasCompletadas.Add(tareaCompleta);
                        Console.WriteLine($"Tarea \"{tareaCompleta}\" completada y movida a Lista de Tareas Completadas.");
                    }
                    else
                    {
                        Console.WriteLine("No hay tareas pendientes para completar.");
                    }
                    break;

                case "3":
                    Console.WriteLine("\nTareas Pendientes (Cola):");
                    foreach (var tarea in tareasPendientes)
                    {
                        Console.WriteLine(tarea);
                    }
                    break;

                case "4":
                    Console.WriteLine("\nTareas Completadas (Lista):");
                    foreach (var tarea in tareasCompletadas)
                    {
                        Console.WriteLine(tarea);
                    }
                    break;

                case "5":
                    Console.Write("Ingrese la nueva tarea para el árbol de prioridad: ");
                    string nuevaTareaArbol = Console.ReadLine();
                    Console.Write("Ingrese la prioridad (Alta, Media, Baja): ");
                    string prioridadTareaArbol = Console.ReadLine();
                    arbolPrioridad.Insert(nuevaTareaArbol, prioridadTareaArbol);
                    Console.WriteLine($"Tarea \"{nuevaTareaArbol}\" agregada al Árbol de Prioridad.");
                    break;

                case "6":
                    Console.WriteLine("\nÁrbol de Prioridad:");
                    arbolPrioridad.InOrderTraversal(arbolPrioridad.Root);
                    break;

                case "7":
                    Console.Write("Ingrese la nueva tarea para el grafo de tareas: ");
                    string nuevaTareaGrafo = Console.ReadLine();
                    grafoTareas.AddTask(nuevaTareaGrafo);

                    Console.Write($"Ingrese dependencias de '{nuevaTareaGrafo}' (separadas por coma): ");
                    string[] dependencias = Console.ReadLine().Split(',');

                    foreach (var dependency in dependencias)
                    {
                        grafoTareas.AddTaskDependency(nuevaTareaGrafo, dependency.Trim());
                    }

                    Console.WriteLine($"Tarea '{nuevaTareaGrafo}' agregada al Grafo de Tareas.");
                    break;


                case "8":
                    Console.Write("Ingrese la tarea para iniciar BFS en el grafo de tareas: ");
                    string tareaInicioBFS = Console.ReadLine();
                    grafoTareas.BFS(tareaInicioBFS);
                    break;

                case "9":
                    Console.WriteLine("Saliendo del programa. ¡Hasta luego!");
                    return;

                default:
                    Console.WriteLine("Opción no válida. Por favor, ingrese un número del 1 al 9.");
                    break;
            }
        }
    }
}

    // Implementación de árbol de prioridad
    class PriorityTree
{
    public class Node
    {
        public string Task;
        public string Priority;
        public Node Left, Right;

        public Node(string task, string priority)
        {
            Task = task;
            Priority = priority;
            Left = Right = null;
        }
    }

    public Node Root;

    public PriorityTree(string rootTask)
    {
        Root = new Node(rootTask, "Media");
    }

    public void Insert(string task, string priority)
    {
        Root = InsertRecursive(Root, task, priority);
    }

    private Node InsertRecursive(Node root, string task, string priority)
    {
        if (root == null)
        {
            return new Node(task, priority);
        }

        if (String.Compare(priority, root.Priority) < 0)
        {
            root.Left = InsertRecursive(root.Left, task, priority);
        }
        else
        {
            root.Right = InsertRecursive(root.Right, task, priority);
        }

        return root;
    }

    public void InOrderTraversal(Node root)
    {
        if (root != null)
        {
            InOrderTraversal(root.Left);
            Console.WriteLine($"{root.Task} - {root.Priority}");
            InOrderTraversal(root.Right);
        }
    }
}

// Implementación de grafo de tareas
class TaskGraph
{
    private Dictionary<string, List<string>> taskDependencies;

    public TaskGraph()
    {
        taskDependencies = new Dictionary<string, List<string>>();
    }

    public void AddTask(string task)
    {
        if (!taskDependencies.ContainsKey(task))
        {
            taskDependencies[task] = new List<string>();
        }
    }

    public void AddTaskDependency(string task, string dependency)
    {
        if (taskDependencies.ContainsKey(task))
        {
            taskDependencies[task].Add(dependency);
        }
    }

    public void BFS(string startTask)
    {
        if (!taskDependencies.ContainsKey(startTask))
        {
            Console.WriteLine($"La tarea '{startTask}' no existe en el grafo de tareas.");
            return;
        }

        HashSet<string> visited = new HashSet<string>();
        Queue<string> queue = new Queue<string>();

        visited.Add(startTask);
        queue.Enqueue(startTask);

        while (queue.Count > 0)
        {
            string currentTask = queue.Dequeue();
            Console.WriteLine(currentTask);

            if (taskDependencies.ContainsKey(currentTask))
            {
                foreach (var dependency in taskDependencies[currentTask])
                {
                    if (!visited.Contains(dependency))
                    {
                        visited.Add(dependency);
                        queue.Enqueue(dependency);
                    }
                }
            }
        }
    }


}
