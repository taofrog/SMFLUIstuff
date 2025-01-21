using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;
using SFML.Window;

namespace UI
{
    internal class TextureButton : Button
    {
        protected Texture texture;
        protected Sprite sprite;

        protected Texture? alttexture;
        protected Sprite? altsprite;
        protected bool usealtsprite = false;

        public TextureButton(string myname, vec2 locked, vec2 offset, vec2 hsize, string texturepath = "assets/notexture.png", string? alttexturepath = null) : base(myname, locked, offset, hsize)
        {
            texture = new Texture(texturepath);
            texture.Smooth = true;
            sprite = new Sprite(texture);
            sprite.Scale = new Vector2f(2 * halfsize.x / texture.Size.X, 2 * halfsize.y / texture.Size.Y);

            if (alttexturepath != null)
            {
                usealtsprite = true;

                alttexture = new Texture(alttexturepath);
                alttexture.Smooth = true;
                altsprite = new Sprite(alttexture);
                altsprite.Scale = new Vector2f(2 * halfsize.x / texture.Size.X, 2 * halfsize.y / texture.Size.Y);
            }
        }

        public override void draw(RenderWindow app)
        {
            Sprite rendersprite = new Sprite(sprite);
            if (held)
            {
                if (altsprite == null)
                {
                    rendersprite.Color = new Color(100, 100, 100, 255);
                }
                else
                {
                    rendersprite = new Sprite(altsprite);
                }
            }

            vec2 pos = topleft(app.GetView());
            rendersprite.Position = new Vector2f(pos.x, pos.y);

            app.Draw(rendersprite);
        }

    }
}
