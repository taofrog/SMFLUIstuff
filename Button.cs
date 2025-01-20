using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;
using SFML.Graphics;
using SFML.System;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;

namespace UI
{
    internal class Button : UIelement
    {
        public bool held;
        public string name;

        protected Texture? alttexture;
        protected Sprite? altsprite;
        protected bool usealtsprite = false;

        public event EventHandler? buttondown;

        public event EventHandler? buttonup;

        public Button(string myname, vec2 locked, vec2 offset, vec2 hsize, string texturepath = "notexture.png", string? alttexturepath = null) : base(locked, offset, hsize, texturepath)
        {
            name = myname;
            held = false;

            if (alttexturepath != null)
            {
                usealtsprite = true;

                alttexture = new Texture(alttexturepath);
                alttexture.Smooth = true;
                altsprite = new Sprite(alttexture);
                altsprite.Scale = new Vector2f(2 * halfsize.x / texture.Size.X, 2 * halfsize.y / texture.Size.Y);
            }
        }

        public void checkpress(vec2 mousepos, RenderWindow app)
        {
            View camera = app.GetView();

            Vector2i mouse = new((int)mousepos.x, (int)mousepos.y);
            Vector2f mouseview = app.MapPixelToCoords(mouse);
            mousepos = new(mouseview.X, mouseview.Y);

            vec2 pos = topleft(camera);
            vec2 pos2 = pos + halfsize * 2;

            if (mousepos.x > pos.x && mousepos.x <= pos2.x &&
                mousepos.y > pos.y && mousepos.y <= pos2.y)
            {
                held = true;
                buttondown?.Invoke(this, new EventArgs());
            }
        }

        public void checkrelease()
        {
            if (held)
            {
                held = false;
                buttonup?.Invoke(this, new EventArgs());
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

        public override string ToString() => name;
    }
}
