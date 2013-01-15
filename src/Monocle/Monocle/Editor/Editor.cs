using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content;
using Monocle.Core;
using Monocle.GUI;

namespace Monocle.Editor
{
    class Editor
    {
        public IResourceManager Resource
        {
            get;
            private set;
        }

        public IEntityCollection Entities
        {
            get;
            private set;
        }

        public ISystemManager Systems
        {
            get;
            private set;
        }

        public IGUIManager GUI
        {
            get;
            private set;
        }

        public IUndoRedo UndoRedo
        {
            get;
            private set;
        }

        public IDragAndDrop DragAndDrop
        {
            get;
            private set;
        }

        public Editor(IResourceManager resource, IEntityCollection entityCollection, ISystemManager systems,
                      IGUIManager guis, IUndoRedo undoRedo, IDragAndDrop dragAndDrop)
        {
            this.Resource = resource;
            this.Entities = entityCollection;
            this.Systems = systems;
            this.GUI = guis;
            this.UndoRedo = undoRedo;
        }
    }
}