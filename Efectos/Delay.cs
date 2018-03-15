using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efectos
{
    class Delay : ISampleProvider
    {
        private ISampleProvider fuente;

        int offsetTiempoMS;
        public int OffsetTiempoMS
                {
                    get
                    {
                        return offsetTiempoMS;
                    }

                    set
                    {
                        offsetTiempoMS = value;              
                    }

                }
        List<float> muestras = new List<float>();

        public Delay(ISampleProvider fuente)
        {
            this.fuente = fuente;
            offsetTiempoMS = 1000;
            //50ms - 5000ms
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        //Offset es el numero de muestras leidas
        public int Read(float[] buffer, int offset, int count)
        {
            //Calculo de tiempos
            var read = fuente.Read(buffer, offset, count);
            float tiempoTranscurrido =
               (float) muestras.Count / (float)fuente.WaveFormat.SampleRate;
            int muestrasTranscurridas = muestras.Count;
            float tiempoTranscurridoMS = tiempoTranscurrido * 1000;
            int numMuestraOffsetTiempo = (int)
                (((float)offsetTiempoMS / 1000.0f) * (float)fuente.WaveFormat.SampleRate);

            //Añadir muestrar al buffer
            for (int i = 0; i < read; i++)
            {
                muestras.Add(buffer[i]);
            }

            //Modificar muestras
            if (tiempoTranscurridoMS > offsetTiempoMS)
            {
                for (int i = 0; i < read; i++)
                {
                    buffer[i] += 
                        muestras[muestrasTranscurridas +
                        i - numMuestraOffsetTiempo];
                }

            }
            return read;
        }
    }
}
