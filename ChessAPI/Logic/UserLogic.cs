using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;

namespace ChessAPI.Models
{
    public class UserLogic
    {
        ModelChessDB db;

        public UserLogic()
        {
            db = new ModelChessDB();
        }

        public bool UserIsExist(string name)
        {
            Users u = db.Users.Where(g => g.Name == name).FirstOrDefault();
            if (u == null)
                return false;
            else
                return true;
        }

        public Games GetGame(string name)
        {
            Users u = db.Users.Where(g => g.Name == name).FirstOrDefault();
            if (u == null)
                return null;
            Games userGameWhite = u.Games.Where(g => g.Statuse == "play" || g.Statuse == "wait" || g.Statuse == "offerDraw").FirstOrDefault();
            Games userGameBlack = u.Games1.Where(g => g.Statuse == "play" || g.Statuse == "offerDraw").FirstOrDefault();
            if (userGameBlack != null)
            {
                return userGameBlack;
            }
            if (userGameWhite != null)
            {   
                return userGameWhite;  
            }
            else
            {
                return GetLastDoneGame(name);
            }
        }

        public Games ConnectToGame(string name, string name2)
        {
            Users u1 = db.Users.Where(g => g.Name == name).FirstOrDefault();
            Users u2 = db.Users.Where(g => g.Name == name2).FirstOrDefault();
            Games userGameWhite = u2.Games.Where(g => g.Statuse == "wait").FirstOrDefault();
            var ourGames = u1.Games.Where(g => g.Statuse == "wait" || g.Statuse == "play" || g.Statuse == "offerDraw").ToList();
            var ourGames1 = u1.Games1.Where(g => g.Statuse == "wait" || g.Statuse == "play" || g.Statuse == "offerDraw").ToList();
            if (ourGames.Count != 0 || ourGames1.Count != 0)
                throw new WebException("Ты уже играешь");
            if (userGameWhite != null)
            {
                userGameWhite.Black = u1.id;
                userGameWhite.Statuse = "play";
                db.Entry(userGameWhite).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return userGameWhite;
            }
            return null;
        }

        public Games CreateOnce(string name)
        {
            Users u = db.Users.Where(g => g.Name == name).FirstOrDefault();
            if (u == null)
                return null;
            Games userGameWhite = u.Games.Where(g => g.Statuse == "play" || g.Statuse ==  "wait" || g.Statuse == "offerDraw").FirstOrDefault();
            Games userGameBlack = u.Games1.Where(g => g.Statuse == "play" || g.Statuse == "wait" || g.Statuse == "offerDraw").FirstOrDefault();
            Chess.Chess chess = new Chess.Chess();
            if (userGameBlack != null)
            {
                return userGameBlack;
            }
            if (userGameWhite != null)
            {
                return userGameWhite;
            }
            if (userGameWhite == null)
            {
                Games waitGame = new Games { FEN = chess.fen, Statuse = "wait", White = u.id, ColorMove = "white" };
                db.Games.Add(waitGame);
                db.SaveChanges();
                return waitGame;    
            }
            else
            {
                return null;
            }

        }

        public Games CreateGame(string name)
        {
            Users u = db.Users.Where(g => g.Name == name).FirstOrDefault();
            if (u == null)
                return null;
            Games userGameWhite = u.Games.Where(g => g.Statuse == "play" ).FirstOrDefault();
            Games userGameBlack = u.Games1.Where(g => g.Statuse == "play" || g.Statuse == "offerDraw").FirstOrDefault();
            Chess.Chess chess = new Chess.Chess();

            if (userGameBlack != null)
            {
                return null;
            }
            if (userGameWhite == null)
            {
                Games waitGame = db.Games.Where(g => g.Statuse == "wait").FirstOrDefault();

                if (waitGame == null)
                {
                    waitGame = new Games { FEN = chess.fen, Statuse = "wait", White = u.id, ColorMove = "white" };
                    db.Games.Add(waitGame);
                    db.SaveChanges();
                    return waitGame;
                }
                else
                {
                    if (waitGame.Users.Name == name)
                        return waitGame;

                    waitGame.Black = u.id;
                    waitGame.Statuse = "play";
                    db.Entry(waitGame).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return waitGame;
                }
            }
            else
            {
                return null;
            }

        }

        public Games GetLastDoneGame(string name)
        {
            Users u = db.Users.Where(g => g.Name == name).FirstOrDefault();
            Games userGameWhite = u.Games.Where(g => g.Statuse == "done" || g.Statuse == "draw").LastOrDefault();
            Games userGameBlack = u.Games1.Where(g => g.Statuse == "done" || g.Statuse == "draw").LastOrDefault();
            if (userGameWhite == null)
            {
                return userGameBlack;
            }
            else if((userGameBlack == null))
            {
                return userGameWhite;
            }
            if(userGameWhite.id > userGameBlack.id)
            {
                return userGameWhite;
            }
            else
            {
                return userGameBlack;
            }
        }

        public string UserColor(string name)
        {
            Users u = db.Users.Where(g => g.Name == name).FirstOrDefault();

            Games userGameWhite = u.Games.Where(g => g.Statuse == "play").FirstOrDefault();
            Games userGameBlack = u.Games1.Where(g => g.Statuse == "play").FirstOrDefault();

            if (userGameWhite == null && userGameBlack != null)
                return "black";
            if (userGameWhite != null && userGameBlack == null)
                return "white";
            return "none";
        }

        public string FlipColor(string color)
        {
            if (color == "white")
                return "black";
            return "white";
        }

        public Games MakeMove(string name, string move)
        {
            Games game = GetGame(name);

            Users curUser = game.Users.Name == name ? game.Users : game.Users1; ;
            Users opositeUser = game.Users.Name == name ? game.Users1 : game.Users;

            string userColor = UserColor(name);

            Chess.Chess chess = new Chess.Chess(game.FEN);

            string[] parts = game.FEN.Split();
            string curColor = parts[1];

            if (curColor[0] != userColor[0])
                return game;

            var chessNext = chess.Move(move);
            if (chessNext.fen == game.FEN)
                return game;

            game.FEN = chessNext.fen;
            game.ColorMove = FlipColor(userColor);
            if (chessNext.IsCheckAndMate())
            {
                game.Statuse = "done";
                curUser.Rating += 1;
                opositeUser.Rating -= 1;
            }
                

            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return game;
        }

        public Games OfferDraw(string name)
        {
            Games game = GetGame(name);

            Users curUser = game.Users.Name == name ? game.Users : game.Users1; ;
            Users opositeUser = game.Users.Name == name ? game.Users1 : game.Users;

            game.Statuse = "offerDraw";

            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return game;
        }

        public Games AcceptDraw(string name)
        {
            Games game = GetGame(name);

            game.Statuse = "draw";

            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return game;
        }

        public Games DeclineDraw(string name)
        {
            Games game = GetGame(name);

            Users curUser = game.Users.Name == name ? game.Users : game.Users1; ;
            Users opositeUser = game.Users.Name == name ? game.Users1 : game.Users;

            game.Statuse = "play";

            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return game;
        }

        public static string HashPass(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }
    }
}