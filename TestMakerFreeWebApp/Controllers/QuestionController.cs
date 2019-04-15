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
    public class QuestionController : Controller
    {
        private ApplicationDbContext DbContext;

        public QuestionController(ApplicationDbContext context)
        {
            DbContext = context;
        }


        #region RESTful conventions methods
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = DbContext.Questions.Where(i => i.Id == id).FirstOrDefault();

            // handle requests asking for non-existing questions
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", id)
                });
            }

            return new JsonResult(
                question.Adapt<QuestionViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        [HttpPut]
        public IActionResult Put ([FromBody]QuestionViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            var question = model.Adapt<Question>();

            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;
            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;

            DbContext.Questions.Add(question);
            DbContext.SaveChanges();

            return new JsonResult(question.Adapt<QuestionViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        [HttpPost]
        public IActionResult Post([FromBody]QuestionViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            var question = DbContext.Questions.Where(i => i.Id == model.Id).FirstOrDefault();

            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", model.Id)
                });
            }

            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;
            question.LastModifiedDate = question.CreatedDate;

            DbContext.SaveChanges();

            return new JsonResult(question.Adapt<QuestionViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var question = DbContext.Questions.Where(i => i.Id == id).FirstOrDefault();

            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", id)
                });
            }

            DbContext.Questions.Remove(question);
            DbContext.SaveChanges();

            return new OkResult();
        }
        #endregion

        // GET: api/question/all
        [HttpGet("All/{quizeId}")]
        public IActionResult All(int quizeId)
        {
            var questions = DbContext.Questions.Where(q => q.QuizId == quizeId).ToArray();

            // output the result in JSON format
            return new JsonResult(
                questions.Adapt<QuestionViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }
    }
}
