using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessAPI.Models
{
    public class Logic
    {
        private ModelChessDB db;

        public Logic()
        {
            db = new ModelChessDB();
        }

        public Games GetCurrentGame()
        {
            var games = db
                .Games
                .Where(g => g.Statuse == "play")
                .OrderBy(g => g.id).FirstOrDefault();
            if (games == null)
                games = CreateNewGame();
            return games;
        }


        public Games CreateNewGame()
        {
            Chess.Chess chess = new Chess.Chess();
            Games game = new Games { FEN = chess.fen, Statuse = "play" };
            db.Games.Add(game);
            db.SaveChanges();
            return game;
        }

        public Games GetGame(int id) => db.Games.Find(id);

        public Games MakeMove(int id, string move)
        {
            Games game = GetGame(id);
            if(game == null) return game;

            if (game.Statuse != "play")
                return game;

            Chess.Chess chess = new Chess.Chess(game.FEN);

            var chessNext = chess.Move(move);
            if (chessNext.fen == game.FEN)
                return game;

            game.FEN = chessNext.fen;
            if (chessNext.IsCheckAndMate())
                game.Statuse = "done";

            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return game;
        }
    }
}