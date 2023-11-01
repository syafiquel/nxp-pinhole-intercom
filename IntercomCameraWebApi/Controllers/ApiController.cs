using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IntercomCameraWebApi.Data;
using IntercomCameraWebApi.Models;
using IntercomCameraWebApi.Services;

namespace IntercomCameraWebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : Controller
    {

        private bool _timerStarted;
        private readonly WebApiDbContext _context;
        private readonly SemaphoreSlim _semaphore;
        private readonly ILogger<ApiController> _logger;
        private readonly IHttpClientService _httpClientService;
        private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();


        public ApiController(WebApiDbContext context, ILogger<ApiController> logger, IHttpClientService httpClientService)
        {
            _context = context;
            _semaphore = new SemaphoreSlim(1, 1);
            _logger = logger;
            _httpClientService = httpClientService;
            _timerStarted = false;
        }

        // For Api testing
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            var data = _context.WebApiData;
            return Ok(data);
        }


        // Query and aggregate camera status
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            System.Timers.Timer timer;
            var data = _context.WebApiData.ToList();
            var totalOffline = data.Where(datum => datum.Status == "Inactive").ToList().Count;
            var totalOnline = data.Where(datum => datum.Status == "Active").ToList().Count;
            var B101Online = data.Where(datum => datum.Block == "B1-01" &&  datum.Status == "Active").ToList().Count;
            var B101Offline = data.Where(datum => datum.Block == "B1-01" && datum.Status == "Inactive" ).ToList().Count;
            var B102Online = data.Where(datum => datum.Block == "B1-02" && datum.Status == "Active").ToList().Count;
            var B102Offline = data.Where(datum => datum.Block == "B1-02" && datum.Status == "Inactive").ToList().Count;
            var B103Online = data.Where(datum => datum.Block == "B1-03" && datum.Status == "Active").ToList().Count;
            var B103Offline = data.Where(datum => datum.Block == "B1-03" && datum.Status == "Inactive").ToList().Count;
            var B1MuliaOnline = data.Where(datum => datum.Block == "B1-MULIA" && datum.Status == "Active").ToList().Count;
            var B1MuliaOffline = data.Where(datum => datum.Block == "B1-MULIA" && datum.Status == "Inactive").ToList().Count;
            var B104Online = data.Where(datum => datum.Block == "B1-04" && datum.Status == "Active").ToList().Count;
            var B104Offline = data.Where(datum => datum.Block == "B1-04" && datum.Status == "Inactive").ToList().Count;
            var B106Online = data.Where(datum => datum.Block == "B1-06" && datum.Status == "Active").ToList().Count;
            var B106Offline = data.Where(datum => datum.Block == "B1-06" && datum.Status == "Inactive").ToList().Count;
            var B107Online = data.Where(datum => datum.Block == "B1-07" && datum.Status == "Active").ToList().Count;
            var B107Offline = data.Where(datum => datum.Block == "B1-07" && datum.Status == "Inactive").ToList().Count;
            var B201Online = data.Where(datum => datum.Block == "B2-01" && datum.Status == "Active").ToList().Count;
            var B201Offline = data.Where(datum => datum.Block == "B2-01" && datum.Status == "Inactive").ToList().Count;
            var B202Online = data.Where(datum => datum.Block == "B2-02" && datum.Status == "Active").ToList().Count;
            var B202Offline = data.Where(datum => datum.Block == "B2-02" && datum.Status == "Inactive").ToList().Count;
            var B203Online = data.Where(datum => datum.Block == "B2-03" && datum.Status == "Active").ToList().Count;
            var B203Offline = data.Where(datum => datum.Block == "B2-03" && datum.Status == "Inactive").ToList().Count;
            var B2MuliaOnline = data.Where(datum => datum.Block == "B2-MULIA" && datum.Status == "Active").ToList().Count;
            var B2MuliaOffline = data.Where(datum => datum.Block == "B2-MULIA" && datum.Status == "Inactive").ToList().Count;
            var B204Online = data.Where(datum => datum.Block == "B2-04" && datum.Status == "Active").ToList().Count;
            var B204Offline = data.Where(datum => datum.Block == "B2-04" && datum.Status == "Inactive").ToList().Count;
            var B205Online = data.Where(datum => datum.Block == "B2-05" && datum.Status == "Active").ToList().Count;
            var B205Offline = data.Where(datum => datum.Block == "B2-05" && datum.Status == "Inactive").ToList().Count;
            var B2MezzanineOnline = data.Where(datum => datum.Block == "MEZZANINE" && datum.Status == "Active").ToList().Count;
            var B2MezzanineOffline = data.Where(datum => datum.Block == "MEZZANINE" && datum.Status == "Inactive").ToList().Count;

            ViewData["data"] = data;
            ViewData["total_cameras"] = data.Count;
            ViewData["total_offline"] = totalOffline;
            ViewData["total_online"] = totalOnline;
            ViewData["B101Online"] = B101Online;
            ViewData["B101Offline"] = B101Offline;
            ViewData["B102Online"] = B102Online;
            ViewData["B102Offline"] = B102Offline;
            ViewData["B103Online"] = B103Online;
            ViewData["B103Offline"] = B103Offline;
            ViewData["B1MuliaOnline"] = B1MuliaOnline;
            ViewData["B1MuliaOffline"] = B1MuliaOffline;
            ViewData["B104Online"] = B104Online;
            ViewData["B104Offline"] = B104Offline;
            ViewData["B106Online"] = B106Online;
            ViewData["B106Offline"] = B106Offline;
            ViewData["B107Online"] = B107Online;
            ViewData["B107Offline"] = B107Offline;
            ViewData["B201Online"] = B201Online;
            ViewData["B201Offline"] = B201Offline;
            ViewData["B202Online"] = B202Online;
            ViewData["B202Offline"] = B202Offline;
            ViewData["B203Online"] = B203Online;
            ViewData["B203Offline"] = B203Offline;
            ViewData["B2MuliaOnline"] = B2MuliaOnline;
            ViewData["B2MuliaOffline"] = B2MuliaOffline;
            ViewData["B204Online"] = B204Online;
            ViewData["B204Offline"] = B204Offline;
            ViewData["B205Online"] = B205Online;
            ViewData["B205Offline"] = B205Offline;
            ViewData["B2MezzanineOnline"] = B2MezzanineOnline;
            ViewData["B2MezzanineOffline"] = B2MezzanineOffline;

            if(!_timerStarted) {
                Console.WriteLine("timer started");
                _timerStarted = true;
                timer = new System.Timers.Timer(5 * 60 * 1000);
                timer.Elapsed += (sender, e) =>  TimerEventHandler(sender, e);
                timer.AutoReset = true;
                timer.Start();
            }

            
            return View();
        }

        // Handle intercom outgoing, established and terminated events
        [HttpGet("event/intercom")]
        public async Task<IActionResult> intercom([FromQuery(Name = "id")] string id, [FromQuery(Name = "action")] string action) {
            Int32 cameraId = Convert.ToInt32(id);
            List<String> data = new List<String>();
            var entity = await _context.WebApiData.FindAsync(cameraId);
            byte[] buffer;
            switch(action)
            {
                case "outgoing":
                    data.Add("outgoing");
                    
                    data.Add(JsonSerializer.Serialize(entity));
                    buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
                    data.Clear();

                    foreach (WebSocket socket in _sockets.Values.Where(s => s.State == WebSocketState.Open))
                    {
                        await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, default);
                    }
                    break;
                case "established":
                    data.Add("established");
                    data.Add(JsonSerializer.Serialize(entity));
                    buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
                    data.Clear();

                    foreach (WebSocket socket in _sockets.Values.Where(s => s.State == WebSocketState.Open))
                    {
                        await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, default);
                    }
                    
                    break;
                case "terminated":
                    data.Add("terminated");
                    data.Add(JsonSerializer.Serialize(entity));
                    buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
                    data.Clear();
                    foreach (WebSocket socket in _sockets.Values.Where(s => s.State == WebSocketState.Open))
                    {
                        await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, default);
                    }
                    break;
                default:
                    break;
            }
            return Ok("Ok");
            
        }

        // Initialize web socket connection
        [HttpGet("ws")]
        public async Task GetWebSocket()
        {
                try {
                    using(WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync()) {
                        string socketId = Guid.NewGuid().ToString();
                        _sockets.TryAdd(socketId, webSocket);
                        await Echo(webSocket);
                    }
                        
                }

                catch(Exception ex) {
                    _logger.LogError(ex.ToString());
                }
                

        }

        // Return floor mapping asset
        [HttpGet("asset")]
        public IActionResult GetImage([FromQuery(Name = "name")] string name)
        {
            string filePath = "Static/Images/" + name;
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);

            string contentType = "image/jpeg";

            // Return the image as a file result
            return File(imageBytes, contentType);
        }

        private async Task Send() {
            await Task.Run(() => log());
        }

        private void log() {
            Console.WriteLine("web socket connected");
        }

        // Broadcast web socket data
        private async Task Echo(WebSocket webSocket)
        {
            byte[] buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private void TimerEventHandler(object sender, System.Timers.ElapsedEventArgs e) {
            _ = RefreshStatus();
        }

        private async Task RefreshStatus() {

            await _semaphore.WaitAsync();
            bool pinholeStatus;
            bool intercomStatus;
            bool status = false;
            bool current;
            List<String> payload = new List<String>();
            byte[] buffer;
            WebApiData contextData = new WebApiData();
            var optionsBuilder = new DbContextOptionsBuilder<WebApiDbContext>();
            optionsBuilder.UseNpgsql("Server=db;Port=5432;Database=trx;User ID=postgres;Password=postgres");
            WebApiDbContext dbContext = new WebApiDbContext(optionsBuilder.Options);
            try {
                    var data = dbContext.WebApiData.ToList();
                    Console.WriteLine(data);
                    foreach(var datum in data) {
                        contextData = datum;
                        var pinholeUrl = datum.URL;
                        var intercomUrl = "http://" + datum.IntercomIP + "/";
                        current = (datum.Status == "Active") ? true : false; 
                        HttpClient httpClient = _httpClientService.CreateClient();
                        httpClient.Timeout = TimeSpan.FromSeconds(10);
                        HttpResponseMessage response = await httpClient.GetAsync(pinholeUrl);
                        pinholeStatus = (response.IsSuccessStatusCode) ? true : false;
                        Console.WriteLine(pinholeStatus);
                        response = await httpClient.GetAsync(intercomUrl);
                        intercomStatus = (response.IsSuccessStatusCode) ? true : false;
                        Console.WriteLine(intercomStatus);
                        status = (intercomStatus && (pinholeStatus || !pinholeStatus)) ? true : false;
                        Console.WriteLine(status);
                        if(status != current) {
                            var entity = dbContext.WebApiData.Find(datum.Id);
                            entity.Status = (status) ? "Active" : "Inactive";
                            await UpdateDb(dbContext);
                            payload.Add("reload");
                            buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
                            foreach (WebSocket socket in _sockets.Values.Where(s => s.State == WebSocketState.Open))
                            {
                                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, default);
                            }
                        }               
                    }
            }

            catch(Exception ex) {
                Console.WriteLine("timer else")
                var entity = dbContext.WebApiData.Find(contextData.Id);
                entity.Status = (status) ? "Active" : "Inactive";
                await UpdateDb(dbContext);
                payload.Add("reload");
                buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
                foreach (WebSocket socket in _sockets.Values.Where(s => s.State == WebSocketState.Open))
                { 
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, default);
                }
            }

            finally {
                _semaphore.Release();
            }
            
        }

        private async Task UpdateDb(WebApiDbContext _dbContext) {
            await _dbContext.SaveChangesAsync();
        }
    }
}




