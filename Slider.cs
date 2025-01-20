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
    internal class Slider : UIelement
    {
        public string name;

        public float length;
        public float val;
        public float handledistance;
        public bool held;
        public vec2 handlesize;

        public Texture handletexture;
        public Sprite handlesprite;

        public Slider(string myname, vec2 locked, vec2 offset, vec2 hsize, float initval) : base(locked, offset, hsize)
        {
            name = myname;
            length = hsize.x * 2;
            val = initval;
            handledistance = val * length;
            handlesize = new(hsize.y * 2 + 4);

            handletexture = new Texture("assets/notexture.png");
            handletexture.Smooth = true;
            handlesprite = new Sprite(handletexture);
            handlesprite.Scale = new Vector2f(handlesize.x / texture.Size.X, handlesize.y / texture.Size.Y);
        }

        public void checkpress(vec2 mousepos, RenderWindow app)
        {
            View camera = app.GetView();

            Vector2i mouse = new((int)mousepos.x, (int)mousepos.y);
            Vector2f mouseview = app.MapPixelToCoords(mouse);
            mousepos = new(mouseview.X, mouseview.Y);

            vec2 pos = topleft(camera);
            vec2 pos2 = pos + halfsize * 2;

            vec2 handlecorner = gethandlecorner(camera);
            vec2 handlecorner2 = handlecorner + handlesize;

            if (mousepos.x > pos.x && mousepos.x <= pos2.x &&
                mousepos.y > pos.y && mousepos.y <= pos2.y)
            {
                held = true;
            }
            else if (mousepos.x > handlecorner.x && mousepos.x <= handlecorner2.x &&
                     mousepos.y > handlecorner.y && mousepos.y <= handlecorner2.y)
            {
                held = true;
            }
        }
        public void checkrelease()
        {
            if (held)
            {
                held = false;
            }
        }

        public float update(vec2 mousepos, RenderWindow app)
        {
            if (held)
            {
                View camera = app.GetView();

                Vector2i mouse = new((int)mousepos.x, (int)mousepos.y);
                Vector2f mouseview = app.MapPixelToCoords(mouse);
                mousepos = new(mouseview.X, mouseview.Y);

                handledistance = Math.Clamp(mousepos.x - topleft(camera).x, 0, halfsize.x * 2);
                val = handledistance / (2 * halfsize.x);
            }

            return val;
        }

        public override void draw(RenderWindow app)
        {
            base.draw(app);
            vec2 handlecorner = gethandlecorner(app.GetView());

            handlesprite.Position = new Vector2f(handlecorner.x, handlecorner.y);

            app.Draw(handlesprite);
        }

        vec2 gethandlecorner(View cam)
        {
            vec2 handlecorner = (topleft(cam) + halfsize) - handlesize / 2;
            handlecorner.x += handledistance - halfsize.x;

            return handlecorner;
        }
    }
}
