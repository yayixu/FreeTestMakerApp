using System;
using System.Collections.Generic;
using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {
        private ApplicationDbContext DbContext;

        public ResultController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        #region RESTful conventions methods

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = DbContext.Results.Where(i => i.Id == id).FirstOrDefault();

            // handle requests asking for non-existing results
            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", id)
                });
            }

            return new JsonResult(
                result.Adapt<ResultViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        [HttpPut]
        public IActionResult Put([FromBody]ResultViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            var result = model.Adapt<Result>();

            result.CreatedDate = DateTime.Now;
            result.LastModifiedDate = result.CreatedDate;

            DbContext.Results.Add(result);
            DbContext.SaveChanges();

            return new JsonResult(result.Adapt<ResultViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        [HttpPost]
        public IActionResult Post([FromBody]ResultViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            var result = DbContext.Results.Where(i => i.Id == model.Id).FirstOrDefault();

            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", model.Id)
                });
            }

            result.QuizId = model.QuizId;
            result.Text = model.Text;
            result.MinValue = model.MinValue;
            result.MaxValue = model.MaxValue;
            result.Notes = model.Notes;
            result.LastModifiedDate = result.CreatedDate;

            DbContext.SaveChanges();

            return new JsonResult(result.Adapt<ResultViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = DbContext.Results.Where(i => i.Id == id).FirstOrDefault();

            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", id)
                });
            }

            DbContext.Results.Remove(result);
            DbContext.SaveChanges();

            return new OkResult();
        }
        #endregion

        // GET: api/question/all
        [HttpGet("All/{quizeId}")]
        public IActionResult All(int quizeId)
        {
            var results = DbContext.Results.Where(r => r.QuizId == quizeId).ToArray();

            // output the result in JSON format
            return new JsonResult(
                results.Adapt<ResultViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }
    }
}
