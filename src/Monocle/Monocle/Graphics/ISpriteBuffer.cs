using System;
using OpenTK;
using System.Text;
using OpenTK.Graphics;
namespace Monocle.Graphics
{
    public interface ISpriteBuffer
    {
        void BufferFrame(Frame frame, Rect position, Color color, float rotation = 0, bool mirror = false);     
        void BufferFrame(Frame frame, Vector2 position, Color color);        
        void BufferFrame(Frame frame, Vector2 position, Color color, Vector2 origin, 
                       Vector2 scale, float rotation = 0.0f, bool mirror = false, float renderLayer = 0);
        
        void BufferString(Font font, StringBuilder toDraw, Vector2 position, Color color, 
                        Vector2 origin, Vector2 scale, float angle = 0, bool mirror = false, float renderLayer = 0.0f);
        
        void BufferString(Font fontUsed, string toDraw, Vector2 position, Color color);
        void BufferString(Font fontUsed, string toDraw, Vector2 position, Color color, Vector2 origin);
        void BufferString(Font fontUsed, string toDraw, Vector2 position, Color color, Vector2 origin, Vector2 scale, 
                        float angle = 0, bool mirror = false, float renderLayer = 0.0f);
        void BufferSubString(Font font, string toDraw, int startIndex, int length, Vector2 position,
                             Color color, Vector2 origin, Vector2 scale, float angle = 0, bool mirror = false, float renderLayer = 0.0f);


        void Draw(ref OpenTK.Matrix4 transformation, ShaderProgram effect = null, SortMode mode = SortMode.Deffered);

        Monocle.Graphics.IGraphicsContext GraphicsContext { get; }
    }
}
