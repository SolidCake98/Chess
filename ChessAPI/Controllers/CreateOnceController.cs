﻿using ChessAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChessAPI.Controllers
{
    public class CreateOnceController : ApiController
    {
        private ModelChessDB db = new ModelChessDB();
        private UserLogic userLogic = new UserLogic();

        [AllowAnonymous]
        public Games GetChess(string id)
        {
            return userLogic.CreateOnce(id);
        }
    }
}