using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : BaseApiController
    {
        public QuizController(ApplicationDbContext context) : base(context) { }

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
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", id)
                });
            }

            // output the result in Json format
            return new JsonResult(
                quiz.Adapt<QuizViewModel>(), JsonSetting);
        }

        /// <summary>
        /// Adds a new Quiz to the Database
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody]QuizViewModel model)
        {
            //return a generic HTTP Status 500(Server Error)
            // if the client payload is invalid
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            // handle the insert (without object-mapping)
            var quiz = new Quiz();

            //properties taken from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            //properties set from server-side
            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = model.CreatedDate;

            //set a temporaty author using the Admin user's userId
            // as user login isn't supported yet: we'll change the code later
            quiz.UserId = DbContext.Users.Where(u => u.UserName == "Admin").FirstOrDefault().Id;

            // add the new quiz
            DbContext.Quizzes.Add(quiz);
            //persist the changes into the Database
            DbContext.SaveChanges();

            //return the newly-created quiz to the client
            return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSetting);
        }

        /// <summary>
        /// Edit the Quiz with the given {id}
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]QuizViewModel model)
        {
            //return a generic HTTP Status 500(Server Error)
            // if the client payload is invalid
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            //retrieve the quiz to edit
            var quiz = DbContext.Quizzes.Where(q => q.Id == model.Id).FirstOrDefault();

            //handle requests asking for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz Id {0} has not been found", model.Id)
                });
            }

            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            quiz.LastModifiedDate = quiz.CreatedDate;

            DbContext.SaveChanges();
            // return the updated quiz to the client
            return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSetting);
        }

        /// <summary>
        /// Deletes the Quiz with a given {id} from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //retrieve the quiz to delete
            var quiz = DbContext.Quizzes.Where(d => d.Id == id).FirstOrDefault();

            //handle requests asking for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz Id {0} has not been found", id)
                });
            }

            DbContext.Quizzes.Remove(quiz);
            DbContext.SaveChanges();

            return new OkResult();
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
                lastest.Adapt<QuizViewModel[]>(), JsonSetting);            
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
                    byTitle.Adapt<QuizViewModel[]>(), JsonSetting);
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
            var random = DbContext.Quizzes.OrderBy(q=> Guid.NewGuid()).Take(num).ToArray();

            return new JsonResult(
                    random.Adapt<QuizViewModel[]>(), JsonSetting);
        }
        #endregion
    }
}
