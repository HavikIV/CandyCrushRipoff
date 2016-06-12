using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using CocosSharp;

namespace CandyCrush
{
    public class GameScene : CCScene
    {
        private CCScene gScene;
        private CCLayer backgroundLayer;
        private CCLayer tilesLayer;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            gScene = new CCScene(gameView);
            addBackground();
            addTiles();
        }

        void addBackground()
        {
            backgroundLayer = new CCLayer();
            var background = new CCSprite("background");
            background.AnchorPoint = new CCPoint(0, 0);
            background.IsAntialiased = false;
            background.Scale = 2.5f;
            backgroundLayer.AddChild(background);
            AddChild(backgroundLayer);
        }

        void addTiles()
        {
            tilesLayer = new CCLayer();
            CCSprite tile;
            int width = 63;
            int height = 71;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tile = new CCSprite("tile");
                    tile.Scale = 1.9f;
                    tile.Position = new CCPoint(135 + (width * i), 250 + (height * j));
                    tilesLayer.AddChild(tile);
                }
            }
            AddChild(tilesLayer);
        }
    }
}