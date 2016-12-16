using System;
using System.Collections.Generic;

namespace pax_infinium
{
    public class ContentManager<T>
    {
        private Microsoft.Xna.Framework.Content.ContentManager Content;
        private Dictionary<string, T> textures;

        public T this[string key]
        {
            get
            {
                if (textures.ContainsKey(key))
                {
                    return textures[key];
                }
                else
                {
                    throw new Exception("That texture hasn't been loaded");
                }
            }
        }

        public ContentManager(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            this.Content = Content;
            textures = new Dictionary<string, T>();
        }

        public void Load(string textureName)
        {
            textures.Add(textureName, Content.Load<T>(textureName));
        }
    }
}
