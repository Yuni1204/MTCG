﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.json
{
    public class UsersTable
    {
        public string Username;
        public string Password;
        public string Bio;
        public string Image;
        public int Coins;
        public int Elo;
        public int GamesPlayed;
        public int Wins;
        public int Losses;
        public int Draws;


        //public string username
        //{
        //    get { return username; }
        //    set
        //    {

        //    }
        //}

        public UsersTable()
        {
            Username = null;
            Password = null;
            Bio = null;
            Image = null;
            Coins = 20;
            Elo = 100;
            GamesPlayed = 0;
            Wins = 0;
            Losses = 0;
            Draws = 0;
        }

        public void updateUserData(/*dictionary*/)
        {

        }
    }
}
