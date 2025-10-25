using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Interfaces
{
    public interface IResult<T>
    {
        string Message { get; set; }

        bool Succeeded { get; set; }

        T Data { get; set; }

        //List<ValidationResult> ValidationErrors { get; set; }

        // Exception Exception { get; set; }

        int Code { get; set; }
    }
}
