using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArticoliWebService.Dtos
{
    public class InfoMsg
    {
        public DateTime Data { get; set; }
        public string Message { get; set; }

        public InfoMsg(DateTime Data, String Message)
        {
            this.Data = Data;
            this.Message = Message;
        }
    }
}