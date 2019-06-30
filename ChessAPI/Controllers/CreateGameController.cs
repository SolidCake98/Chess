using ChessAPI.Models;
using System.Web.Http;

namespace ChessAPI.Controllers
{
    public class CreateGameController : ApiController
    {
        private ModelChessDB db = new ModelChessDB();
        private UserLogic userLogic = new UserLogic();

        [AllowAnonymous]
        public Games GetChess(string id)
        {
            return userLogic.CreateGame(id);
        }
    }
}
