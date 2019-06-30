using ChessAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChessAPI.Controllers
{
    public class ConnectToGameController : ApiController
    {

        private ModelChessDB db = new ModelChessDB();

        [AllowAnonymous]
        public List<Games> GetConnectToGame()
        {
            
            return db.Games.Where(o=> o.Statuse == "wait").ToList();
        }

        [AllowAnonymous]
        public IHttpActionResult GetConnectToGame(string id, string move)
        {
            UserLogic userLogic = new UserLogic();
            try
            {
                var g = userLogic.ConnectToGame(id, move);
                return Ok(g);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
