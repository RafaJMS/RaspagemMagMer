using RaspagemMagMer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemMagMer.Operations
{
    public class LogRegister
    {
        public static string CodRobo { get; set; } = "1806";

        public static string UsuRob { get; set; } = "rafaelMecenas";

        public static void RegistrarLog(DateTime dateLog, string processo, string infLog, int idProd)
        {
            using var context = new LogContext();
            var log = new Log
            {
                CodigoRobo = CodRobo,
                UsuarioRobo = UsuRob,
                DateLog = dateLog,
                Etapa = processo,
                InformacaoLog = infLog,
                IdProdutoAPI = idProd
            };
            context.LOGROBO.Add(log);
            context.SaveChanges();
        }
    }
}
