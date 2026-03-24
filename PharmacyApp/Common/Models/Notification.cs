using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models
{
    internal class Notification
    {
        public string title { get; set; }
        public string message { get; set; }

        public Notification(string title, string message)
        {
            this.title = title;
            this.message = message;
        }
    }
}
