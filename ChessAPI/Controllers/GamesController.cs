using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ChessAPI.Filters;
using ChessAPI.Models;
using ChessAPI.Token;

namespace ChessAPI.Controllers
{
    public class GamesController : ApiController
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: api/Games
        [JwtAuthentication]
        public IHttpActionResult GetGames()
        {
            HttpRequestMessage re = Request;
            var headers = re.Headers;

            if (headers.Contains("Authorization"))
            {
                string token = headers.GetValues("Authorization").First();
                var name = JWTManager.AuthenticateJwtToken(token.Split()[1]).Result.Identity.Name;
                return Ok(name);
            }
            return NotFound();
            
        }

        // GET: api/Games/5
        [AllowAnonymous]
        [ResponseType(typeof(Games))]
        public IHttpActionResult GetGames(int id)
        {
            Users games = db.Users.Find(id);
            if (games == null)
            {
                return NotFound();
            }

            return Ok(games);
        }

        //// PUT: api/Games/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutGames(int id, Games games)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != games.id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(games).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GamesExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Games
        //[ResponseType(typeof(Games))]
        //public IHttpActionResult PostGames(Games games)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Games.Add(games);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = games.id }, games);
        //}

        //// DELETE: api/Games/5
        //[ResponseType(typeof(Games))]
        //public IHttpActionResult DeleteGames(int id)
        //{
        //    Games games = db.Games.Find(id);
        //    if (games == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Games.Remove(games);
        //    db.SaveChanges();

        //    return Ok(games);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GamesExists(int id)
        {
            return db.Games.Count(e => e.id == id) > 0;
        }
    }
}