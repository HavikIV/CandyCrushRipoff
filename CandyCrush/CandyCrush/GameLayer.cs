using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace CandyCrush
{
    public class GameLayer : CCLayerColor
    {
        CCLayer backgroundLayer;
        CCSprite background;

        public GameLayer() : base(CCColor4B.Black)
        {
            backgroundLayer = new CCLayer();
            background = new CCSprite("background");
            background.AnchorPoint = new CCPoint(0, 0);
            background.IsAntialiased = false;
            background.Scale = 2.5f;
            backgroundLayer.AddChild(background);
            AddChild(backgroundLayer);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
            //label.Position = bounds.Center;
            //background.Position = VisibleBoundsWorldspace.Center;

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }
    }
}

