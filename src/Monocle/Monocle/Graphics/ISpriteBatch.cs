using System;
using OpenTK;
using System.Text;
using OpenTK.Graphics;
namespace Monocle.Graphics
{
    public interface ISpriteBatch
    {
        void AddFrame(Frame frame, Rect position, Color4 color, float rotation = 0, bool mirror = false);     
        void AddFrame(Frame frame, Vector2 position, Color4 color);        
        void AddFrame(Frame frame, Vector2 position, Color4 color, Vector2 origin, 
                       Vector2 scale, float rotation = 0.0f, bool mirror = false, float renderLayer = 0);
        
        void AddString(TextureFont font, StringBuilder toDraw, Vector2 position, Color4 color, 
                        Vector2 origin, Vector2 scale, float angle = 0, bool mirror = false, float renderLayer = 0.0f);
        
        void AddString(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color);
        void AddString(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin);
        void AddString(TextureFont fontUsed, string toDraw, Vector2 position, Color4 color, Vector2 origin, Vector2 scale, 
                        float angle = 0, bool mirror = false, float renderLayer = 0.0f);
        void End(ref OpenTK.Matrix4 transformation, Effect effect = null, SortMode mode = SortMode.Deffered);
    }
}
