using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoBari.Domain
{
    public sealed class HelloWord
    {
        public HelloWord(Guid id, string message, string tempo)
        {
            Id = id;
            Message = message;
            Tempo = tempo;
        }

        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Tempo { get; set; }
    }
}
