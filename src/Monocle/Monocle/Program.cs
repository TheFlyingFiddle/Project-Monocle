using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monocle.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using Monocle.Graphics;
using System.IO;
using System.Runtime.InteropServices;
using Monocle.Content.Serialization;
using Monocle.GUI;
using Monocle.Content;

using GContext = Monocle.Graphics.IGraphicsContext;
using Monocle.EntityGUI;
using Monocle.Core;

namespace Monocle
{
    class HelloGL3 : GameWindow
    {
        Texture2D texture;
        Texture2D texture1;
        SpriteBatch fontBatch;
        TextureFont font;
        TextureFont font1;
        TextureFont font2;
        EditableText text;
        StringBuilder builder = new StringBuilder(4096);
        GContext context;
        IServiceLocator locator;

        GFSMComponent gfsmComponent;


        public HelloGL3()
            : base(1280, 720,
            new GraphicsMode(), "OpenGL 3 Example", 0,
            DisplayDevice.Default, 3, 0,
            GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug)
        { }
        
        protected override void OnLoad (System.EventArgs e)
        {
            VSync = VSyncMode.On;

            locator = new ServiceLocator();
            this.context = new DebugGraphicsContext(new OpenGLGraphicsContext());
            locator.RegisterService<GContext>(this.context);

            var loader = new ResourceLoader();
            loader.AddImportersAndProcessors(this.GetType().Assembly);



            ResourceContext resourceContext = new ResourceContext(this.locator, new TypeReaderFactory(), new TypeWriterFactory(), loader, Environment.CurrentDirectory + "\\Assets", Environment.CurrentDirectory);
            resourceContext.ShouldSaveAllLoadedAssets = false;
            
            this.texture = resourceContext.LoadAsset<Texture2D>("overButton.png");
            this.texture1 = resourceContext.LoadAsset<Texture2D>("pressedButton.png");
            
            this.font = resourceContext.LoadAsset<TextureFont>("Arial_22.fnt");
            this.font1 = resourceContext.LoadAsset<TextureFont>("Consolas.fnt");
            this.font2 = resourceContext.LoadAsset<TextureFont>("Arial_No_Rly.fnt"); 
                        
            ShaderProgram effect = resourceContext.LoadAsset<ShaderProgram>("Basic.effect");
            this.fontBatch = new SpriteBatch(context, effect);
            text = new EditableText(-1, true);

            this.gfsmComponent = CreateButton();

            var entity = new Entity(new VariableCollection());

            entity.AddVar<Rect>("Bounds", new Rect(100, 100, 200, 200));
            entity.AddVar<string>("Text", "Enter");
            entity.AddVar<Frame>("BG", new Frame(this.texture.Bounds, this.texture));
            entity.AddVar<TextureFont>("Font", this.font);

            entity.AddComponent(this.gfsmComponent);

            // Other state
            context.Enable(EnableCap.Blend);
            context.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            context.ClearColor(Color4.MidnightBlue);

            this.Keyboard.KeyDown += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyDown);
        }

        private GFSMComponent CreateButton()
        {
            var idleFrameAction = new GFSMFrameAction("BG", "Bounds");
            var idleTextAction = new GFSMTextAction("Font", "Bounds", "Text");
            var idleState = new GFSMState(new int[0], new GFSMAction[] { idleFrameAction, idleTextAction });

            return new GFSMComponent(new GFSMState[] { idleState });
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[OpenTK.Input.Key.Escape])
                Exit();
        }

        int cursorPos;
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            char c = e.KeyChar;
            cursorPos = text.ProcessChar(c, cursorPos);
        }

        void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Delete)
                text.ProcessChar('\u007F', cursorPos);
            else if (e.Key == OpenTK.Input.Key.Left)
                cursorPos = (cursorPos == 0) ? cursorPos : (cursorPos - 1);
            else if (e.Key == OpenTK.Input.Key.Right)
                cursorPos = (cursorPos == text.Length) ? cursorPos : cursorPos + 1;
            else if (e.Key == OpenTK.Input.Key.Up)
                cursorPos = this.LastNewLineIndex();
            else if (e.Key == OpenTK.Input.Key.Down)
                cursorPos = this.NextNewLineIndex();
        }

        private int LastNewLineIndex()
        {
            for (int i = cursorPos - 1; i >= 1; i--)
            {
                if (this.text.ToString()[i] == '\n')
                    return i;
            }
            return cursorPos;
        }

        private int NextNewLineIndex()
        {
            for (int i = cursorPos + 1; i < text.Length; i++)
            {
                if (this.text.ToString()[i] == '\n')
                    return i;
            }

            return cursorPos;
        }

        float rot = 0;
        protected override void OnRenderFrame(FrameEventArgs e)
        {
   /*      

            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, -1, 10);

            rot += (float)e.Time;
            

            Vector2 origin = font.MessureString("The brown dog jumped over the lazy fox!?");
            fontBatch.AddString(font, "The brown dog jumped over the lazy fox!?", Vector2.Zero, Color4.White, Vector2.Zero, Vector2.One, renderLayer: 1.0f);
            fontBatch.AddString(font, "The brown dog jumped over the lazy fox!?", new Vector2(0, origin.Y), Color4.Black, Vector2.Zero, Vector2.One, renderLayer: 1.0f);

            fontBatch.AddString(font, "This is a \nmulti line text!", new Vector2(0, origin.Y) * 2, Color4.Yellow, Vector2.Zero, Vector2.One, renderLayer: 1.0f);           
            fontBatch.AddString(font, "This is scaled text!.", new Vector2(0, origin.Y) * 4, Color4.Green, Vector2.Zero, new Vector2(2,2), renderLayer: 1.0f);
            
            fontBatch.AddString(font, "This is mirrored text!.", new Vector2(0, origin.Y) * 6, Color4.Red, Vector2.Zero, Vector2.One * 3, 0, true);
            fontBatch.AddString(font, "This is rotated and scaled text!.", new Vector2(15, origin.Y) * 8, Color4.Purple, Vector2.Zero, new Vector2(1.5f,1.4f), 0.5f, false);

            fontBatch.AddString(font1, "This is a consolas text!", new Vector2(400, 100), Color4.DimGray, Vector2.Zero);
          //  fontBatch.AddString(font2, "This is a small arial text.", new Vector2(700, 300), Color4.Gold, Vector2.Zero);

            fontBatch.AddString(font1, text.ToString(), new Vector2(650, 0), Color4.White);
           // fontBatch.AddString(font2, "This is a small very scaled font text.", new Vector2(300, 400), Color4.Gold, Vector2.Zero, new Vector2(5,5));

        //    string s = this.builder.ToString();

         //   fontBatch.AddString(font2, s,Vector2.Zero, Color4.Black);
            fontBatch.End(ref projectionMatrix);*/

            context.Viewport(0, 0, Width, Height);
            context.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, -1, 10);

            this.gfsmComponent.Draw(Vector2.Zero, this.fontBatch);
            fontBatch.End(ref projectionMatrix);

            SwapBuffers();
        }

        Random r = new Random();

        private Color4 RandomColor()
        {
            return new Color4((byte)r.Next(0, 256), (byte)r.Next(0, 256), (byte)r.Next(0, 256), (byte)255); 
        }

        private Vector2 RandomPos()
        {
            return new Vector2(r.Next(0, this.Width), r.Next(0, this.Height));
        }

        public static void Main(string[] args)
        {
            using (HelloGL3 hello = new HelloGL3())
            {
                hello.Run(30);
            }
        }
    }
}