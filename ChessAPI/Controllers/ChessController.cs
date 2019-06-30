using ChessAPI.Models;
using System.Web.Http;

namespace ChessAPI.Controllers
{
    public class ChessController : ApiController
    {
        private ModelChessDB db = new ModelChessDB();
        private UserLogic userLogic = new UserLogic();

        [AllowAnonymous]
        public Games GetChess(string id)
        {
            return userLogic.GetGame(id);
        }


        [AllowAnonymous]
        public Games GetChess(string id, string move) => userLogic.MakeMove(id, move);
    }
}
