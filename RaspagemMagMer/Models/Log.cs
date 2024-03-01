using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemMagMer.Models
{
    public class Log
    {
        [Key]
        public int IdLog { get; set; }
        public string CodRob { get; set; }
        public string UsuRob { get; set; }
        public DateTime DateLog { get; set; }
        public string Processo { get; set; }
        public string InfLog { get; set; }
        public int IdProd { get; set; }
    }

}
