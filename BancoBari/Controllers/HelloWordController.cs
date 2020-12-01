using BancoBari.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BancoBari.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWordController : ControllerBase
    {
        private ILogger<HelloWord> _logger;

        public HelloWordController(ILogger<HelloWord> logger)
        {
            _logger = logger;
        }

        public IActionResult GetHelloWord()
        {
            try
            {
                HelloWord helloWord = new HelloWord(
                    Guid.NewGuid(), 
                    "Hello World!", 
                    DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var filaMessage = JsonSerializer.Serialize(helloWord);
                    var body = Encoding.UTF8.GetBytes(filaMessage);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", filaMessage);
                }

                return Accepted(helloWord);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao tentar executar mensagem", ex);

                return new StatusCodeResult(500);
            }
            
        }
    }
}
