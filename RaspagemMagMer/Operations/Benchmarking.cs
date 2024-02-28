using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemMagMer.Operations
{
    public class Benchmarking
    {
        public static string CompareValue(string precoMag, string precoMer)
        {
            char[] charRemove = { 'R', '$', ' ' };

            double mercadoLivrePreco = Convert.ToDouble(precoMer.Trim(charRemove));
            double magazineLuizaPreco = Convert.ToDouble(precoMag.Trim(charRemove));

            if (magazineLuizaPreco > mercadoLivrePreco)

            {
                
                return $"O preço do produto está melhor no Mercado livre, pois está R${magazineLuizaPreco-mercadoLivrePreco} mais barato";

            }
            else if (magazineLuizaPreco < mercadoLivrePreco)
            {
                return $"O preço do produto está melhor na Magazine Luiza, pois está R${mercadoLivrePreco-magazineLuizaPreco} mais barato";
            }
            else
            {
                return "Os preços são equivalentes";
            }
        }
    }
}
