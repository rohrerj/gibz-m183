using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging.Models
{
    public class LogsViewModel
    {
        public List<(string browser, string ip, string action, int result, string message, DateTime creationDate)> Logs { get; set; } = new List<(string browser, string ip, string action, int result, string message, DateTime creationDate)>();
    }
}