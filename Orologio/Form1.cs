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
        //dichiaro le variabili qua per comodità
        #region Graphics
        public Pen Orologio = new Pen(Color.Black, 10);//penna(colore, spessore)
        public Brush InternoOrologio = new SolidBrush(Color.LightGray);//pennello(colore)
        public Pen Secondi = new Pen(Color.Red, 4);
        public Pen Minuti = new Pen(Color.Black, 10);
        public Pen Ore = new Pen(Color.Black, 15);
        public Pen Tacca = new Pen(Color.Black);
        public Pen Rettangolo = new Pen(Color.Black, 6);
        public Brush InternoRettangolo = new SolidBrush(Color.DarkSeaGreen);
        public Brush Numeri = new SolidBrush(Color.Black);
        public Brush Clock = new SolidBrush(Color.DarkBlue);
        Font FontNumeri, FontClock;//FontNumeri: font dei num da 1 a 12; FontClock: font dei num dell'orologio digitale
        SizeF Stringsize;//variabile per le dimensioni (altezza e larghezza)
        #endregion

        #region Angoli
        public int Alpha, Beta, Gamma, Teta;//alpha: secondi; Beta: minuti; Gamma: ore; Teta: angolo generico (viene usato dopo);
        #endregion

        #region Variabili
        public int LancettaSecondi, LancettaMinuti, LancettaOre;//lunghezza lancetta
        public int Raggio;
        public int Cx, Cy;//centri
        public float x, y;
        public int x2, y2;
        public bool TemaScuro = false;
        #endregion

        public form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)//ridisegna il form ogni volta c'è ne bisogno
        {
            //tutti i valori sono stati provati a mano, non hanno un significato preciso;
            Tema.Location = new Point(Width - 170, Height - 100);//ricolloca il pulsante per il tema scuro in base alla grandezza del form
            //metto il raggio dipendente dalla grandezza del form in modo che si ingrandisce e rimplociolisce insieme ad esso
            if (Width <= Height)
            {//uso la larghezza del form se è più piccola
                //diametro: larghezza del form - un quinto della larghezza del form [idem per l'altezza]
                Raggio = (Width - (Width / 5)) / 2;//lunghezza raggio: diametro / 2
            }
            else
            {//se no uso l'altezza del form
                Raggio = (Height - (Height / 5)) / 2;//lunghezza raggio
            }
            //metto le lunghezze dipendenti dalla raggio in modo che si ingrandiscono e rimplocioliscono insieme ad esso
            LancettaSecondi = Raggio - (Raggio / 8);//lunghezza lancette, secondi: raggio - (un ottavo del raggio)
            LancettaMinuti = Raggio - (Raggio / 5);//minuti: raggio - (un quinto del raggio)
            LancettaOre = Raggio - (Raggio / 3);//ore: raggio - (un terzo del raggio)
            Cx = Width / 2;//coordinata x del centro del form
            Cy = Height / 2 - (Height / 32);//coordinata y del centro del form - un trentaduesimo dell'altezza del form (perchè non era ben centrato)
            FontNumeri = new Font("stencil", (Raggio / 6), FontStyle.Bold); //font(font, dimensioni carattere (un sesto del raggio), grassetto)
            FontClock = new Font("calibri", (Raggio / 6));//font(font, dimensioni carattere (un sesto del raggio))

            /*faccio centro - raggio perchè il cerchio viene disegnato dal suo spigolo in'alto a sinistra,
              che dista esattamente quanto il raggio (sia in x che y) dal centro del cerchio;
              g.DrawEllipse(Penna, x, y, diametro (x), diametro (y));*/
            e.Graphics.DrawEllipse(Orologio, Cx - Raggio, Cy - Raggio, Raggio * 2, Raggio * 2); //Orologio
            e.Graphics.FillEllipse(InternoOrologio, Cx - Raggio, Cy - Raggio, Raggio * 2, Raggio * 2); //L'interno dell'orologio

            #region Numeri
            Teta = 60; //angolo iniziale (num 1)
            for (int i = 1; i <= 12; i++)//12 numeri, quindi si ripete 12 volte
            {
                //Ho fatto un if perchè il secondo e terzo quadrate veniva più in alto per qualche motivo;
                //Le formule sono sempre uguali però, cambia solo il valore della distanza dal centro;
                if(Teta > 0 || Teta < -180)//primo e quarto quadrante
                {
                    Stringsize = e.Graphics.MeasureString(i.ToString(), FontNumeri); //misura la larghezza e altezza del testo(stringa, font);
                    //distanza dal centro = raggio - 14 cinquantesimi del raggio
                    //x: converti ad un intero(distanza dal centro * coseno(angolo in radianti) + Cx - metà della larghezza della stringa (per centrarlo))
                    x = Convert.ToInt32((Raggio - (14 * Raggio / 50)) * Math.Cos(Teta * (Math.PI / 180)) + Cx - Stringsize.Width / 2);
                    //y: converti ad un intero(-distanza dal centro * seno(angolo in radianti) + Cy - metà dell'altezza della stringa (per centrarlo))
                    y = Convert.ToInt32(-(Raggio - (14 * Raggio / 50)) * Math.Sin(Teta * (Math.PI / 180)) + Cy - Stringsize.Height / 2);
                    e.Graphics.DrawString(i.ToString(), FontNumeri, Numeri, x, y); //g.DrawString(testo, font, pennello, x, y)
                }
                else//secondo e terzo quadrante
                {
                    Stringsize = e.Graphics.MeasureString(i.ToString(), FontNumeri);
                    //distanza dal centro = raggio - 24 centesimi del raggio
                    x = Convert.ToInt32((Raggio - (24 * Raggio / 100)) * Math.Cos(Teta * (Math.PI / 180)) + Cx - Stringsize.Width / 2);
                    y = Convert.ToInt32(-(Raggio - (24 * Raggio / 100)) * Math.Sin(Teta * (Math.PI / 180)) + Cy - Stringsize.Height / 2);
                    e.Graphics.DrawString(i.ToString(), FontNumeri, Numeri, x, y);
                }
                Teta -= 30;//30° è un dodicesimo del cerchio
            }
            #endregion

            #region Tacche
            Teta = 90; //angolo iniziale (00:00:00)
            for (int i = 0; i < 60; i++)//60 tacche, quindi si ripete 60 volte
            {
                //gli if sono solo per spessore e lungezza diversa per le tacche in punti speciali es(12, 1, 2 ecc.)
                if (Teta % 90 == 0)//ad ogni quarto (12, 3, 6, 9)
                {
                    Tacca.Width = 10;//spessore della tacca
                    //distanza dal centro = raggio - 9 sessantaquatresimi del raggio
                    //x: converti ad un intero(distanza dal centro iniziale * coseno(angolo in radianti) + Cx)
                    x = Convert.ToInt32((Raggio - (9 * Raggio / 64)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    //y: converti ad un intero(-distanza dal centro iniziale * seno(angolo in radianti) + Cy)
                    y = Convert.ToInt32(-(Raggio - (9 * Raggio / 64)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    //distanza dal centro = raggio - un trentaduesimo del raggio
                    //x2: converti ad un intero(distanza dal centro finale * coseno(angolo in radianti) + Cx)
                    x2 = Convert.ToInt32((Raggio - (Raggio / 32)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    //y2: converti ad un intero(-distanza dal centro finale * seno(angolo in radianti) + Cy)
                    y2 = Convert.ToInt32(-(Raggio - (Raggio / 32)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    //g.DrawLine(penna, x: punto iniziale, y: punto iniziale, x2: punto finale, y2: punto finale)
                    e.Graphics.DrawLine(Tacca, x, y, x2, y2);
                }
                else if (Teta % 30 == 0)//ad ogni dodicesimo (1, 2, 4, 5, 7, 8, 10, 11)
                {
                    Tacca.Width = 8;
                    //distanza dal centro = raggio - 7 sessantaquatresimi del raggio
                    x = Convert.ToInt32((Raggio - (7 * Raggio / 64)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y = Convert.ToInt32(-(Raggio - (7 * Raggio / 64)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    //distanza dal centro = raggio - un trentaduesimo del raggio
                    x2 = Convert.ToInt32((Raggio - (Raggio / 32)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y2 = Convert.ToInt32(-(Raggio - (Raggio / 32)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    e.Graphics.DrawLine(Tacca, x, y, x2, y2);
                }
                else//ad ogni sessantesimo (tutto il resto)
                {
                    Tacca.Width = 4;
                    //distanza dal centro = raggio - 7 sessantaquatresimi del raggio
                    x = Convert.ToInt32((Raggio - (7 * Raggio / 64)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y = Convert.ToInt32(-(Raggio - (7 * Raggio / 64)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    //distanza dal centro = raggio - 3 sessantaquatresimi del raggio
                    x2 = Convert.ToInt32((Raggio - (3 * Raggio / 64)) * Math.Cos(Teta * (Math.PI / 180)) + Cx);
                    y2 = Convert.ToInt32(-(Raggio - (3 * Raggio / 64)) * Math.Sin(Teta * (Math.PI / 180)) + Cy);
                    e.Graphics.DrawLine(Tacca, x, y, x2, y2);
                }
                Teta -= 6;//angolo - un sessantesimo del cerchio
            }
            #endregion

            #region Timer
            //DateTime.Now.ToString("HH:mm") --> Orario corrente in ore da 24 in stringa;
            Stringsize = e.Graphics.MeasureString(DateTime.Now.ToString("HH:mm"), FontClock);
            //x: Cx - metà della largezza del testo;
            x = Cx - Stringsize.Width / 2;
            //y: Cy + un quinto del raggio;
            y = Cy + (Raggio / 5);
            //larghezza = larghezza della stringa
            //altezza = altezza della stringa - un sesto di essa (per estetica)
            //g.DrawRectangle(penna, x, y, larghezza, altezza)
            e.Graphics.FillRectangle(InternoRettangolo, x, y, Stringsize.Width, Stringsize.Height - (Stringsize.Height / 6));
            e.Graphics.DrawRectangle(Rettangolo, x, y, Stringsize.Width, Stringsize.Height - (Stringsize.Height / 6));
            //altezza = altezza della stringa - un dodicesimo di essa (per centrarlo)
            e.Graphics.DrawString(DateTime.Now.ToString("HH:mm"), FontClock, Clock, x, y - (Stringsize.Height / 12));
            #endregion

            #region Lancietta minuti
            //ogni minuto sono 6° = (360° / 60 min)
            Beta = 90 - DateTime.Now.Minute * 6;//angolo: angolo di partenza - minuuto corrente trasformato in angolo
            //converti ad un intero(lunghezza * coseno(angolo in radianti) + Cx)
            x = Convert.ToInt32(LancettaMinuti * Math.Cos(Beta * (Math.PI / 180)) + Cx);
            //converti ad un intero(-lunghezza * seno(angolo in radianti) + Cy)
            y = Convert.ToInt32(-LancettaMinuti * Math.Sin(Beta * (Math.PI / 180)) + Cy);
            //g.DrawLine(penna, x del punto iniziale, y del punto iniziale, , x del punto finale, y del punto finale)
            e.Graphics.DrawLine(Minuti, Cx, Cy, x, y);
            #endregion

            #region Lancietta ore
            //angolo ore: angolo di partenza - ora corrente trasformato in angolo - minuto corrente trasformato in angolo
            //ogni ora sono 30° = (360° / 12 ore),
            //ogni minuto è 0.5° = (360° / (12 ore * 60 min)); non possiamo usare decimali allora facciamo ogni 2 min
            Gamma = 90 - DateTime.Now.Hour * 30 - DateTime.Now.Minute / 2;
            //stessa formula della lancietta dei minuti solo con angolo e lungezza delle ore
            x = Convert.ToInt32(LancettaOre * Math.Cos(Gamma * (Math.PI / 180)) + Cx);
            y = Convert.ToInt32(-LancettaOre * Math.Sin(Gamma * (Math.PI / 180)) + Cy);
            e.Graphics.DrawLine(Ore, Cx, Cy, x, y);
            #endregion

            /*i secondi sono alla fine così disgna la lancietta sopra il resto (è più estetico)*/
            #region Lancietta secondi
            //un secondo son 6° = (360° / 60 sec)
            Alpha = 90 - DateTime.Now.Second * 6; //angolo: angolo di partenza - secondo corrente trasformato in angolo
            //stessa formula della lancietta dei minuti solo con angolo e lungezza dei secondi
            x = Convert.ToInt32(LancettaSecondi * Math.Cos(Alpha * (Math.PI / 180)) + Cx);
            y = Convert.ToInt32(-LancettaSecondi * Math.Sin(Alpha * (Math.PI / 180)) + Cy);
            e.Graphics.DrawLine(Secondi, Cx, Cy, x, y);
            #endregion

            //g.FillEllipse(colore pieno, cx - raggio cerchio, cy - raggio cerchio, diametro, diametero)
            e.Graphics.FillEllipse(Numeri, Cx - 15, Cy - 15, 30, 30); //Cerchio centrale, il pennello è numeri solo per comodità (è nero)
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            //bisogna cancellare le lanciatte che sono state disegnate prima di disegnare di nuovo la lancietta
            Invalidate(); //(testo originale: Invalidates the entire surface of the control and causes the control to be redrawn)
            //da qual che ho capito sbianca tutto e lo ridisegna (l'ho trovato su google)
        }

        private void TemaScuro_Click(object sender, EventArgs e)
        {//Semplice da spigare, cambia solo i colori
            if(TemaScuro)//se il tema è scuro metti chiaro
            {
                BackColor = Color.White;//colore form
                Orologio.Color = Color.Black;//colore dell'orologio ecc.
                Minuti.Color = Color.Black;
                Ore.Color = Color.Black;
                Tacca.Color = Color.Black;
                Numeri = new SolidBrush(Color.Black);
                InternoRettangolo = new SolidBrush(Color.DarkSeaGreen);
                InternoOrologio = new SolidBrush(Color.LightGray);

                Tema.BackColor = Color.White;//colore del pulsante
                Tema.ForeColor = Color.Black;//colore del testo del pulsante
                Tema.Text = "Metti tema scuro";
                TemaScuro = false;
            }
            else//se no metti scuro
            {
                BackColor = Color.FromArgb(25, 39, 52);//fromArgb: prende un colore in Red, Green, Blue (RGB)
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