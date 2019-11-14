﻿using System;
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
            if(frecuenciaFundamental <= 161 && frecuenciaFundamental <= 164)
            {
                imgFlechaDerecha.Visibility = Visibility.Visible;
                imgFlechaIzquierda.Visibility = Visibility.Hidden;
            }
            else if(frecuenciaFundamental > 164 && frecuenciaFundamental <= 200 )
            {
                imgFlechaDerecha.Visibility = Visibility.Hidden;
                imgFlechaIzquierda.Visibility = Visibility.Visible;
                
            }
            if (frecuenciaFundamental >= 161 && frecuenciaFundamental <= 167)
            {
                imgFlechaDerecha.Visibility = Visibility.Visible;
                lblTono.Text = "E3";
                lblCuerda.Text = "Cuerda 6";
            }


            else if (frecuenciaFundamental >= 217 && frecuenciaFundamental <= 223)
            {
                lblTono.Text = "A3";
                lblCuerda.Text = "Cuerda 5";
            }

            else if (frecuenciaFundamental >= 290 && frecuenciaFundamental <= 297)
            {
                lblTono.Text = "D4";
                lblCuerda.Text = "Cuerda 4";
            }

            else if (frecuenciaFundamental >= 389 && frecuenciaFundamental <= 395)
            {
                lblTono.Text = "G4";
                lblCuerda.Text = "Cuerda 3";
            }

            else if (frecuenciaFundamental >= 490 && frecuenciaFundamental <= 498)
            {
                lblTono.Text = "B4";
                lblCuerda.Text = "Cuerda 2";
            }

            else if (frecuenciaFundamental >= 656 && frecuenciaFundamental <= 674)
            {
                lblTono.Text = "E5";
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
