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

        public LabelButton(string myname, vec2 locked, vec2 offset, vec2 hsize, string label, string texturepath = "assets/notexture.png", string? alttexturepath = null) 
            : base(myname, locked, offset, hsize, texturepath, alttexturepath)
        {
            text = new(label, font);
            Console.WriteLine(text);
        }

        public override void draw(RenderWindow app)
        {
            base.draw(app);

            vec2 pos = topleft(app.GetView());
            text.Position = new(pos.x, pos.y);
            app.Draw(text);
        }
    }
}
