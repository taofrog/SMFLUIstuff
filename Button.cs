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

        public event EventHandler? buttondown;

        public event EventHandler? buttonup;

        public Button(string myname, vec2 locked, vec2 offset, vec2 hsize) : base(myname, locked, offset, hsize)
        {
            held = false;
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

        public override string ToString() => name;
    }
}
