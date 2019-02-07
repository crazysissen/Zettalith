using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zettalith.Pieces
{
    [Serializable]
    abstract class SubPiece
    {
        public bool Unlocked => Subpieces.Unlocked[ToIndex()];

        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackDamage { get; set; }
        public Mana ManaCost { get; set; }
        public Mana MoveCost { get; set; }
        public Ability Ability { get; set; }
        public string Description { get; set; }
        public Texture2D Texture { get; set; }

        // Creates an index for this subpiece to be saved as
        public int ToIndex()
        {
            return Subpieces.subpieces.IndexOf(GetType());
        }

        // Unlocks this subpiece to the player
        public void Unlock()
        {
            Subpieces.Unlocked[ToIndex()] = true;
        }

        //Locks this subpiece to the player
        public void Lock()
        {
            Subpieces.Unlocked[ToIndex()] = false;
        }
    }
}