using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using NintendoGames.Models;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.Text;
using System.Diagnostics;
using GamesList.Services;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NintendoController : ControllerBase
    {

        private readonly INintendoService _nintendoService;

        public NintendoController(INintendoService nintendoService)
        {
            _nintendoService = nintendoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> GetAllGamesFromWeb()
        {
            var gamesList = await _nintendoService.GetAllGamesFromWeb();

            return Ok(gamesList);
        }
    }





}


