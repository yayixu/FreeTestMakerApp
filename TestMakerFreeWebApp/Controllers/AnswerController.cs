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
    public class AnswerController : Controller
    {
        private ApplicationDbContext DbContext;

        public AnswerController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        #region RESTful conventions methods

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = DbContext.Answers.Where(i => i.Id == id).FirstOrDefault();

            // handle requests asking for non-existing answers
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", id)
                });
            }

            return new JsonResult(
                answer.Adapt<AnswerViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            var answer = model.Adapt<Answer>();

            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Notes = model.Notes;
            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;

            DbContext.Answers.Add(answer);
            DbContext.SaveChanges();

            return new JsonResult(answer.Adapt<AnswerViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            var answer = DbContext.Answers.Where(i => i.Id == model.Id).FirstOrDefault();

            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", model.Id)
                });
            }

            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Notes = model.Notes;
            answer.LastModifiedDate = answer.CreatedDate;

            DbContext.SaveChanges();

            return new JsonResult(answer.Adapt<AnswerViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var answer = DbContext.Answers.Where(i => i.Id == id).FirstOrDefault();

            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", id)
                });
            }

            DbContext.Answers.Remove(answer);
            DbContext.SaveChanges();

            return new OkResult();
        }
        #endregion


        // GET: api/answers/all
        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var answers = DbContext.Answers.Where(i => i.QuestionId == questionId).ToArray();

            // output the result in JSON formatl
            return new JsonResult(
                answers.Adapt<AnswerViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }
    }
}
