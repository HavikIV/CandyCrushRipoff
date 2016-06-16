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
using CandyCrush.Entities;

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
            addCandyLayer();
        }

        void addBackground()
        {
            backgroundLayer = new CCLayer();
            var background = new CCSprite("background");
            background.AnchorPoint = new CCPoint(0, 0);
            background.IsAntialiased = false;
            backgroundLayer.AddChild(background);
            AddChild(backgroundLayer);
        }

        void addTiles()
        {
            tilesLayer = new CCLayer();
            CCSprite tile;
            int width = 62;
            int height = 70;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tile = new CCSprite("tile");
                    tile.Position = new CCPoint(70 + (width * i), 250 + (height * j));
                    tilesLayer.AddChild(tile);
                }
            }
            AddChild(tilesLayer);
        }

        void addCandyLayer()
        {
            AddChild(new candyLayer());
        }
    }
}