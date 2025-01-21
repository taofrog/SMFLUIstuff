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
    internal class LabelButton : TextureButton
    {
        private Text text;
        private Font font = new("assets/SuperStory.ttf");
        float textyoffset = 0;

        public LabelButton(string myname, vec2 locked, vec2 offset, vec2 hsize, string label, string texturepath = "assets/notexture.png", string? alttexturepath = null) 
            : base(myname, locked, offset, hsize, texturepath, alttexturepath)
        {
            text = new(label, font);
            vec2 textsize = new(text.GetGlobalBounds().Width, text.GetGlobalBounds().Height);
            float scalefac = halfsize.x * 2 / textsize.x;
            text.Scale = new(scalefac, scalefac);
            text.Origin = new(text.GetLocalBounds().Left, text.GetLocalBounds().Top);

            textsize *= scalefac;

            textyoffset = halfsize.y - textsize.y / 2;
        }

        public override void draw(RenderWindow app)
        {
            base.draw(app);

            vec2 pos = topleft(app.GetView());
            pos.y += textyoffset;

            FloatRect r = text.GetLocalBounds();

            text.Position = new(pos.x, pos.y);
            app.Draw(text);
        }
    }
}
