﻿using System;
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
        #region Graphics
        public Graphics g;
        public Pen Orologio = new Pen(Color.Black, 10);//penna(colore, spessore)
        public Pen Secondi = new Pen(Color.Red, 4);
        public Pen Minuti = new Pen(Color.Black, 10);
        public Pen Ore = new Pen(Color.Black, 15);
        public Pen Tacca = new Pen(Color.Black);
        public Pen Rettangolo = new Pen(Color.Black, 6);
        public Brush Numeri = new SolidBrush(Color.Black);
        public Brush Clock = new SolidBrush(Color.Blue);//pennello(colore)
        Font FontNumeri = new Font("lobster", 50, FontStyle.Bold);//font(font, dimenasioni carattere, grassetto)
        Font FontClock = new Font("lobster", 50);
        SizeF Stringsize;//variabile per le dimensioni (altezza e larghezza)
        #endregion

        #region Angoli
        public int Alpha = 90 - DateTime.Now.Second * 6;//angoli: angolo di partenza - orario corrente trasformato in angolo
        public int Beta = 90 - DateTime.Now.Minute * 6;
        //angolo ore: angolo di partenza - ora corrente trasformato in angolo - minuto corrente trasformato in angolo
        public int Gamma = 90 - DateTime.Now.Hour * 30 - DateTime.Now.Minute / 2;//ogni ora sono 30°(360/12),
                                                                                //ogni minuto è 0.5°(360/(12*60)), allora facciamo ogni 2 min (*2)
        public int Teta;
        #endregion

        #region Variabili
        public int LancettaSecondi = 300;//lunghezza lancetta
        public int LancettaMinuti = 270;
        public int LancettaOre = 230;
        public int Raggio = 350;
        public int Cx;
        public int Cy;
        public float x;
        public int y;
        public int x2;
        public int y2;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            Cx = Width / 2;//coordinata x del contro del form
            Cy = Height / 2;//coordinata y del contro del form
            g = CreateGraphics();

            //bisogna cancellare le lanciatte che sono state disegnate prima di disegnare di nuovo la lancietta
            g.Clear(Color.White);//sbianca il form

            #region Parte non movente

            /*g.DrawEllipse(Penna, x, y, diametro (x), diametro (y));
            faccio centro - raggio perchè il form disegna dall'angolo in'alto a sinistra,
            che dista quanto il raggio sia in x che y dal centro del cerchio;*/
            g.DrawEllipse(Orologio, Cx - Raggio, Cy - Raggio, Raggio * 2, Raggio * 2); //Orologio

            #region Numeri
            Teta = 60; //angolo iniziale (num 1)
            for (int i = 1; i <= 12; i++)//12 numeri, quindi si ripete 12 volte
            {
                Stringsize = g.MeasureString(i.ToString(), FontNumeri); //misura la larghezza e altezza del testo(stringa, font);
                //x: converti ad un intero(distanza dal centro * coseno(angolo in radianti) + Cx - metà della larghezza della stringa (per centrarlo))
                x = Convert.ToInt32((Raggio - 80) * Math.Cos(Teta * (Math.PI / 180)) + Cx - Stringsize.Width / 2);
                //y: converti ad un intero(-distanza dal centro * seno(angolo in radianti) + Cy - metà dell'altezza della stringa (per centrarlo))
                y = Convert.ToInt32(-(Raggio - 80) * Math.Sin(Teta * (Math.PI / 180)) + Cy - Stringsize.Height / 2);
                g.DrawString(i.ToString(), FontNumeri, Numeri, x, y); //g.DrawString(testo, font, pennello, x, y)
                Teta -= 30;
            }
            #endregion

            #region Tacche
            Teta = 90; //angolo iniziale (00:00:00)
            for (int i = 0; i < 60; i++)//60 tacche, quindi si ripete 12 volte
            {
                //gli if sono solo per spessore e lungezza diversa per le tacche in punti speciale es(12,1, 2 ecc.)
                if (Teta % 90 == 0)//ad ogni quarto (12, 3, 6, 9)
                {
                    Tacca = new Pen(Color.Black, 10);
                    //x: converti ad un intero(distanza dal centro iniziale * coseno(angolo in radianti) + Cx)
                    x = Convert.ToInt32((Raggio - 40) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    //y: converti ad un intero(-distanza dal centro iniziale * seno(angolo in radianti) + Cy)
                    y = Convert.ToInt32(-(Raggio - 40) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    //x2: converti ad un intero(distanza dal centro finale * coseno(angolo in radianti) + Cx)
                    x2 = Convert.ToInt32((Raggio - 10) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    //y2: converti ad un intero(-distanza dal centro finale * seno(angolo in radianti) + Cy)
                    y2 = Convert.ToInt32(-(Raggio - 10) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    //g.DrawLine(penna, x: punto iniziale, y: punto iniziale, x2: punto finale, y2: punto finale)
                    g.DrawLine(Tacca, x, y, x2, y2);
                }
                else if (Teta % 30 == 0)//ad ogni dodicesimo (1, 2, 4, 5, 7, 8, 10, 11)
                {
                    Tacca = new Pen(Color.Black, 8);
                    x = Convert.ToInt32((Raggio - 35) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y = Convert.ToInt32(-(Raggio - 35) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    x2 = Convert.ToInt32((Raggio - 10) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y2 = Convert.ToInt32(-(Raggio - 10) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    g.DrawLine(Tacca, x, y, x2, y2);
                }
                else//ad ogni sessantesimo (tutto il resto)
                {
                    Tacca = new Pen(Color.Black, 4);
                    x = Convert.ToInt32((Raggio - 30) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y = Convert.ToInt32(-(Raggio - 30) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    x2 = Convert.ToInt32((Raggio - 15) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y2 = Convert.ToInt32(-(Raggio - 15) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    g.DrawLine(Tacca, x, y, x2, y2);
                }
                Teta -= 6;//angolo - un sessantesimo del cerchio
            }
            #endregion

            #endregion

            #region Parte movente

            #region Timer
            //DateTime.Now.ToString("HH:mm") --> Orario corrente in ore da 24 in stringa;
            Stringsize = g.MeasureString(DateTime.Now.ToString("HH:mm"), FontClock);
            //x: Cx - metà della largezza del testo;
            x = Cx - Stringsize.Width / 2;
            //y: Cy + un quinto del raggio;
            y = Cy + (Raggio / 5);
            //g.DrawRectangle(penna, x, y, larghezza, altezza)
            g.DrawRectangle(Rettangolo, x, y, Stringsize.Width, Stringsize.Height);
            g.DrawString(DateTime.Now.ToString("HH:mm"), FontClock, Clock, x, y);
            #endregion

            #region Lancietta minuti
            //muovi di un minuto solo se la lancietta dei secondi e a ore 12 (0 secondi/60 secondi)
            if (DateTime.Now.Second == 0)
            {
                Beta -= 6;//ogni minuto sono 6°
            }
            //converti ad un intero(lunghezza * coseno(angolo in radianti) + Cx)
            x = Convert.ToInt32(LancettaMinuti * Math.Cos(Beta * (Math.PI / 180)) + Cx);
            //converti ad un intero(-lunghezza * seno(angolo in radianti) + Cy)
            y = Convert.ToInt32(-LancettaMinuti * Math.Sin(Beta * (Math.PI / 180)) + Cy);
            //g.DrawLine(penna, x del punto iniziale, y del punto iniziale, , x del punto finale, y del punto finale)
            g.DrawLine(Minuti, Cx, Cy, x, y);
            #endregion

            #region Lancietta ore
            //muovi di 1° solo se la lancietta dei secondi e a ore 12 (0 secondi/60 secondi) E ogni 2 minuti
            if (DateTime.Now.Minute % 2 == 0 && DateTime.Now.Second == 0)
            {
                Gamma -= 1;//ogni 2 minuti sono 1° per la lancietta delle ore
            }
            //stessa formula della lancietta dei minuti solo con angolo e lungezza delle ore
            x = Convert.ToInt32(LancettaOre * Math.Cos(Gamma * (Math.PI / 180)) + Cx);
            y = Convert.ToInt32(-LancettaOre * Math.Sin(Gamma * (Math.PI / 180)) + Cy);
            g.DrawLine(Ore, Cx, Cy, x, y);
            #endregion

            /*i secondi sono alla fine così disgna la lancietta sopra il resto (è più estetico)*/
            #region Lancietta secondi
            Alpha -= 6; //ogni secondo sono 6°
            //stessa formula della lancietta dei minuti solo con angolo e lungezza dei secondi
            x = Convert.ToInt32(LancettaSecondi * Math.Cos(Alpha * (Math.PI / 180)) + Cx);
            y = Convert.ToInt32(-LancettaSecondi * Math.Sin(Alpha * (Math.PI / 180)) + Cy);
            g.DrawLine(Secondi, Cx, Cy, x, y);
            #endregion

            #endregion

            //g.FillEllipse(colore pieno, cx - raggio cerchio/2, cy - raggio cerchio/2, diametrox, diametero y)
            g.FillEllipse(Brushes.Black, Cx - 15, Cy - 15, 30, 30); //Cerchio centrale
        }
    }
}