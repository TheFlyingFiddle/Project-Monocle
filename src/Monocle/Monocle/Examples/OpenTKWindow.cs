﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;
using Monocle.EntityGUI;
using Monocle.Content;
using Monocle.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Monocle.Content.Serialization;
using System.IO;

namespace Monocle
{
    abstract class OpenTKWindow
    {
        private readonly GameWindow window;

        public int Width
        {
            get { return this.window.Width; }
            set { this.window.Width = value; }
        }

        public int Height
        {
            get { return this.window.Height; }
            set { this.window.Height = value; }
        }

        public Panel Panel
        {
            get { return this.panel; }
        }

        public event Action Resize;

        protected IGraphicsContext GraphicsContext;
        protected ResourceContext Resourses;
        private IGUIRenderer guiRenderer;
      
        private readonly GUIEventSystem eventSystem;
        private readonly Panel panel;
        private readonly int fps;
        private readonly string resourceFolder;
        private Time Time;

        public OpenTKWindow(int width, int height, int fps, string title, string resourceFolder)
        {
            window = new GameWindow(width, height, new OpenTK.Graphics.GraphicsMode(),
                                    title, 0, DisplayDevice.Default, 3, 3, 
                                    OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible 
                                    | OpenTK.Graphics.GraphicsContextFlags.Debug);
            this.fps = fps;
            this.resourceFolder = resourceFolder;

            window.Resize += new EventHandler<EventArgs>(window_Resize);
            window.Load += new EventHandler<EventArgs>(window_Load);
            window.UpdateFrame += new EventHandler<FrameEventArgs>(window_UpdateFrame);
            window.RenderFrame += new EventHandler<FrameEventArgs>(window_RenderFrame);

            this.panel = new Panel(null);
            this.panel.Focused = true;
            eventSystem = new GUIEventSystem(this.panel, window.Mouse, window.Keyboard, window);
        }

        void window_RenderFrame(object sender, FrameEventArgs e)
        {
            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, -1, 10);
            this.GraphicsContext.Viewport = new Rect(0, 0, this.Width, this.Height);
            this.GraphicsContext.Scissor = this.GraphicsContext.Viewport;
            GraphicsContext.Clear(Color.MidnightBlue, ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            this.guiRenderer.Draw(this.panel, ref projectionMatrix);           

            this.Draw(this.Time);

            this.window.SwapBuffers();
        }

        void window_UpdateFrame(object sender, FrameEventArgs e)
        {
            TimeSpan elapsed = TimeSpan.FromSeconds(e.Time);
            this.Time = new Time(this.Time.Total + elapsed, elapsed);

            this.panel.Update(this.Time);
            this.Update(this.Time);
        }

        void window_Load(object sender, EventArgs e)
        {
            this.GraphicsContext = new DebugGraphicsContext(new OpenGLGraphicsContext());
            var locator = new ServiceLocator();
            locator.RegisterService<IGraphicsContext>(this.GraphicsContext);

            var resourceLoader = new ResourceLoader();
            resourceLoader.AddImportersAndProcessors(typeof(ResourceLoader).Assembly);

            this.Resourses = new ResourceContext(locator, new TypeReaderFactory(), new TypeWriterFactory(), resourceLoader,
                                                        Path.Combine(this.resourceFolder, "Assets"), this.resourceFolder);
            this.Resourses.ShouldSaveAllLoadedAssets = false;

            var pixel = this.Resourses.LoadAsset<Texture2D>("pixel.png");
            var effect = this.Resourses.LoadAsset<ShaderProgram>("Basic.effect");
            this.guiRenderer = new GUIRenderer(new SpriteBuffer(this.GraphicsContext, effect), new Frame(pixel.Bounds, pixel));


            window.VSync = VSyncMode.Adaptive;
            window.Keyboard.KeyRepeat = true;
                  
            // Other state
            GraphicsContext.Enable(EnableCap.Blend);
            GraphicsContext.Enable(EnableCap.ScissorTest);
            GraphicsContext.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);

            var factory = CreateGUIFactory();
            this.Load(factory);
        }

        private GUIFactory CreateGUIFactory()
        {
            var defaultFont = this.Resourses.LoadAsset<Font>("Fonts\\Metro.fnt");
            
            var factory = new GUIFactory();
            factory.RegisterStandardObject<Label>(new Label(defaultFont, ""));

            return factory;
        }

        void window_Resize(object sender, EventArgs e)
        {
            this.panel.Size = new Vector2(window.Width, window.Height);
            this.GraphicsContext.Viewport = new Rect(0, 0, window.Width, window.Height);
            
            if (this.Resize != null)
            {
                this.Resize();
            }
        }
        
        /// <summary>
        /// Runs the window.
        /// </summary>
        public void Run()
        {
            using (window)
            {
                window.Run(this.fps);
            }
        }

        /// <summary>
        /// Called once upon load.
        /// </summary>
        protected abstract void Load(GUIFactory factory);

        /// <summary>
        /// Called every frame, update logic goes here.
        /// </summary>
        /// <param name="time">Time</param>
        protected virtual void Update(Time time) { }

        /// <summary>
        /// Called every frame, render code goes here.
        /// </summary>
        /// <param name="time">Time</param>
        protected virtual void Draw(Time time) { }
    }
}
