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
    public partial class form1 : Form
    {
        #region Graphics
        public Pen Orologio = new Pen(Color.Black, 10);
        public Brush InternoOrologio = new SolidBrush(Color.LightGray);
        public Pen Secondi = new Pen(Color.Red, 4);
        public Pen Minuti = new Pen(Color.Black, 10);
        public Pen Ore = new Pen(Color.Black, 16);
        public Pen Tacca = new Pen(Color.Black);
        public Pen Rettangolo = new Pen(Color.Black, 6);
        public Brush InternoRettangolo = new SolidBrush(Color.DarkSeaGreen);
        public Brush Numeri = new SolidBrush(Color.Black);
        public Brush Clock = new SolidBrush(Color.DarkBlue);
        Font FontNumeri, FontClock;
        SizeF Stringsize;
        #endregion

        #region Angoli
        public int Alpha, Beta, Gamma, Teta;
        #endregion

        #region Variabili
        public int LancettaSecondi, LancettaMinuti, LancettaOre;
        public int Raggio;
        public int Cx, Cy;
        public float x, y;
        public int x2, y2;
        public bool TemaScuro = false;
        #endregion

        public form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Tema.Location = new Point(Width - 170, Height - 100);
            if (Width <= Height)
            {
                Raggio = (Width - (Width / 5)) / 2;
            }
            else
            {
                Raggio = (Height - (Height / 5)) / 2;
            }
            LancettaSecondi = Raggio - (Raggio / 8);
            LancettaMinuti = Raggio - (Raggio / 5);
            LancettaOre = Raggio - (Raggio / 3);
            Cx = Width / 2;
            Cy = Height / 2 - (Height / 32);
            FontNumeri = new Font("stencil", (Raggio / 6), FontStyle.Bold);
            FontClock = new Font("calibri", (Raggio / 6));

            e.Graphics.DrawEllipse(Orologio, Cx - Raggio, Cy - Raggio, Raggio * 2, Raggio * 2);
            e.Graphics.FillEllipse(InternoOrologio, Cx - Raggio, Cy - Raggio, Raggio * 2, Raggio * 2);

            #region Numeri
            Teta = 60;
            for (int i = 1; i <= 12; i++)
            {
                if(Teta > 0 || Teta < -180)
                {
                    Stringsize = e.Graphics.MeasureString(i.ToString(), FontNumeri);
                    x = Convert.ToInt32((Raggio - (14 * Raggio / 50)) * Math.Cos(Teta * (Math.PI / 180)) + Cx - Stringsize.Width / 2);
                    y = Convert.ToInt32(-(Raggio - (14 * Raggio / 50)) * Math.Sin(Teta * (Math.PI / 180)) + Cy - Stringsize.Height / 2);
                    e.Graphics.DrawString(i.ToString(), FontNumeri, Numeri, x, y);
                }
                else
                {
                    Stringsize = e.Graphics.MeasureString(i.ToString(), FontNumeri);
                    x = Convert.ToInt32((Raggio - (24 * Raggio / 100)) * Math.Cos(Teta * (Math.PI / 180)) + Cx - Stringsize.Width / 2);
                    y = Convert.ToInt32(-(Raggio - (24 * Raggio / 100)) * Math.Sin(Teta * (Math.PI / 180)) + Cy - Stringsize.Height / 2);
                    e.Graphics.DrawString(i.ToString(), FontNumeri, Numeri, x, y);
                }
                Teta -= 30;
            }
            #endregion

            #region Tacche
            Teta = 90;
            for (int i = 0; i < 60; i++)
            {
                if (Teta % 90 == 0)
                {
                    Tacca.Width = 10;
                    x = Convert.ToInt32((Raggio - (9 * Raggio / 64)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y = Convert.ToInt32(-(Raggio - (9 * Raggio / 64)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    x2 = Convert.ToInt32((Raggio - (Raggio / 32)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y2 = Convert.ToInt32(-(Raggio - (Raggio / 32)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    e.Graphics.DrawLine(Tacca, x, y, x2, y2);
                }
                else if (Teta % 30 == 0)
                {
                    Tacca.Width = 8;
                    x = Convert.ToInt32((Raggio - (7 * Raggio / 64)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y = Convert.ToInt32(-(Raggio - (7 * Raggio / 64)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    x2 = Convert.ToInt32((Raggio - (Raggio / 32)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y2 = Convert.ToInt32(-(Raggio - (Raggio / 32)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    e.Graphics.DrawLine(Tacca, x, y, x2, y2);
                }
                else
                {
                    Tacca.Width = 4;
                    x = Convert.ToInt32((Raggio - (7 * Raggio / 64)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y = Convert.ToInt32(-(Raggio - (7 * Raggio / 64)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    x2 = Convert.ToInt32((Raggio - (3 * Raggio / 64)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y2 = Convert.ToInt32(-(Raggio - (3 * Raggio / 64)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    e.Graphics.DrawLine(Tacca, x, y, x2, y2);
                }
                Teta -= 6;
            }
            #endregion

            #region Orologio digitale
            Stringsize = e.Graphics.MeasureString(DateTime.Now.ToString("HH:mm"), FontClock);
            x = Cx - Stringsize.Width / 2;
            y = Cy + (Raggio / 5);
            e.Graphics.FillRectangle(InternoRettangolo, x, y, Stringsize.Width, Stringsize.Height - (Stringsize.Height / 6));
            e.Graphics.DrawRectangle(Rettangolo, x, y, Stringsize.Width, Stringsize.Height - (Stringsize.Height / 6));
            e.Graphics.DrawString(DateTime.Now.ToString("HH:mm"), FontClock, Clock, x, y - (Stringsize.Height / 12));
            #endregion

            #region Lancetta minuti
            Beta = 90 - DateTime.Now.Minute * 6;
            x = Convert.ToInt32(LancettaMinuti * Math.Cos(Beta * (Math.PI / 180)) + Cx);
            y = Convert.ToInt32(-LancettaMinuti * Math.Sin(Beta * (Math.PI / 180)) + Cy);
            e.Graphics.DrawLine(Minuti, Cx, Cy, x, y);
            #endregion

            #region Lancetta ore
            Gamma = 90 - DateTime.Now.Hour * 30 - DateTime.Now.Minute / 2;
            x = Convert.ToInt32(LancettaOre * Math.Cos(Gamma * (Math.PI / 180)) + Cx);
            y = Convert.ToInt32(-LancettaOre * Math.Sin(Gamma * (Math.PI / 180)) + Cy);
            e.Graphics.DrawLine(Ore, Cx, Cy, x, y);
            #endregion

            #region Lancetta secondi
            Alpha = 90 - DateTime.Now.Second * 6;
            x = Convert.ToInt32(LancettaSecondi * Math.Cos(Alpha * (Math.PI / 180)) + Cx);
            y = Convert.ToInt32(-LancettaSecondi * Math.Sin(Alpha * (Math.PI / 180)) + Cy);
            e.Graphics.DrawLine(Secondi, Cx, Cy, x, y);
            #endregion

            e.Graphics.FillEllipse(Numeri, Cx - 15, Cy - 15, 30, 30);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void TemaScuro_Click(object sender, EventArgs e)
        {
            if(TemaScuro)
            {
                BackColor = Color.White;
                Orologio.Color = Color.Black; 
                Minuti.Color = Color.Black;
                Ore.Color = Color.Black;
                Tacca.Color = Color.Black;
                Numeri = new SolidBrush(Color.Black);
                InternoRettangolo = new SolidBrush(Color.DarkSeaGreen);
                InternoOrologio = new SolidBrush(Color.LightGray);

                Tema.BackColor = Color.White; 
                Tema.ForeColor = Color.Black; 
                Tema.Text = "Metti tema scuro";
                TemaScuro = false;
            } 
            else
            {
                BackColor = Color.FromArgb(25, 39, 52); 
                Orologio.Color = Color.LightGray;
                Minuti.Color = Color.LightGray;
                Ore.Color = Color.LightGray;
                Tacca.Color = Color.LightGray;
                Numeri = new SolidBrush(Color.LightGray);
                InternoRettangolo = new SolidBrush(Color.DarkOliveGreen);
                InternoOrologio = new SolidBrush(Color.FromArgb(21, 32, 43));

                Tema.BackColor = Color.FromArgb(25, 39, 52);
                Tema.ForeColor = Color.LightGray;
                Tema.Text = "Metti tema chiaro";
                TemaScuro = true;
            }
        }
    }
}