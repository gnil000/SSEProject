using Microsoft.AspNetCore.Mvc;
using SSEProject.Models;
using System.Text;
using System.Text.Json;

namespace TaskForAutodor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        [HttpPost]
        public async Task Get(RequestView rv)
        {
            Random random = new Random();

            Response.Headers.Add("Content-Type", "text/event-stream");

            if (rv.tasks < 3 || rv.tasks > 25)
            {
                string message = $"{JsonSerializer.Serialize("Количество таск должно быть не меньше 3 и не больше 25")}\n\n";
                byte[] messageBytes = ASCIIEncoding.UTF8.GetBytes(message);
                await Response.Body.WriteAsync(messageBytes, 0, messageBytes.Length);
                await Response.Body.FlushAsync();
            }
            else
            {
                int countr = 0;
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < rv.tasks; i++)
                {
                    var t = Task.Run(async () =>
                    {
                        int time = Random.Shared.Next(100, 1000);

                        await Task.Delay(time);

                        countr = Interlocked.Increment(ref countr);
                        string message = $"{JsonSerializer.Serialize(new ResponseView(countr, time))}\n";
                        byte[] messageBytes = ASCIIEncoding.ASCII.GetBytes(message);

                        await Response.Body.WriteAsync(messageBytes, 0, messageBytes.Length);
                        await Response.Body.FlushAsync();
                    });
                    if (rv.parallel)
                        tasks.Add(t);
                    else
                        await t;
                }
                Task.WaitAll(tasks.ToArray());
            }
        }

    }

}
