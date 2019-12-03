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
    }
}