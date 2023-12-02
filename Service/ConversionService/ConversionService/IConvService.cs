using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ConversionService
{
    [ServiceContract]
    public interface IConvService
    {
        [OperationContract]
        string GetData(string value);
    }
}
