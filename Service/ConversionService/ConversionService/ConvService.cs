using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ConversionService
{
    public class ConvService : IConvService
    {        
        public string GetData(string value)
        {
            Dollar.DollarConversion obj = new Dollar.DollarConversion();           
            return obj.GetData(value);
        }
    }
}
