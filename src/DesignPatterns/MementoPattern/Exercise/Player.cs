using System;
using System.Collections.Generic;
using System.Text;

namespace MementoPattern.Exercise
{
    public class Player
    {
        public int Level { get; private set; }
        public int Score { get; private set; }
        public float Health { get; private set; }

        private int lifeline = 3;

        public Player()
        {
            Level = 3;
            Score = 0;
            Health = 1;
        }

        public void UpLevel() => Level++;

        internal void Hit()
        {
            Health -= 0.1f;
        }

        public void AddPoints(int points)
        {
            Score += points;
        }

        public override string ToString()
        {
            return $"Level: {Level} Score: {Score} Healt: {Health:P0} Left: {lifeline}";
        }
    }
}
