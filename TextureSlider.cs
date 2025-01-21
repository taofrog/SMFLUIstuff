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
    internal class TextureSlider : Slider
    {
        vec2 pad;

        protected Texture slidetexture;
        protected Sprite slidesprite;

        protected Texture handletexture;
        protected Sprite handlesprite;

        protected Texture? althandletexture;
        protected Sprite? althandlesprite;

        bool usealtsprite = false;

        public TextureSlider(string myname, vec2 locked, vec2 offset, vec2 hsize, float initval, 
            string slidetexpath = "assets/notexture.png", string handletexpath = "assets/notexture.png", string? althandletexpath = null, vec2? padding = null)   
            : base(myname, locked, offset, hsize, initval)
        {
            pad = padding ?? new(0, 0);

            slidetexture = new Texture(slidetexpath);
            slidetexture.Smooth = true;
            slidesprite = new Sprite(slidetexture);
            slidesprite.Scale = new Vector2f(2 * (halfsize.x + pad.x) / slidetexture.Size.X, 2 * (halfsize.y + pad.y) / slidetexture.Size.Y);

            handletexture = new Texture(handletexpath);
            handletexture.Smooth = true;
            handlesprite = new Sprite(handletexture);
            handlesprite.Scale = new Vector2f(handlesize.x / handletexture.Size.X, handlesize.y / handletexture.Size.Y);

            if (althandletexpath != null)
            {
                usealtsprite = true;

                althandletexture = new Texture(althandletexpath);
                althandletexture.Smooth = true;
                althandlesprite = new Sprite(althandletexture);
                althandlesprite.Scale = new Vector2f(handlesize.x / handletexture.Size.X, handlesize.y / handletexture.Size.Y);
            }
        }

        public override void draw(RenderWindow app)
        {
            // slide
            vec2 pos = topleft(app.GetView()) - pad;

            slidesprite.Position = new Vector2f(pos.x, pos.y);

            app.Draw(slidesprite);

            // handle 
            vec2 handlecorner = gethandlecorner(app.GetView());

            Sprite rendersprite = new Sprite(handlesprite);
            if (held)
            {
                if (althandlesprite == null)
                {
                    rendersprite.Color = new Color(100, 100, 100, 255);
                }
                else
                {
                    rendersprite = new Sprite(althandlesprite);
                }
            }

            
            rendersprite.Position = new Vector2f(handlecorner.x, handlecorner.y);

            app.Draw(rendersprite);
        }
    }
}
