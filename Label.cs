using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;
using SFML.Graphics;
using System.Reflection.Emit;

namespace UI
{
    internal class Label : UIelement
    {
        public Text text { get; set; }
        private Font font = new("assets/AldotheApache.ttf");
        float textyoffset = 0;

        public Label(string myname, vec2 locked, vec2 offset, vec2 hsize, string label)
            : base(myname, locked, offset, hsize)
        {
            text = new(label, font);
            vec2 textsize = new(text.GetGlobalBounds().Width, text.GetGlobalBounds().Height);

            vec2 scalefac = halfsize * 2 / textsize;
            float smaller = MathF.Min(scalefac.x, scalefac.y);
            text.Scale = new(smaller, smaller);
            text.Origin = new(text.GetLocalBounds().Left, text.GetLocalBounds().Top);

            textyoffset = halfsize.y - textsize.y / 2;
        }

        public void retext(string newtext)
        {
            text = new(newtext, font);
            vec2 textsize = new(text.GetGlobalBounds().Width, text.GetGlobalBounds().Height);

            vec2 scalefac = halfsize * 2 / textsize;
            float smaller = MathF.Min(scalefac.x, scalefac.y);
            text.Scale = new(smaller, smaller);
            text.Origin = new(text.GetLocalBounds().Left, text.GetLocalBounds().Top);

            textyoffset = halfsize.y - textsize.y / 2;
        }

        public override void draw(RenderWindow app)
        {
            vec2 pos = topleft(app.GetView());
            pos.y += textyoffset;
            FloatRect r = text.GetLocalBounds();

            text.Position = new(pos.x, pos.y);
            app.Draw(text);
        }
    }
}
