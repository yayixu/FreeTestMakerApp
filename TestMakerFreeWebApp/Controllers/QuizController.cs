using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
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
            //Create a sample quiz to match the given result
            var v = new QuizViewModel()
            {
                Id = id,
                Title = String.Format("Sample quize with id {0}", id),
                Description = "Not a real quiz, it's just a sample",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            // output the result in Json format
            return new JsonResult(
                v,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }
        #endregion

        #region Attribute-based routing methods
        /// <summary>
        /// GET: api/quiz/latest
        /// Retrieves the {num} latest quizzes.
        /// </summary>
        /// <param name="num">The num of quizzes to retrieve</param>
        /// <returns>the num latest quizzes</returns>
        [HttpGet("Latest/{num}")]
        public IActionResult Latest(int num = 10)
        {
            var sampleQuizes = new List<QuizViewModel>();

            // add a first sample quiz
            sampleQuizes.Add(new QuizViewModel()
            {
                Id = 1,
                Title = "Which Shingeki No Kyojin character are you?",
                Description = "Anime-related personality test",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });

            // add a bunch of other sample quizzes
            for (int i = 2; i <= num; i++)
            {
                sampleQuizes.Add(new QuizViewModel()
                {
                    Id = i,
                    Title = String.Format("Sample Quize {0}", i),
                    Description = "This is a sample quize",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }

            //output the result in JSON format
            return new JsonResult(
                sampleQuizes,
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
            var sampleQuizes = ((JsonResult)Latest(num)).Value
                as List<QuizViewModel>;

            return new JsonResult(
                    sampleQuizes.OrderBy(t => t.Title),
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
            var sampleQuizes = ((JsonResult)Latest(num)).Value
                as List<QuizViewModel>;

            return new JsonResult(
                    sampleQuizes.OrderBy(t => Guid.NewGuid()),
                    new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented
                    });
        }
        #endregion
    }
}
