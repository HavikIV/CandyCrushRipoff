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

namespace CandyCrush.Scenes
{
    public class GameScene : CCScene
    {
        private CCScene gScene;
        private CCLayer backgroundLayer;
        private CCLayer tilesLayer;
        private candyLayer cLayer;
        private int swipeFromRow, swipeFromCol; // Keeps track of where the swipe started from
        private CCLabel debugLabel;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            gScene = new CCScene(gameView);
            addBackground();
            addTiles();
            addCandyLayer();
            addDebug();
            swipeFromRow = swipeFromCol = 90;   //initializes the variables to 90 even though this isn't a valid location in the grid
            CreateTouchListener(); 
        }

        private void addDebug()
        {
            debugLabel = new CCLabel("Debug info shows here...", "Arial", 30, CCLabelFormat.SystemFont);
            debugLabel.Color = CCColor3B.Red;
            debugLabel.AnchorPoint = new CCPoint(0, 0);
            debugLabel.Position = new CCPoint(0, 30);
            cLayer.AddChild(debugLabel);
        }

        private void addBackground()
        {
            backgroundLayer = new CCLayer();
            var background = new CCSprite("background");
            background.AnchorPoint = new CCPoint(0, 0);
            background.IsAntialiased = false;
            backgroundLayer.AddChild(background);
            AddChild(backgroundLayer);
        }

        private void addTiles()
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

        private void addCandyLayer()
        {
            cLayer = new candyLayer();
            AddChild(cLayer);
        }

        private void CreateTouchListener()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = HandleTouchesMoved;
            touchListener.OnTouchesBegan = HandleTouchesBegan;
            cLayer.AddEventListener(touchListener);
        }

        private void HandleTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCPoint location = touches[0].Location;

            debugLabel.Text = "The user touched the screen.";
            int row = 90, col = 90;
            if (cLayer.convertToPoint(location, ref row, ref col))
            {
                candy candy = cLayer.candyAt(row, col);
                if (candy != null)
                {
                    swipeFromRow = candy.getRow();
                    swipeFromCol = candy.getColumn();
                }
            }
        }

        private void HandleTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (swipeFromCol > 8)
            {
                return;
            }

            debugLabel.Text = "The user moved from the initial touch point.";
            // we only care about the first touch:
            var locationOnScreen = touches[0].Location;

            int column, row;
            row = column = 90;
            if (cLayer.convertToPoint(locationOnScreen, ref row, ref column))
            {

                int horzDelta = 0, vertDelta = 0;
                if (column < swipeFromCol)
                {          // swipe left
                    horzDelta = -1;
                }
                else if (column > swipeFromCol)
                {   // swipe right
                    horzDelta = 1;
                }
                else if (row < swipeFromRow)
                {         // swipe down
                    vertDelta = -1;
                }
                else if (row > swipeFromRow)
                {         // swipe up
                    vertDelta = 1;
                }

                if (horzDelta != 0 || vertDelta != 0)
                {
                    debugLabel.Text = "Checking to see if the swap is valid.";
                    cLayer.trySwap(horzDelta, vertDelta, swipeFromRow, swipeFromCol);
                    swipeFromCol = 90;
                }
            }
        }

        private void touchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            swipeFromRow = 90;
            swipeFromCol = 90;
        }

        private void touchesCancelled(List<CCTouch> touches, CCEvent touchEvent)
        {
            touchesEnded(touches, touchEvent);
        }
    }
}