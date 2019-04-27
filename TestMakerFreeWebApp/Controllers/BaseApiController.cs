using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMakerFreeWebApp.Data;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        protected ApplicationDbContext DbContext { get; private set; }
        protected JsonSerializerSettings JsonSetting { get; private set; }


        public BaseApiController(ApplicationDbContext context)
        {
            DbContext = context;

            JsonSetting = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }



    }
}
