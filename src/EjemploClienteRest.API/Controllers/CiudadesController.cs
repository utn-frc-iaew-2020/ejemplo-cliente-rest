using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

//Using TinyRestClient
using Tiny.RestClient;
using System.Net.Http;

//Using clase Ciudad
using ClienteRestAPI.Models;
using ClienteRestAPI.Authorization;

namespace ClienteRestAPI.Controllers
{
    [Route("api/[controller]")]
    public class CiudadesController : ControllerBase
    {

        private readonly ILogger<CiudadesController> _logger;
        private readonly IConfiguration _config;
        public CiudadesController(ILogger<CiudadesController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public async Task<IEnumerable<Ciudad>> Get()
        {
            try
            {
                _logger.LogInformation("Inicio");

                // Consumir API Proveedor
                // Creamos una instancia de Client TinyRestClient,
                // indicando la url base de la API que queremos consumir
                var client = new TinyRestClient(new HttpClient(), _config["url_api_proveedor"]);
                // El metodo GetRequest prepara el request HTTP a 
                // la API en cuestion, en este caso /ciudades               
                var ciudades = await client.
                                // El metodo GetRequest prepara el request HTTP a la API en cuestion, en este caso /ciudades   
                                GetRequest("Ciudades").
                                //Para agregar parametros en el request
                                AddQueryParameter("name", "valor"). 
                                //Agregamos el Token OAuth 2.0            
                                WithOAuthBearer(await AuthorizationHelper.ObtenerAccessToken()).
                                // El metodo ExecuteAsync ejecuta la peticio HTTP y con la 
                                // respuesta construye una lista de Ciudades (List<Ciudad>)
                                ExecuteAsync<List<Ciudad>>();

                return ciudades;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                return null;
            }
        }
    }
}
