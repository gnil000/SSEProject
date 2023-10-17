using Client;
using System.Net.Http.Json;
using System.Text.Json;

string url = "https://localhost:7235/Task";

Console.WriteLine("Введите количество тасков");
int tasks = Convert.ToInt16(Console.ReadLine());
Console.WriteLine("Выполнять их параллельно?\n1 - true\t2-false");
bool parallel = Console.ReadLine() == "1";

int total = 0; //счётчик суммы времени

var client = new HttpClient();
var response = await client.PostAsJsonAsync(url, new RequestView { tasks = tasks, parallel = parallel });
using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());
while (!reader.EndOfStream)
{
    var line = await reader.ReadLineAsync();
    //Console.WriteLine(line);
    var res = JsonSerializer.Deserialize<ResponseView>(line);
    Console.WriteLine($"{res.order} = {res.time}");
    total += res.time;
}

Console.WriteLine($"total = {total}");
Console.ReadLine();