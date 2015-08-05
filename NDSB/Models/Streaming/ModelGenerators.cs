﻿using System.Collections.Generic;

namespace DataScienceECom.Models
{
    public static class ModelGenerators
    {
        public static IEnumerable<IStreamingModel> Entropia()
        {
            for (int i = 4; i < 9; i++)
                for (double j = 0.5; j < 1.1; j += 0.2)
                    yield return new GMEntropia(j, 2, i);
        }

        public static IEnumerable<IStreamingModel> Entropia2()
        {
            for (double i = 7; i < 10; i += 0.5)
                for (double j = 0.8; j < 1.2; j += 0.05)
                    yield return new GMEntropia(j, 2, i);
        }

        public static IEnumerable<IStreamingModel> Entropia3()
        {
            for (int i = 6; i < 9; i += 1)
                for (double j = 0.7; j < 1.2; j += 0.1)
                    yield return new GMEntropia(j, 4, i);
        }

        public static IEnumerable<IStreamingModel> Entropia4()
        {
            for (int i = 6; i < 9; i += 1)
                for (double j = 0.5; j < 0.9; j += 0.1)
                    yield return new GMEntropia(j, 4, i, 500000);
        }

        public static IEnumerable<IStreamingModel> Entropia5()
        {
            yield return new GMEntropia(0.5, 4, 6);
        }

        public static IEnumerable<IStreamingModel> Entropia6()
        {
            yield return new GMEntropia(0.5, 4, 6, 500000);
            //yield return new GMEntropia(0.5, 4, 7, 500000);
            //yield return new GMEntropia(0.5, 4, 8, 500000);
        }

        public static IEnumerable<IStreamingModel> Entropia7()
        {
            yield return new GMEntropia(0.5, 4, 7, 500000);
            yield return new GMEntropia(0.7, 4, 7, 500000);
            yield return new GMEntropia(0.9, 4, 7, 500000);
            yield return new GMEntropia(1.1, 4, 7, 500000);

            yield return new GMEntropia(0.5, 4, 6, 500000);
            yield return new GMEntropia(0.7, 4, 6, 500000);
            yield return new GMEntropia(0.9, 4, 6, 500000);
            yield return new GMEntropia(1.1, 4, 6, 500000);
        }
    }
}
