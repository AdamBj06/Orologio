using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orologio
{
    public partial class Form1 : Form
    {
        public Graphics g;
        public Pen Orologio = new Pen(Color.Black, 10);
        public Pen Secondi;
        public Pen Minuti;
        public Pen Ore;
        public Pen Tacca;
        public Brush Testo = new SolidBrush(Color.Black);
        Font FontTesto = new Font("Arial", 20, FontStyle.Bold);
        SizeF Stringsize;
        public int Alpha = 90 - DateTime.Now.Second * 6;
        public int Beta = 90 - DateTime.Now.Minute * 6;
        public int Gamma = 90 - DateTime.Now.Hour * 30;
        public int LancettaSecondi = 280;
        public int LancettaMinuti = 260;
        public int LancettaOre = 200;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            g = CreateGraphics();

            #region cancella lanciette vecchie
            //Secondi
            Secondi = new Pen(Color.White, 6);
            g.DrawLine(Secondi, Width / 2, Height / 2, Convert.ToInt32(LancettaSecondi * Math.Cos(Alpha * (Math.PI / 180)) + Width / 2), Convert.ToInt32(-LancettaSecondi * Math.Sin(Alpha * (Math.PI / 180)) + Height / 2));
            //Minuti
            Minuti = new Pen(Color.White, 8);
            g.DrawLine(Minuti, Width / 2, Height / 2, Convert.ToInt32(LancettaMinuti * Math.Cos(Beta * (Math.PI / 180)) + Width / 2), Convert.ToInt32(-LancettaMinuti * Math.Sin(Beta * (Math.PI / 180)) + Height / 2));
            //Ore
            Ore = new Pen(Color.White, 15);
            g.DrawLine(Ore, Width / 2, Height / 2, Convert.ToInt32(LancettaOre * Math.Cos(Gamma * (Math.PI / 180)) + Width / 2), Convert.ToInt32(-LancettaOre * Math.Sin(Gamma * (Math.PI / 180)) + Height / 2));
            #endregion

            #region Lancietta secondi
            Alpha -= 6;
            Secondi = new Pen(Color.Red, 6);
            g.DrawLine(Secondi, Width / 2, Height / 2, Convert.ToInt32(LancettaSecondi * Math.Cos(Alpha * (Math.PI / 180)) + Width / 2), Convert.ToInt32(-LancettaSecondi * Math.Sin(Alpha * (Math.PI / 180)) + Height / 2));
            #endregion

            #region Lancietta minuti
            if(DateTime.Now.Second == 0)
            {
                Beta -= 6;
            }
            Minuti = new Pen(Color.Black, 8);
            g.DrawLine(Minuti, Width / 2, Height / 2, Convert.ToInt32(LancettaMinuti * Math.Cos(Beta * (Math.PI / 180)) + Width / 2), Convert.ToInt32(-LancettaMinuti * Math.Sin(Beta * (Math.PI / 180)) + Height / 2));
            #endregion

            #region Lancietta ore
            if (DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                Gamma -= 30;
            }
            Ore = new Pen(Color.Black, 15);
            g.DrawLine(Ore, Width / 2, Height / 2, Convert.ToInt32(LancettaOre * Math.Cos(Gamma * (Math.PI / 180)) + Width / 2), Convert.ToInt32(-LancettaOre * Math.Sin(Gamma * (Math.PI / 180)) + Height / 2));
            #endregion

            #region Tacche
            /*Tacca = new Pen(Color.Black, 8);
            g.DrawLine(Tacca, Width / 2, Height / 2 - 265, Width / 2, Height / 2 - 290);
            Tacca = new Pen(Color.Black, 6);
            g.DrawLine(Tacca, Width / 2 + 132, Height / 2 - 222, Width / 2 + 148, Height / 2 - 245);
            //g.DrawLine(Tacca, Width / 2, Height / 2 - 88, Width / 2, Height / 2 - 1);
            //IV Quadrante
            Tacca = new Pen(Color.Black, 8);
            g.DrawLine(Tacca, Width / 2 + 265, Height / 2, Width / 2 + 290, Height / 2);
            Tacca = new Pen(Color.Black, 6);
            //g.DrawLine(Tacca, Width / 2, Height / 2 - 265, Width / 2, Height / 2 - 290);
            //g.DrawLine(Tacca, Width / 2, Height / 2 - 265, Width / 2, Height / 2 - 290);
            //III Quadrante
            Tacca = new Pen(Color.Black, 8);
            g.DrawLine(Tacca, Width / 2, Height / 2 + 265, Width / 2, Height / 2 + 290);
            Tacca = new Pen(Color.Black, 6);
            //g.DrawLine(Tacca, Width / 2, Height / 2 - 265, Width / 2, Height / 2 - 290);
            //g.DrawLine(Tacca, Width / 2, Height / 2 - 265, Width / 2, Height / 2 - 290);
            //II Quadrante
            Tacca = new Pen(Color.Black, 8);
            g.DrawLine(Tacca, Width / 2 - 265, Height / 2, Width / 2 - 290, Height / 2);
            Tacca = new Pen(Color.Black, 6);
            //g.DrawLine(Tacca, Width / 2, Height / 2 - 265, Width / 2, Height / 2 - 290);
            //g.DrawLine(Tacca, Width / 2, Height / 2 - 265, Width / 2, Height / 2 - 290);*/

            /*Tacca = new Pen(Color.Black, 8);
            for (int i = 0; i < 60; i++)
            {
                g.DrawLine(Ore, Width / 2 + 135, Height / 2 - 225, Convert.ToInt32(15 * Math.Cos(Gamma * (Math.PI / 180)) + Width / 2), Convert.ToInt32(-15 * Math.Sin(Gamma * (Math.PI / 180)) + Height / 2));
            }*/
            #endregion
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            g = CreateGraphics();
            g.DrawEllipse(Orologio, Width / 2 - 300, Height / 2 - 300, 600, 600); //Orologio

            #region Numeri
            //I Quadrante
            Stringsize = g.MeasureString("12", FontTesto);
            g.DrawString("12", FontTesto, Testo, Width / 2 - Stringsize.Width / 2, Height / 2 - 340);
            Stringsize = g.MeasureString("1", FontTesto);
            g.DrawString("1", FontTesto, Testo, Width / 2 + 170 - Stringsize.Width / 2, Height / 2 - 283 - Stringsize.Height / 2);
            Stringsize = g.MeasureString("2", FontTesto);
            g.DrawString("2", FontTesto, Testo, Width / 2 + 283 - Stringsize.Width / 2, Height / 2 - 170 - Stringsize.Height / 2);
            //IV Quadrante
            Stringsize = g.MeasureString("3", FontTesto);
            g.DrawString("3", FontTesto, Testo, Width / 2 + 320, Height / 2 - Stringsize.Height/2);
            Stringsize = g.MeasureString("4", FontTesto);
            g.DrawString("4", FontTesto, Testo, Width / 2 + 283 - Stringsize.Width / 2, Height / 2 + 170 - Stringsize.Height / 2);
            Stringsize = g.MeasureString("5", FontTesto);
            g.DrawString("5", FontTesto, Testo, Width / 2 + 170 - Stringsize.Width / 2, Height / 2 + 283 - Stringsize.Height / 2);
            //III Quadrante
            Stringsize = g.MeasureString("6", FontTesto);
            g.DrawString("6", FontTesto, Testo, Width / 2 - Stringsize.Width / 2, Height / 2 + 320);
            Stringsize = g.MeasureString("7", FontTesto);
            g.DrawString("7", FontTesto, Testo, Width / 2 - 170 - Stringsize.Width / 2, Height / 2 + 283 - Stringsize.Height / 2);
            Stringsize = g.MeasureString("8", FontTesto);
            g.DrawString("8", FontTesto, Testo, Width / 2 - 283 - Stringsize.Width / 2, Height / 2 + 170 - Stringsize.Height / 2);
            //II Quadrante
            Stringsize = g.MeasureString("9", FontTesto);
            g.DrawString("9", FontTesto, Testo, Width / 2 - 340 , Height / 2 - Stringsize.Height / 2);
            Stringsize = g.MeasureString("10", FontTesto);
            g.DrawString("10", FontTesto, Testo, Width / 2 - 283 - Stringsize.Width / 2, Height / 2 - 170 - Stringsize.Height / 2);
            Stringsize = g.MeasureString("11", FontTesto);
            g.DrawString("11", FontTesto, Testo, Width / 2 - 170 - Stringsize.Width / 2, Height / 2 - 283 - Stringsize.Height / 2);
            #endregion
        }
    }
}