using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Demo.Graphql
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var serviceCollection = new ServiceCollection();

      serviceCollection
          .AddLatihan1Client()
          .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5000/graphql"));

      IServiceProvider services = serviceCollection.BuildServiceProvider();
      ILatihan1Client client = services.GetRequiredService<ILatihan1Client>();

      Console.WriteLine("===Get All Todos===");
      var result = await client.GetTodos.ExecuteAsync();
      foreach (var todo in result.Data.Todos)
      {
        Console.WriteLine($"id: {todo.Id}, Task: {todo.Task}, Completed: {todo.Completed}, Created: {todo.Created_At}");

      }
      Console.WriteLine("===Get By ID: 1===");
      var resById = await client.GetTodoById.ExecuteAsync(1);
      foreach (var todo in resById.Data.TodoById)
      {
        Console.WriteLine($"id: {todo.Id}, Task: {todo.Task}, Completed: {todo.Completed}, Created: {todo.Created_At}");

      }

      Console.WriteLine("===Add Todo===");
      AddTodoInput add = new AddTodoInput
      {
        Task = "belajar graphql",
        Completed = false
      };

      var resAdd = await client.AddTodo.ExecuteAsync(add);
      var id = resAdd.Data.AddTodo.Todo.Id;
      var task = resAdd.Data.AddTodo.Todo.Task;
      Console.WriteLine($"==> Data berhasil ditambahkan.");
      Console.WriteLine($"id: {id}, Task: {task}");

      Console.WriteLine("===Update Todo id: 7===");
      AddTodoInput update = new AddTodoInput
      {
        Task = "belanja sudah selesaui",
        Completed = true
      };

      var resUpdate = await client.UpdateTodo.ExecuteAsync(8, update);
      var idUpdate = resUpdate.Data.UpdateTodo.Todo.Id;
      var taskUpdate = resUpdate.Data.UpdateTodo.Todo.Task;
      Console.WriteLine($"==> Data berhasil diupdate.");
      Console.WriteLine($"id: {idUpdate}, Task: {taskUpdate}");

      Console.WriteLine("===Delete Todo id: 4===");
      var resDelete = await client.DeleteTodo.ExecuteAsync(4);
      var todoDel = resDelete.Data.DeleteTodo.Todo;
      Console.WriteLine($"==> Data berhasil didelete.");
      Console.WriteLine($"id: {todoDel.Id}, Task: {todoDel.Task}, Completed: {todoDel.Completed}, Created: {todoDel.Created_At}");
    }
  }
}
