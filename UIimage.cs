using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;

namespace UI
{
    internal class UIimage : UIelement
    {
        protected Texture texture;
        protected Sprite sprite;

        public UIimage(string myname, vec2 locked, vec2 offset, vec2 hsize, string texturepath = "assets/notexture.png") : base(myname, locked, offset, hsize)
        {
            texture = new Texture(texturepath);
            texture.Smooth = true;
            sprite = new Sprite(texture);
            sprite.Scale = new Vector2f(2 * halfsize.x / texture.Size.X, 2 * halfsize.y / texture.Size.Y);
        }
        public override void draw(RenderWindow app)
        {
            vec2 pos = topleft(app.GetView());
            sprite.Position = new Vector2f(pos.x, pos.y);

            app.Draw(sprite);
        }
    }
}
