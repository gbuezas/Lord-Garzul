using Microsoft.Xna.Framework;

namespace Hunting_Lord_Garzul.Objetos
{
    public class Parallax
    {
        public string[] CapaParallax { get; set; }

        // Rectangulo adaptado al parallax para no poner tiles de mas
        // y que se adapte segun la velocidad de la capa correspondiente al limite de la pantalla
        public Rectangle RectanguloParallax;
        
        // Velocidad del parallax en el eje X
        public float ParallaxX { get; set; }

        // Velocidad del parallax en el eje Y
        public float ParallaxY { get; set; }

        public Parallax(string[] capaParallax, float parallaxX, float parallaxY)
        {
            CapaParallax = capaParallax;
            ParallaxX = parallaxX;
            ParallaxY = parallaxY;
        }
    }
}
