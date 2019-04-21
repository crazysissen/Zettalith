using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class PieceStats
    {
        const bool USEDISTANCE = true;

        List<PieceStatsInstance> instances;

        public PieceStats()
        {
            instances = new List<PieceStatsInstance>();
        }

        public void Update()
        {
            Grid grid = InGameController.Grid;
            TileObject[] objects = grid.GetList().ToArray();

            Vector2 mousePosition = RendererController.Camera.ScreenToWorldPosition(Input.MousePosition.ToVector2());

            for (int i = 0; i < objects.Length; i++)
            {
                if (instances.Count <= i)
                {
                    instances.Add(new PieceStatsInstance());
                }

                if (objects[i] is TilePiece tilePiece)
                {
                    instances[i].Set(tilePiece, (tilePiece.SupposedPosition - mousePosition).Length(), USEDISTANCE);
                }
            }
        }

        class PieceStatsInstance
        {
            public Renderer.SpriteScreenFloating overlay, healthCrystal, armorCrystal, damageCrystal;
            public Renderer.Text health, armor, damage, healthSh, armorSh, damageSh;

            public PieceStatsInstance()
            {
                overlay = new Renderer.SpriteScreenFloating(new Layer(MainLayer.GUI, -21), Load.Get<Texture2D>("StatsOverlay"), Vector2.Zero, Vector2.One * 0.5f, new Color(255, 255, 255, 210), 0, new Vector2(40, 25), SpriteEffects.None);

                healthCrystal = new Renderer.SpriteScreenFloating(new Layer(MainLayer.GUI, -20), Load.Get<Texture2D>("HealthCrystal"), Vector2.Zero, Vector2.One, Color.White, 0, new Vector2(6.5f, 6.5f), SpriteEffects.None);
                armorCrystal = new Renderer.SpriteScreenFloating(new Layer(MainLayer.GUI, -20), Load.Get<Texture2D>("ArmorCrystal"), Vector2.Zero, Vector2.One, Color.White, 0, new Vector2(6.5f, 6.5f), SpriteEffects.None);
                damageCrystal = new Renderer.SpriteScreenFloating(new Layer(MainLayer.GUI, -20), Load.Get<Texture2D>("DamageCrystal"), Vector2.Zero, Vector2.One, Color.White, 0, new Vector2(6.5f, 6.5f), SpriteEffects.None);

                health = new Renderer.Text(new Layer(MainLayer.GUI, -18), Font.Styled, "H", 1, 0, Vector2.Zero, Color.White);
                armor = new Renderer.Text(new Layer(MainLayer.GUI, -18), Font.Styled, "H", 1, 0, Vector2.Zero, Color.White);
                damage = new Renderer.Text(new Layer(MainLayer.GUI, -18), Font.Styled, "H", 1, 0, Vector2.Zero, Color.White);

                healthSh = new Renderer.Text(new Layer(MainLayer.GUI, -19), Font.Styled, "H", 1, 0, Vector2.Zero, Color.Black);
                armorSh = new Renderer.Text(new Layer(MainLayer.GUI, -19), Font.Styled, "H", 1, 0, Vector2.Zero, Color.Black);
                damageSh = new Renderer.Text(new Layer(MainLayer.GUI, -19), Font.Styled, "H", 1, 0, Vector2.Zero, Color.Black);
            }

            public void Set(TilePiece piece, float distance, bool useDistance)
            {
                Camera camera = RendererController.Camera;

                Stats
                    baseStats = piece.Piece.BaseStats,
                    modifiedStats = piece.Piece.ModifiedStats;

                int currentArmor = 0;
                Vector2 fontScale = FontSize(distance, useDistance), supposedPosition = piece.SupposedPosition;

                // Set texts

                health.String = new StringBuilder(modifiedStats.Health.ToString());
                health.Scale = fontScale;
                health.Origin = health.Font.MeasureString(health.String) * 0.5f;
                damage.String = new StringBuilder(modifiedStats.AttackDamage.ToString());
                damage.Scale = fontScale;
                damage.Origin = damage.Font.MeasureString(damage.String) * 0.5f;
                armor.String = new StringBuilder(currentArmor == 0 ? "" : currentArmor.ToString());
                armor.Scale = fontScale;
                armor.Origin = armor.Font.MeasureString(armor.String) * 0.5f;

                healthSh.String = health.String;
                healthSh.Scale = fontScale;
                healthSh.Origin = health.Origin;
                damageSh.String = damage.String;
                damageSh.Scale = fontScale;
                damageSh.Origin = damage.Origin;
                armorSh.String = armor.String;
                armorSh.Scale = fontScale;
                armorSh.Origin = armor.Origin;

                // Set positions

                Vector2
                    hPos = camera.WorldToScreenPosition(supposedPosition + new Vector2(currentArmor <= 0 ? -0.19f : -0.34f, -1.9f)),
                    aPos = camera.WorldToScreenPosition(supposedPosition + new Vector2(0, -1.9f)),
                    dPos = camera.WorldToScreenPosition(supposedPosition + new Vector2(currentArmor <= 0 ? 0.19f : 0.34f, -1.9f)),
                    crystalSize = camera.WorldToScreenSize(Vector2.One) * 0.8f * (useDistance ? Multiplier(distance, 6, 7) : 1);

                overlay.Position = camera.WorldToScreenPosition(supposedPosition + new Vector2(0, -1.8f));
                overlay.Size = camera.WorldToScreenSize(Vector2.One) * 0.4f;
                overlay.Color = new Color(255, 255, 255, (int)(210 * (useDistance ? Multiplier(distance, 5.8f, 6.8f) : 1)));

                healthCrystal.Position = hPos;
                healthCrystal.Size = crystalSize;
                damageCrystal.Position = dPos;
                damageCrystal.Size = crystalSize;
                armorCrystal.Position = aPos;
                armorCrystal.Size = crystalSize;

                armorCrystal.Active = currentArmor > 0;
                armor.Active = currentArmor > 0;
                armorSh.Active = currentArmor > 0;

                health.Position = hPos;
                damage.Position = dPos;
                armor.Position = aPos;

                Vector2 offset = camera.WorldToScreenSize(new Vector2(0.30f, 0.30f));

                healthSh.Position = hPos + offset;
                damageSh.Position = dPos + offset;
                armorSh.Position = aPos + offset;
            }

            Vector2 FontSize(float distance, bool useDistance)
            {
                return RendererController.Camera.WorldToScreenSize(Vector2.One * 0.15f) * (useDistance ? Multiplier(distance, 6.2f, 7.2f) : 1);
            }

            float Multiplier(float distance, float minDistance, float maxDistance)
            {
                if (distance < minDistance)
                {
                    return 1;
                }

                if (distance < maxDistance)
                {
                    return 1 - MathZ.SineA((distance - minDistance) / (maxDistance - minDistance));
                }

                return 0;
            }
        }
    }
}
