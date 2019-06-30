using ChessAPI.Models;
using ChessAPI.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChessAPI.Controllers
{
    public class TokenController : ApiController
    {
        private ModelChessDB db = new ModelChessDB();

        [AllowAnonymous]
        public IHttpActionResult GetToken(string username, string password)
        {
            if (CheckUser(username, password))
            {
                string token = (JWTManager.GenerateToken(username));
                var principal = JWTManager.AuthenticateJwtToken(token);
                var i = principal.Result.Identity.Name;
                return Ok((token, i));
            }

            return StatusCode(HttpStatusCode.Unauthorized);
        }

        public bool CheckUser(string username, string password)
        {
            var u = db.Users.Where(o => o.Name == username).ToList();
            if (u.Count == 0)
                return false;
            return UserLogic.VerifyHashedPassword(u[0].Password, password);
        }
    }
}
