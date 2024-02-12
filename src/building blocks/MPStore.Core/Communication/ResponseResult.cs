using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Core.Communication
{
    public class ResponseResult
    {
        public string Titulo { get; set; }
        public int Status { get; set; }
        public ResponseErrorMessages Errors { get; set; }
        public ResponseResult() 
        { 
            Errors = new ResponseErrorMessages();
        }
    }
}
