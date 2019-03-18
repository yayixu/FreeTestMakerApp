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
    public class QuizController : Controller
    {
        private ApplicationDbContext DbContext;

        public QuizController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        #region Restful conventions methods
        /// <summary>
        /// GET: api/quize/{id}
        /// Retrieves the quiz with the given {id}
        /// </summary>
        /// <param name="id">The id of an existing quiz</param>
        /// <returns>The quiz with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quiz = DbContext.Quizzes.Where(i => i.Id == id).FirstOrDefault();

            // output the result in Json format
            return new JsonResult(
                quiz.Adapt<QuizViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        /// <summary>
        /// Adds a new Quiz to the Database
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(QuizViewModel m)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit the Quiz with the given {id}
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(QuizViewModel m)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the Quiz with a given {id} from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Attribute-based routing methods
        /// <summary>
        /// GET: api/quiz/latest
        /// Retrieves the {num} latest quizzes.
        /// </summary>
        /// <param name="num">The num of quizzes to retrieve</param>
        /// <returns>the num latest quizzes</returns>
        [HttpGet("Latest/{num:int?}")]
        public IActionResult Latest(int num = 10)
        {
            var lastest = DbContext.Quizzes.OrderByDescending(q => q.CreatedDate).Take(num).ToArray();

            //output the result in JSON format
            return new JsonResult(
                lastest.Adapt<QuizViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });            
        }

        /// <summary>
        /// GET: api/quiz/ByTitle
        /// Retrieves the {num} Quizes sorted by Ttitle(A to Z)
        /// </summary>
        /// <param name="num">the number of quizes to retrieve</param>
        /// <returns>{num} Quizes sorted by title</returns>
        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            var byTitle = DbContext.Quizzes.OrderBy(q => q.Title).Take(num).ToArray();

            return new JsonResult(
                    byTitle.Adapt<QuizViewModel[]>(),
                    new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented
                    });
        }

        /// <summary>
        /// GET: api/quiz/mostViewed
        /// Retrieves the {num} random Quizes
        /// </summary>
        /// <param name="num">the number of quizes to retrieve</param>
        /// <returns>{num} random Quizes</returns>
        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var random = DbContext.Quizzes.Take(num).ToArray();

            return new JsonResult(
                    random.Adapt<QuizViewModel[]>(),
                    new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented
                    });
        }
        #endregion
    }
}
