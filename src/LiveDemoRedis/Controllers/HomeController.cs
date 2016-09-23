using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace LiveDemoRedis.Controllers
{
    public class HomeController : Controller
    {
        private IDistributedCache _cache;

        public HomeController(IDistributedCache cache)
        {
            _cache = cache;
        }

        private void ArmazenarValorCache(
            string chave, string valor)
        {
            DistributedCacheEntryOptions opcoesCache =
                new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration(
                TimeSpan.FromMinutes(3));

            _cache.SetString(chave, valor);
        }

        public IActionResult Index()
        {
            string testeString =
                _cache.GetString("TesteString");
            if (testeString == null)
            {
                testeString = "Valor de exemplo";
                ArmazenarValorCache("TesteString", testeString);
            }
            ViewBag.TesteString = testeString;

            TipoComplexo objetoComplexo = null;
            string strObjetoComplexo =
                _cache.GetString("TesteObjetoComplexo");
            if (strObjetoComplexo == null)
            {
                objetoComplexo = new TipoComplexo();
                objetoComplexo.Texto = "Texto de exemplo";
                objetoComplexo.ValorInteiro = 22;
                objetoComplexo.ValorNumerico = 2016.09;

                strObjetoComplexo =
                    JsonConvert.SerializeObject(objetoComplexo);
                ArmazenarValorCache("TesteObjetoComplexo", strObjetoComplexo);
            }
            else
            {
                objetoComplexo =
                    JsonConvert.DeserializeObject<TipoComplexo>(strObjetoComplexo);
            }
            ViewBag.ObjetoComplexo = objetoComplexo;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
