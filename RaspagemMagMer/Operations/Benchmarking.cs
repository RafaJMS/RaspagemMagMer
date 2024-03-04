using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemMagMer.Operations
{
    public class Benchmarking
    {
        public static string CompareValue(string precoMag, string precoMer,string linkMer,string linkMag)
        {
            char[] charRemove = { 'R', '$', ' ' };

            decimal mercadoLivrePreco = Convert.ToDecimal(precoMer.Trim(charRemove));
            decimal magazineLuizaPreco = Convert.ToDecimal(precoMag.Trim(charRemove));

            if (magazineLuizaPreco > mercadoLivrePreco)

            {
                return $"O preço do produto está melhor no Mercado livre, pois está R$ {(magazineLuizaPreco - mercadoLivrePreco)} mais barato\n" +
                       $"Link para produto Mer: {linkMer}";


            }
            else if (magazineLuizaPreco < mercadoLivrePreco)
            {
                return $"O preço do produto está melhor na Magazine Luiza, pois está R$ {mercadoLivrePreco - magazineLuizaPreco} mais barato\n" +
                       $"Link para produto Mag: {linkMag}";

            }
            else
            {
                return "Os preços são equivalentes";
            }
        }
    }
}
