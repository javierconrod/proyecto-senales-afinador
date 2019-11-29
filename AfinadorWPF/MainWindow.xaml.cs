using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NAudio;
using NAudio.Wave;
using NAudio.Dsp;


namespace AfinadorWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaveIn waveIn;
        WaveFormat formato;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            waveIn = new WaveIn();
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
            formato = waveIn.WaveFormat;

            waveIn.BufferMilliseconds = 500;

            waveIn.DataAvailable += WaveIn_DataAvailable;

            waveIn.StartRecording();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesGrabados = e.BytesRecorded;

            int numMuestras = bytesGrabados / 2;

            int exponente = 0;
            int numeroBits = 0;

            do
            {
                exponente++;
                numeroBits = (int)Math.Pow(2, exponente);
            } while (numeroBits < numMuestras);
            exponente -= 1;
            numeroBits = (int)Math.Pow(2, exponente);

            Complex[] muestrasComplejas = new Complex[numeroBits];

            for (int i = 0; i < bytesGrabados; i += 2)
            {
                short muestra = (short)(buffer[i + 1] << 8 | buffer[i]);
                float muestra32bits = (float)muestra / 32768.0f;
                if (i / 2 < numeroBits)
                {
                    muestrasComplejas[i / 2].X = muestra32bits;
                }
            }
            FastFourierTransform.FFT(true, exponente, muestrasComplejas);
            float[] valoresAbsolutos = new float[muestrasComplejas.Length];

            for (int i = 0; i < muestrasComplejas.Length; i++)
            {
                valoresAbsolutos[i] = (float)Math.Sqrt(((muestrasComplejas[i].X * muestrasComplejas[i].X) + (muestrasComplejas[i].Y * muestrasComplejas[i].Y)));
            }
            int indiceValorMaximo = valoresAbsolutos.ToList().IndexOf(valoresAbsolutos.Max());

            float frecuenciaFundamental = (float)(indiceValorMaximo * formato.SampleRate) / (float)valoresAbsolutos.Length;

            lblFrecuencia.Text = frecuenciaFundamental.ToString("n") + " Hz";





            /* Aqui se modificann los valores para que hagan lo de apretar o aflojar la cuerda */
            //Inicio de la cuerda 6

            if (frecuenciaFundamental >= 149 && frecuenciaFundamental <= 159)   //esta floja
            {
                lblTono.Text = "Eb";
                imgFlechaDerecha.Visibility = Visibility.Visible;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Foreground = Brushes.Gray;
            }
            else if (frecuenciaFundamental >= 169 && frecuenciaFundamental <= 179) //esta tensa
            {
                lblTono.Text = "E#";
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Visible;
                lblTono.Foreground = Brushes.Gray;

            }
            else if (frecuenciaFundamental >= 160 && frecuenciaFundamental <= 168) //esta al toque prro
            {
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Text = "E";
                lblTono.Foreground = Brushes.Green;
                lblCuerda.Text = "Cuerda 6";
            }

            //Fin de la cuerda 6
            //comienzo de la cuerda 5

            else if (frecuenciaFundamental >= 204 && frecuenciaFundamental <= 214)   //esta floja
            {
                lblTono.Text = "Ab";
                imgFlechaDerecha.Visibility = Visibility.Visible;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Foreground = Brushes.Gray;
            }
            else if (frecuenciaFundamental >= 226 && frecuenciaFundamental <= 236) //esta tensa
            {
                lblTono.Text = "A#";
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Visible;
                lblTono.Foreground = Brushes.Gray;

            }
            else if (frecuenciaFundamental >= 215 && frecuenciaFundamental <= 225) //esta al toque prro
            {
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Text = "A";
                lblTono.Foreground = Brushes.Green;
                lblCuerda.Text = "Cuerda 5";
            }

            //fin de la cuerda 5
            //inicia la 4

            else if (frecuenciaFundamental >= 276 && frecuenciaFundamental <= 286)   //esta floja
            {
                lblTono.Text = "Db";
                imgFlechaDerecha.Visibility = Visibility.Visible;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Foreground = Brushes.Gray;
            }
            else if (frecuenciaFundamental >= 298 && frecuenciaFundamental <= 308) //esta tensa
            {
                lblTono.Text = "D#";
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Visible;
                lblTono.Foreground = Brushes.Gray;

            }
            else if (frecuenciaFundamental >= 287 && frecuenciaFundamental <= 297) //esta al toque prro
            {
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Text = "D";
                lblTono.Foreground = Brushes.Green;
                lblCuerda.Text = "Cuerda 4";
            }

            //termina la 4
            //inicia la 3

            else if (frecuenciaFundamental >= 180 && frecuenciaFundamental <= 190)   //esta floja
            {
                lblTono.Text = "Gb";
                imgFlechaDerecha.Visibility = Visibility.Visible;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Foreground = Brushes.Gray;
            }
            else if (frecuenciaFundamental >= 198 && frecuenciaFundamental <= 203) //esta tensa
            {
                lblTono.Text = "G#";
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Visible;
                lblTono.Foreground = Brushes.Gray;

            }
            else if (frecuenciaFundamental >= 191 && frecuenciaFundamental <= 197) //esta al toque prro
            {
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Text = "G";
                lblTono.Foreground = Brushes.Green;
                lblCuerda.Text = "Cuerda 3";
            }

            //termina la 3
            //inicia la 2

            else if (frecuenciaFundamental >= 237 && frecuenciaFundamental <= 242)   //esta floja
            {
                lblTono.Text = "Bb";
                imgFlechaDerecha.Visibility = Visibility.Visible;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Foreground = Brushes.Gray;
            }
            else if (frecuenciaFundamental >= 250 && frecuenciaFundamental <= 260) //esta tensa
            {
                lblTono.Text = "B#";
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Visible;
                lblTono.Foreground = Brushes.Gray;

            }
            else if (frecuenciaFundamental >= 243 && frecuenciaFundamental <= 249) //esta al toque prro
            {
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Text = "B";
                lblTono.Foreground = Brushes.Green;
                lblCuerda.Text = "Cuerda 2";
            }

            //termina la 2
            //comienza la 1

            else if (frecuenciaFundamental >= 313 && frecuenciaFundamental <= 323)   //esta floja
            {
                lblTono.Text = "eb";
                imgFlechaDerecha.Visibility = Visibility.Visible;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Foreground = Brushes.Gray;
            }
            else if (frecuenciaFundamental >= 333 && frecuenciaFundamental <= 343) //esta tensa
            {
                lblTono.Text = "e#";
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Visible;
                lblTono.Foreground = Brushes.Gray;

            }
            else if (frecuenciaFundamental >= 324 && frecuenciaFundamental <= 332) //esta al toque prro
            {
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
                lblTono.Text = "e";
                lblTono.Foreground = Brushes.Green;
                lblCuerda.Text = "Cuerda 1";
            }

            /* Aquí termina el codigo de mostrar la nota */

        }

        private void BtnDetener_Click(object sender, RoutedEventArgs e)
        {
            waveIn.StopRecording();
        }
    }
}
