using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;
using Monocle.Game;

namespace Monocle.Core
{
    public interface IEntity : ICloneable, IMonocleCollection
    {
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent of the entity.  
        /// </summary>
        /// <remarks>If this value is null the entity is a root entity.</remarks>
        IEntity Parent { get; set; }

        /// <summary>
        /// Gets an enumerable of all the children of this entity.
        /// </summary>
        IEnumerable<IEntity> Children { get; }

        /// <summary>
        /// Checks if the entity has a specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>true if the entity has the tag otherwise false</returns>
        bool HasTag(string tag);

        /// <summary>
        /// Adds a tag to the entity.
        /// </summary>
        /// <param name="tag">The tag to add.</param>
        void AddTag(string tag);

        /// <summary>
        /// Removes a tag from the entity.
        /// </summary>
        /// <param name="tag">The tag to remove.</param>
        /// <returns>true if a tag was removed.</returns>
        bool RemoveTag(string tag);

        /// <summary>
        /// Gets the first component of type T in the entity.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if a component of type T does not exist.</exception>
        /// <typeparam name="T">The type of the component to retrieve.</typeparam>
        /// <returns>A component of type T.</returns>
        T GetComponent<T>();

        /// <summary>
        /// Gets all the components of type T in the entity.
        /// </summary>
        /// <typeparam name="T">The type of the components to retrieve.</typeparam>
        /// <returns>All the components of type T.</returns>
        IEnumerable<T> GetComponents<T>();

        /// <summary>
        /// Tries to get a component from the entity.
        /// </summary>
        /// <typeparam name="T">The type of the component</typeparam>
        /// <param name="component">The component or null if retrieval was unsuccessful.</param>
        /// <returns>Failure of success.</returns>
        bool TryGetComponent<T>(out T component);

        /// <summary>
        /// Removes a component from the entity.
        /// </summary>
        /// <param name="component">The component to remove.</param>
        void RemoveComponent(Component component);

        /// <summary>
        /// Creates a component and adds it to the entity.
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <returns>The component created.</returns>
        T AddComponent<T>() where T : Component, new();

        /// <summary>
        /// Gets a variable from the entity.
        /// </summary>
        /// <exception cref="InvalidCastException">Thrown if the variable is not of type T.</exception>
        /// <exception cref="ArgumentException">Thrown if the variable does not exist.</exception>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="name">The name of the variable.</param>
        /// <returns>A variable.</returns>
        Variable<T> GetVar<T>(string name);

        /// <summary>
        /// Tries to get a variable of the entity.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="name">The name of the variable.</param>
        /// <param name="variable">The variable store.</param>
        /// <returns>Failure or success.</returns>
        bool TryGetVar<T>(string name, out Variable<T> variable);

        /// <summary>
        /// Checks if the entity has a specific variable.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="name">The name of the variable.</param>
        /// <returns>Present or not present.</returns>
        bool HasVar(string name);
        
        /// <summary>
        /// Adds a variable with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the new variable.</param>
        /// <param name="value">The value of the new variable.</param>
        /// <typeparam name="T">The type of the new variable.</typeparam>
        Variable<T> AddVar<T>(string name, T value);

        /// <summary>
        /// Removes the variable with the specified name.
        /// </summary>
        /// <param name="name">Name of the variable to remove.</param>
        /// <returns>True if there was a variable with the specified name.</returns>
        bool RemoveVar(string name);

        /// <summary>
        /// Sends a message to this entity.
        /// </summary>
        /// <see cref="MessageSender"/>
        /// <param name="messageName">Name of the message.</param>
        /// <param name="param">Parameter of the message.</param>
        void SendMessage(string messageName, object param = null, MessageOptions options = MessageOptions.DontRequireReceiver);

        /// <summary>
        /// Sends a message to this entity and all of its ancestors.
        /// </summary>
        /// <see cref="MessageSender"/>
        /// <param name="messageName">Name of the message.</param>
        /// <param name="param">Parameter of the message.</param>
        void SendMessageUpwards(string messageName, object param = null, MessageOptions options = MessageOptions.DontRequireReceiver);

        /// <summary>
        /// Sends a message to this entity and all of its descendants.
        /// </summary>
        /// <see cref="MessageSender"/>
        /// <param name="messageName">Name of the message.</param>
        /// <param name="param">Parameter of the message.</param>
        void SendMessageDownwards(string messageName, object param = null, MessageOptions options = MessageOptions.DontRequireReceiver);


        /// <summary>
        /// Destroys this entity during the next destruction cycle.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Invoked When the entity is destroyed.
        /// </summary>
        event Action<MonocleObject> Destroyed;
    }
}