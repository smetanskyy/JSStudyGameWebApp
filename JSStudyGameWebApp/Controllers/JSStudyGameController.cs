using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JSStudyGameWebApp.Entities;
using JSStudyGameWebApp.Helpers;
using JSStudyGameWebApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JSStudyGameWebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class JSStudyGameController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _env;

        public JSStudyGameController(AppDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("login")]
        public IActionResult GetPlayerBaseInfo(string emailOrLogin, string password)
        {
            PlayerVM player = new PlayerVM
            {
                Id = 0,
                Email = "none",
                Password = "none",
                Login = "none",
                IsAdmin = false
            };
            Player playerFromDb = null;
            var query = _context.Players.AsQueryable();

            if (password == null)
            {
                return Ok(player);
            }

            if (!string.IsNullOrWhiteSpace(emailOrLogin))
            {
                playerFromDb = emailOrLogin.Contains('@') ? query.SingleOrDefault(p => p.Email == emailOrLogin) : query.SingleOrDefault(p => p.Login == emailOrLogin);
            }

            if (playerFromDb == null)
            {
                return Ok(player);
            }

            if (password == playerFromDb.Password)
            {
                player = new PlayerVM
                {
                    Id = playerFromDb.Id,
                    Email = playerFromDb.Email,
                    Password = playerFromDb.Password,
                    Login = playerFromDb.Login,
                    IsAdmin = playerFromDb.IsAdmin
                };
            }
            return Ok(player);
        }

        [HttpGet("addinfo")]
        public IActionResult GetPlayerAddInfo(string login, string password)
        {
            PlayerAdditionalInfoVM playerAddInfoVM = new PlayerAdditionalInfoVM()
            {
                IdPlayerAdditionalInfo = 0,
                Name = "none",
                Surname = "none",
                Photo = "none",
                BirthDate = DateTime.Now,
                Gender = true
            };

            var playerBasic = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
            if (playerBasic == null)
                return Ok(playerAddInfoVM);

            var playerAddInfo = _context.PlayersAdditionalInfo.SingleOrDefault(p => p.IdPlayerAdditionalInfo == playerBasic.Id);
            if (playerAddInfo == null)
                return Ok(playerAddInfoVM);

            playerAddInfoVM.IdPlayerAdditionalInfo = playerAddInfo.IdPlayerAdditionalInfo;
            playerAddInfoVM.Name = playerAddInfo.Name;
            playerAddInfoVM.Surname = playerAddInfo.Surname;
            playerAddInfoVM.Gender = playerAddInfo.Gender;
            playerAddInfoVM.Photo = playerAddInfo.Photo;
            playerAddInfoVM.BirthDate = playerAddInfo.BirthDate;

            return Ok(playerAddInfoVM);
        }

        [HttpGet("score")]
        public IActionResult GetPlayerScore(string login, string password)
        {
            PlayerScoreVM scoreVM = new PlayerScoreVM()
            {
                IdPlayerScore = 0,
                CorrectAnswers = 0,
                IncorrectAnswers = 0,
                SkippedAnswers = 0,
                TotalScore = 0,
                TimeGameInSeconds = 0,
                ProgressInGame = 0,
                CurrentQuestionNoAnswer = 1,
                AnswersSkipped = "",
                AnswersWrong = ""
            };

            var playerBasic = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
            if (playerBasic == null)
                return Ok(scoreVM);

            var score = _context.Scores.SingleOrDefault(p => p.IdPlayerScore == playerBasic.Id);
            if (score == null)
            {
                scoreVM.IdPlayerScore = playerBasic.Id;
                return Ok(scoreVM);
            }

            scoreVM.IdPlayerScore = score.IdPlayerScore;
            scoreVM.CorrectAnswers = score.CorrectAnswers;
            scoreVM.IncorrectAnswers = score.IncorrectAnswers;
            scoreVM.SkippedAnswers = score.SkippedAnswers;
            scoreVM.TotalScore = score.TotalScore;
            scoreVM.TimeGameInSeconds = score.TimeGameInSeconds;
            scoreVM.ProgressInGame = score.ProgressInGame;
            scoreVM.CurrentQuestionNoAnswer = score.CurrentQuestionNoAnswer;
            scoreVM.AnswersSkipped = score.AnswersSkipped;
            scoreVM.AnswersWrong = score.AnswersWrong;

            return Ok(scoreVM);
        }

        [HttpGet("fullinfo")]
        public IActionResult GetPlayersFullInfo(string login, string password, string page)
        {
            var admin = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
            if (admin == null)
                return Ok(null);
            if (admin.IsAdmin == false)
                return Ok(null);

            int _activePage;
            int numberOfObjectsPerPage = 5;
            if (page == null)
                _activePage = 1;
            else
                _activePage = int.Parse(page);

            var query = _context.Players.AsQueryable();

            query = query
                    .OrderBy(c => c.Id)
                    .Skip(numberOfObjectsPerPage * (_activePage - 1))
                    .Take(numberOfObjectsPerPage);

            var getFullInfo = from p in query
                              join a in _context.PlayersAdditionalInfo on p.Id equals a.IdPlayerAdditionalInfo
                              select new PlayerFullInfo
                              {
                                  Id = p.Id,
                                  Password = p.Password,
                                  Email = p.Email,
                                  Login = p.Login,
                                  IsAdmin = p.IsAdmin,
                                  Name = a.Name,
                                  Surname = a.Surname,
                                  Photo = a.Photo,
                                  BirthDate = a.BirthDate,
                                  Gender = a.Gender == true ? "male" : "female"
                              };

            return Ok(getFullInfo);
        }

        [HttpGet("search")]
        public IActionResult GetPlayerFullInfo(string login, string password, string page, string slogin, string semail, string sname, string ssurname)
        {
            var admin = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
            if (admin == null)
                return Ok(null);
            if (admin.IsAdmin == false)
                return Ok(null);

            int _activePage;
            int numberOfObjectsPerPage = 5;
            if (page == null)
                _activePage = 1;
            else
                _activePage = int.Parse(page);

            var query = _context.Players.AsQueryable();

            query = query
                    .OrderBy(c => c.Id)
                    .Skip(numberOfObjectsPerPage * (_activePage - 1))
                    .Take(numberOfObjectsPerPage);

            var getFullInfo = from p in query
                              join a in _context.PlayersAdditionalInfo on p.Id equals a.IdPlayerAdditionalInfo
                              select new PlayerFullInfo
                              {
                                  Id = p.Id,
                                  Password = p.Password,
                                  Email = p.Email,
                                  Login = p.Login,
                                  IsAdmin = p.IsAdmin,
                                  Name = a.Name,
                                  Surname = a.Surname,
                                  Photo = a.Photo,
                                  BirthDate = a.BirthDate,
                                  Gender = a.Gender == true ? "male" : "female"
                              };

            return Ok(getFullInfo);
        }


        [HttpPost("player")]
        public IActionResult CreatePlayer([FromBody]PlayerVM model)
        {
            if (model == null)
                return Ok(0);

            int id = 0;
            var player = _context.Players.SingleOrDefault(p => p.Login == model.Login);
            if (player != null)
            {
                //This login is already in use!";
                id += -1;
            }

            player = null;
            player = _context.Players.SingleOrDefault(p => p.Email == model.Email);
            if (player != null)
            {
                //This email is already in use!";
                id += -2;
            }

            if (id < 0)
                return Ok(id);

            Player playerToDB;
            try
            {
                playerToDB = new Player()
                {
                    Login = model.Login,
                    Email = model.Email,
                    IsAdmin = model.IsAdmin,
                    Password = model.Password
                };

                _context.Players.Add(playerToDB);
                _context.SaveChanges();

                EmailWorker.SendEmail(model, "Account was successfully registered!");

            }
            catch (Exception) { return Ok(0); }
            return Ok(playerToDB.Id);
        }

        [HttpPost("playeraddinfo")]
        public IActionResult CreatePlayerAdditionalInfo([FromBody]PlayerAdditionalInfoVM model)
        {
            if (model == null)
                return Ok(0);
            PlayerAdditionalInfo player;
            try
            {
                string imageName = "none";
                if (!string.IsNullOrWhiteSpace(model.Photo))
                {
                    string fileDestDir = _env.ContentRootPath;
                    fileDestDir = Path.Combine(fileDestDir, "Photos");
                    imageName = Path.GetRandomFileName() + ".jpg";
                    var bitmap = model.Photo.FromBase64StringToImage();
                    bitmap.Save(Path.Combine(fileDestDir, imageName), ImageFormat.Jpeg);
                }
                player = new PlayerAdditionalInfo()
                {
                    IdPlayerAdditionalInfo = model.IdPlayerAdditionalInfo,
                    Name = model.Name,
                    Surname = model.Surname,
                    Photo = imageName,
                    BirthDate = model.BirthDate,
                    Gender = model.Gender
                };

                _context.PlayersAdditionalInfo.Add(player);
                _context.SaveChanges();
            }
            catch (Exception) { return Ok(0); }

            return Ok(player.IdPlayerAdditionalInfo);
        }

        [HttpPost("score")]
        public IActionResult CreatePlayerScore([FromBody]PlayerScoreVM model)
        {
            if (model == null)
                return Ok(0);

            PlayerScore player;
            try
            {
                player = new PlayerScore()
                {
                    IdPlayerScore = model.IdPlayerScore,
                    CorrectAnswers = model.CorrectAnswers,
                    IncorrectAnswers = model.IncorrectAnswers,
                    SkippedAnswers = model.SkippedAnswers,
                    TotalScore = model.TotalScore,
                    TimeGameInSeconds = model.TimeGameInSeconds,
                    ProgressInGame = model.ProgressInGame,
                    CurrentQuestionNoAnswer = model.CurrentQuestionNoAnswer,
                    AnswersSkipped = model.AnswersSkipped,
                    AnswersWrong = model.AnswersWrong
                };
                _context.Scores.Add(player);
                _context.SaveChanges();
            }
            catch (Exception) { return Ok(0); }

            return Ok(player.IdPlayerScore);
        }

        // change person info
        [HttpPut("player")]
        public IActionResult ChangePlayer([FromBody]PlayerVM model)
        {
            if (model == null || model.Id <= 0)
                return Ok(0);

            int mistake = 0;
            var player = _context.Players.SingleOrDefault(p => p.Id == model.Id);
            if (player == null)
                return Ok(0);
            if (player.Login != model.Login)
            {
                var somePlayer = _context.Players.SingleOrDefault(p => p.Login == model.Login);
                if (somePlayer != null)
                {
                    //This login is already in use!";
                    mistake += -1;
                }
            }

            if (player.Email != model.Email)
            {
                var somePlayer = _context.Players.SingleOrDefault(p => p.Email == model.Email);
                if (somePlayer != null)
                {
                    //This email is already in use!";
                    mistake += -2;
                }

            }

            if (mistake < 0)
                return Ok(mistake);

            try
            {
                player.Login = model.Login;
                player.Email = model.Email;
                player.IsAdmin = model.IsAdmin;
                player.Password = model.Password;
                _context.SaveChanges();

                EmailWorker.SendEmail(model, "Account was successfully changed!");
            }
            catch (Exception) { return Ok(0); }
            return Ok(player.Id);
        }

        [HttpPut("playeraddinfo")]
        public IActionResult ChangePlayerAdditionalInfo([FromBody]PlayerAdditionalInfoVM model)
        {
            if (model == null || model.IdPlayerAdditionalInfo <= 0)
                return Ok(0);
            PlayerAdditionalInfo player = _context.PlayersAdditionalInfo.SingleOrDefault(p => p.IdPlayerAdditionalInfo == model.IdPlayerAdditionalInfo);
            if (player == null)
                return CreatePlayerAdditionalInfo(model);
            else
            {
                try
                {
                    string imageName;
                    if (!string.IsNullOrWhiteSpace(model.Photo))
                    {
                        string fileDestDir = _env.ContentRootPath;
                        fileDestDir = Path.Combine(fileDestDir, "Photos");

                        imageName = Path.GetRandomFileName() + ".jpg";
                        var bitmap = model.Photo.FromBase64StringToImage();
                        bitmap.Save(Path.Combine(fileDestDir, imageName), ImageFormat.Jpeg);

                        if (player.Photo != "stepanPhoto.jpg" && System.IO.File.Exists(Path.Combine(fileDestDir, player.Photo)))
                        {
                            // If file found, delete it    
                            System.IO.File.Delete(Path.Combine(fileDestDir, player.Photo));
                        }
                    }
                    else
                        imageName = player.Photo;

                    player.Name = model.Name;
                    player.Surname = model.Surname;
                    player.Photo = imageName;
                    player.BirthDate = model.BirthDate;
                    player.Gender = model.Gender;
                    _context.SaveChanges();
                }
                catch (Exception) { return Ok(0); }

                return Ok(player.IdPlayerAdditionalInfo);
            }
        }

        [HttpPut("score")]
        public IActionResult ChangePlayerScore([FromBody]PlayerScoreVM model)
        {
            if (model == null || model.IdPlayerScore <= 0)
                return Ok(0);
            PlayerScore player = _context.Scores.SingleOrDefault(p => p.IdPlayerScore == model.IdPlayerScore);
            if (player == null)
                return CreatePlayerScore(model);
            else
            {
                try
                {
                    player.IdPlayerScore = model.IdPlayerScore;
                    player.CorrectAnswers = model.CorrectAnswers;
                    player.IncorrectAnswers = model.IncorrectAnswers;
                    player.SkippedAnswers = model.SkippedAnswers;
                    player.TotalScore = model.TotalScore;
                    player.TimeGameInSeconds = model.TimeGameInSeconds;
                    player.ProgressInGame = model.ProgressInGame;
                    player.CurrentQuestionNoAnswer = model.CurrentQuestionNoAnswer;
                    player.AnswersSkipped = model.AnswersSkipped;
                    player.AnswersWrong = model.AnswersWrong;
                    _context.SaveChanges();
                }
                catch (Exception) { return Ok(0); }

                return Ok(player.IdPlayerScore);
            }
        }

        // delete person info
        [HttpDelete("player")]
        public IActionResult DeletePlayer(string login, string password)
        {
            try
            {
                Player player = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
                if (player == null)
                    throw new Exception();

                PlayerScore score = _context.Scores.SingleOrDefault(p => p.IdPlayerScore == player.Id);
                if (score != null)
                {
                    _context.Scores.Remove(score);
                    _context.SaveChanges();
                }

                PlayerAdditionalInfo playerAddInfo = _context.PlayersAdditionalInfo.SingleOrDefault(p => p.IdPlayerAdditionalInfo == player.Id);
                if (playerAddInfo != null)
                {
                    _context.PlayersAdditionalInfo.Remove(playerAddInfo);
                    _context.SaveChanges();
                }

                _context.Players.Remove(player);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return Ok(false);
            }
            return Ok(true);
        }

        [HttpDelete("playeraddinfo")]
        public IActionResult DeletePlayerAddInfo(string login, string password)
        {
            try
            {
                Player player = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
                if (player == null)
                    throw new Exception();

                PlayerAdditionalInfo playerAddInfo = _context.PlayersAdditionalInfo.SingleOrDefault(p => p.IdPlayerAdditionalInfo == player.Id);
                if (playerAddInfo != null)
                {
                    _context.PlayersAdditionalInfo.Remove(playerAddInfo);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return Ok(false);
            }
            return Ok(true);
        }

        [HttpDelete("score")]
        public IActionResult DeleteScore(string login, string password)
        {
            try
            {
                Player player = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
                if (player == null)
                    throw new Exception();

                PlayerScore score = _context.Scores.SingleOrDefault(p => p.IdPlayerScore == player.Id);
                if (score != null)
                {
                    _context.Scores.Remove(score);
                    _context.SaveChanges();
                }

            }
            catch (Exception)
            {
                return Ok(false);
            }
            return Ok(true);
        }

        [HttpGet("test")]
        public IActionResult GetTest(string login, string password, string id)
        {
            var player = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
            if (player == null)
                return Ok(null);

            var query = _context.Tests.AsQueryable();
            int idOfTest = 1;
            if (id != null)
            {

                try
                {
                    idOfTest = int.Parse(id);
                }
                catch (Exception) { idOfTest = 1; }
            }

            var test = query.SingleOrDefault(t => t.Id == idOfTest);
            TestVM testVM = new TestVM
            {
                Id = test.Id,
                Question = test.Question,
                AnswerA = test.AnswerA,
                AnswerB = test.AnswerB,
                AnswerC = test.AnswerC,
                CorrectAnswer = test.CorrectAnswer,
                IdSection = test.IdSection,
                Reference = test.Reference
            };

            return Ok(testVM);
        }

        [HttpGet("section")]
        public IActionResult GetSections(string login, string password, string id)
        {
            var player = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
            if (player == null)
                return Ok(null);

            int idSection = 1;
            if (id != null)
            {
                try
                {
                    idSection = int.Parse(id);
                }
                catch (Exception) { idSection = 1; }
            }
            var section = _context.Sections.SingleOrDefault(s => s.IdSection == idSection);

            //var sections = query.Select(c => new SectionVM
            //{
            //    IdSection = c.IdSection,
            //    NameOFSection = c.NameOFSection
            //}).ToList();

            return Ok(section);
        }

        [HttpGet("amountoftests")]
        public IActionResult GetAmountOfTest(string login, string password)
        {
            var player = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
            if (player == null)
                return Ok(0);

            var query = _context.Tests.AsQueryable();

            return Ok(query.Count());
        }

        [HttpGet("amountofplayers")]
        public IActionResult GetAmountOfPlayers(string login, string password)
        {
            var player = _context.Players.SingleOrDefault(p => p.Login == login && p.Password == password);
            if (player == null)
                return Ok(0);

            var query = _context.Players.AsQueryable();

            return Ok(query.Count());
        }
        
    }
}