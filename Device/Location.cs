using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Device
{
   public class Location<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
    }
}
