﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using LotsOfClients.KeyValueBaseReference;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace LotsOfClients {
    class Program {
        // ZIPF
        private static int[,] ids = new int[,] { { 0, 0, 14, 3, 3, 0, 4, 0, 20, 0, 8, 0, 3, 4, 0, 3, 19, 1753, 112, 0, 0, 44, 1, 2748, 4, 0, 1, 4750, 1, 0, 3, 918, 0, 0, 1, 4, 0, 0, 0, 0, 18, 21, 0, 0, 0, 4, 3, 1, 4, 2159, 0, 1, 0, 0, 4, 66, 1, 108, 0, 44, 0, 0, 0, 38, 0, 0, 4, 106, 15, 6, 3, 4, 1, 4, 0, 0, 0, 8, 3, 0, 4, 4, 4, 484, 0, 0, 2104, 367, 0, 10, 1, 0, 0, 0, 1, 4, 0, 0, 4, 20 }, { 11, 0, 0, 37, 1, 357, 0, 1, 0, 1, 3, 0, 4, 0, 22, 8, 4, 59, 0, 0, 108, 1, 0, 0, 182, 1, 0, 0, 1, 4, 0, 1, 4, 1, 0, 146, 0, 4, 27, 15, 21, 1, 0, 1, 5, 0, 110, 6, 5, 4, 0, 4, 1, 0, 4, 0, 0, 0, 0, 77, 19, 0, 0, 0, 17, 3, 0, 14, 66, 1, 0, 1, 40, 0, 0, 1919, 3, 5678, 0, 1, 113, 17, 0, 11, 0, 0, 0, 17, 0, 0, 14, 69602, 0, 0, 3, 42, 10, 0, 192, 1 }, { 3, 0, 0, 0, 121, 0, 0, 5, 28, 11, 0, 0, 0, 0, 1, 14, 76, 0, 15, 0, 5, 1, 1, 1, 29, 8, 3, 0, 43, 6, 0, 0, 0, 3, 1, 7, 122, 10, 17, 52, 123, 0, 3, 0, 0, 0, 1, 0, 388, 13, 3, 6, 8, 3, 6, 11, 0, 82, 12, 18, 0, 28, 0, 11, 0, 12, 0, 8, 1, 1, 3, 4, 1, 0, 3, 4, 4, 3, 1, 3, 0, 9, 0, 3, 20, 8, 1, 1, 0, 0, 5, 179, 1, 0, 0, 0, 0, 0, 0, 5 }, { 0, 0, 6, 0, 5, 655, 19, 0, 6, 4, 4, 1, 0, 21, 7, 3, 33, 0, 1, 659, 16, 84, 1, 0, 7, 152, 9, 0, 1, 4, 37, 1, 18, 30, 0, 9, 165, 0, 3, 3, 0, 0, 0, 0, 6, 0, 0, 0, 16, 13, 8, 38964, 1, 0, 1, 0, 23, 0, 14, 5, 0, 0, 5, 1, 1, 1, 86, 3, 3, 0, 2527, 579, 20, 0, 0, 0, 0, 1, 0, 4, 9, 0, 0, 0, 0, 17, 61, 3, 0, 0, 0, 1, 26, 0, 1, 6, 0, 0, 0, 4 }, { 0, 0, 3, 5, 2609, 0, 0, 16, 12, 0, 24, 0, 3, 61, 40, 1, 0, 4, 268, 4, 0, 54, 0, 3, 3, 5, 0, 0, 473, 7, 0, 0, 0, 4, 0, 18, 0, 9, 4, 13, 0, 3, 0, 0, 1, 3, 0, 52, 0, 12, 0, 605, 4, 0, 3, 0, 7, 18, 1, 7, 0, 1, 92, 50, 0, 0, 31, 0, 3, 26, 0, 20, 0, 1, 1, 5, 0, 0, 5, 18, 0, 0, 151, 11, 0, 1, 5, 0, 0, 16, 0, 20, 0, 0, 0, 0, 0, 5, 1, 1 }, { 7, 0, 12, 15, 160, 3, 36, 0, 7063, 8, 41, 340, 52, 0, 0, 0, 0, 0, 3, 87, 0, 10, 3, 1, 3, 5, 8, 3, 0, 7, 0, 3, 0, 40, 7, 0, 0, 1, 12, 0, 3, 4, 0, 4, 4, 4, 0, 267, 11, 0, 0, 1, 8, 178, 7, 85, 4, 0, 0, 6, 0, 9502, 0, 6, 32, 3, 6, 0, 0, 0, 35, 117, 8, 5, 0, 69, 0, 58, 0, 1, 0, 10, 8, 0, 13, 55, 5, 0, 8, 1, 0, 85, 3, 0, 9, 0, 203, 0, 0, 0 }, { 0, 5, 32, 4, 0, 0, 0, 14, 0, 13, 16, 17, 0, 0, 1, 0, 0, 0, 1, 7, 3, 4, 0, 4, 5, 3, 1, 1338, 43, 0, 0, 12, 0, 715, 0, 0, 4, 7, 0, 0, 1, 1, 0, 0, 0, 43, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 4, 0, 0, 0, 9, 9, 0, 0, 24, 0, 0, 29, 3, 0, 0, 4, 33, 113, 32, 1, 1, 1, 679, 3, 9, 3, 0, 0, 4, 5, 1, 511, 4, 0, 3, 4, 44, 0, 10, 0, 3, 4 }, { 0, 0, 14, 3, 0, 27, 1, 5, 1, 11, 156, 9, 1, 23, 0, 1, 7, 8, 13, 0, 1, 0, 0, 4, 1, 3, 0, 0, 7, 0, 0, 0, 1, 0, 3, 0, 3, 0, 5, 4, 0, 0, 10, 0, 1, 49, 4, 0, 13, 5100, 0, 5, 0, 61, 7, 10, 3, 5, 178, 40, 0, 36, 805, 4, 0, 0, 3, 17, 5, 12, 9, 5, 7, 1, 0, 396, 0, 0, 3, 37, 0, 0, 1, 5, 0, 8, 0, 6, 0, 422, 6, 3, 8, 7, 0, 0, 0, 4, 5, 7 }, { 6, 0, 1, 134, 0, 4, 0, 1, 7, 132, 3, 3, 0, 0, 0, 4, 1085, 16, 0, 1, 32, 3, 0, 0, 17, 1, 0, 9, 0, 0, 0, 0, 7, 0, 0, 0, 3, 13, 131, 4, 0, 0, 0, 0, 0, 3, 1, 283, 0, 0, 0, 0, 0, 16, 1, 6, 28, 0, 0, 0, 0, 64, 1, 74, 68, 302, 62, 0, 0, 0, 15, 3, 3, 25, 3, 0, 0, 7, 0, 1, 1, 0, 0, 19, 1778, 4, 6, 0, 256, 0, 9, 1, 0, 0, 0, 9, 1, 1, 0, 0 }, { 1, 61, 4, 52, 0, 0, 0, 5, 0, 1, 12, 0, 0, 1, 0, 1, 1, 3, 4, 0, 6, 1, 0, 0, 4, 0, 0, 6, 1, 1, 0, 0, 1, 0, 4, 1, 3, 5, 27, 0, 105, 0, 1, 16, 8, 1, 6, 8, 1, 11, 18, 10, 7, 10, 92, 0, 0, 1, 33, 13, 0, 0, 4, 3, 0, 0, 0, 3, 65, 23, 0, 10, 42, 0, 0, 15, 23037, 5, 0, 5, 1, 0, 0, 3, 6, 1, 1, 0, 0, 0, 6, 27, 128, 0, 0, 105, 48, 29, 1, 54 }, { 0, 1, 145, 9, 0, 72, 0, 0, 3, 4, 0, 4, 4, 0, 0, 0, 345, 0, 4, 19, 4, 0, 1, 1, 161, 20, 11, 0, 3, 107, 0, 1, 0, 0, 0, 1, 1, 18, 1, 0, 0, 0, 0, 0, 8, 0, 6, 5, 1, 1, 16, 8, 11, 1, 0, 10, 1, 0, 209, 0, 1, 0, 0, 0, 0, 3, 0, 11, 1, 1, 1, 0, 1, 0, 0, 1, 34, 0, 0, 4, 0, 1, 1, 94, 1, 0, 8, 1, 0, 4, 0, 0, 0, 30, 0, 24, 0, 3, 5, 1 }, { 3, 0, 29500, 5, 794, 8, 0, 4185, 0, 5, 0, 0, 4, 1, 4, 18, 0, 5, 0, 0, 14, 6, 4, 4, 1, 3323, 1, 4, 30, 1, 0, 0, 21, 1, 13, 4, 40, 0, 0, 0, 6518, 0, 58, 1, 0, 7, 0, 0, 0, 1, 1, 19, 0, 71, 0, 0, 0, 0, 0, 4, 834, 12, 0, 19, 0, 4, 8, 0, 10, 1, 0, 15, 1, 0, 6, 0, 0, 0, 1, 64, 0, 4, 112, 3, 1, 0, 267, 18, 1, 0, 6, 50, 0, 0, 0, 165, 5045, 52, 1, 0 }, { 0, 0, 0, 1, 5, 0, 0, 4, 1, 3, 45, 0, 13, 0, 3, 40, 0, 134, 12, 10, 0, 11, 0, 21, 114, 50, 0, 3, 8, 6, 4, 107, 0, 0, 3, 32, 1, 1, 0, 4, 265, 0, 1, 1, 5, 1, 7, 1861, 0, 21, 0, 4, 6, 0, 1, 16, 7, 7, 123, 0, 0, 26920, 0, 12, 0, 54, 6, 6, 378, 0, 1, 0, 4, 3, 5, 1, 181, 0, 3, 61, 0, 0, 339, 0, 0, 6, 10, 0, 9, 37, 64, 0, 0, 34, 0, 264, 1, 0, 1, 3 }, { 5, 0, 0, 0, 0, 5, 3, 0, 71, 9, 3, 1, 0, 0, 0, 0, 0, 0, 35, 3, 0, 3197, 0, 20, 4, 38, 0, 0, 5, 0, 127, 18, 564, 0, 0, 186, 0, 88, 0, 3, 3, 115, 8, 0, 0, 0, 3, 0, 34, 3, 9, 787, 0, 0, 0, 10, 0, 13, 16, 155, 17, 0, 0, 0, 0, 0, 31, 1, 1, 237, 6, 5, 6, 6, 5, 1, 0, 0, 0, 1, 3, 1, 0, 0, 0, 86, 89, 1, 0, 3, 0, 0, 0, 0, 3, 0, 6, 0, 11491, 10 }, { 31, 1651, 0, 3, 126, 0, 0, 1, 0, 0, 57, 17, 14, 8, 0, 1, 0, 1, 262, 3, 11, 1, 3, 1, 1, 23, 0, 1, 11, 5, 0, 19, 188, 73, 0, 52, 0, 0, 1, 0, 7, 1, 0, 3, 5099, 0, 1, 1, 3, 75, 13, 59, 4, 14, 0, 3, 0, 0, 0, 0, 3, 0, 6, 35, 1, 22, 0, 1, 0, 0, 0, 1, 0, 3, 39, 10, 2605, 3, 1, 14, 1, 10, 1, 7, 151, 0, 6, 30, 3, 7, 4, 0, 0, 0, 0, 0, 3, 1, 110, 1 }, { 51, 1, 3, 7, 3, 0, 9, 0, 4, 0, 1324, 8, 163, 1, 0, 16, 1, 27, 0, 1, 4, 0, 17, 19, 0, 3, 0, 0, 19, 1503, 1, 0, 0, 8, 0, 0, 19, 1, 0, 11, 1, 5, 18, 1, 3, 1, 3, 0, 19, 23, 0, 5, 14, 0, 10, 7, 0, 1, 14, 0, 8, 0, 1, 10110, 0, 18, 0, 12, 0, 0, 6, 97, 0, 1, 0, 0, 4, 1, 0, 3, 0, 8, 0, 0, 575, 3, 6, 1029, 9, 0, 18, 0, 1, 1, 13, 0, 128, 3, 0, 5 }, { 29, 90, 50, 46, 676, 0, 0, 0, 1, 0, 9, 0, 0, 12, 93, 1, 9, 0, 1, 3, 1, 24, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 10, 551, 19, 0, 0, 0, 1, 8, 5, 1, 38, 14, 5, 40, 28, 37, 1, 0, 4, 5, 0, 0, 855, 0, 3, 181, 29, 0, 1234, 45, 0, 1189, 0, 3, 0, 302, 3, 0, 50, 13, 0, 1, 1, 1, 0, 1, 22, 0, 0, 0, 3, 0, 5, 3, 1, 11, 0, 0, 1037, 3, 0, 0, 1, 0, 1, 0 }, { 10, 1, 0, 0, 10, 1, 0, 3, 0, 393, 4, 3, 1, 1, 1, 4, 0, 4, 0, 1, 0, 0, 0, 0, 3, 7, 0, 0, 13, 0, 0, 19, 6, 28, 0, 3, 0, 7, 1, 0, 0, 1, 11, 0, 1, 4, 118, 0, 10, 4, 0, 1, 50, 0, 0, 0, 1, 1, 0, 0, 0, 5, 0, 43431, 0, 0, 1, 1, 1, 5, 0, 104, 1, 0, 3, 4, 8, 205, 3, 0, 4, 4, 13, 3, 267, 13, 0, 0, 0, 117, 1, 52, 1, 0, 14, 3, 1, 1, 1, 0 }, { 1, 0, 0, 18, 0, 6, 0, 3906, 0, 3, 1, 1, 261, 7, 0, 0, 0, 1, 0, 11, 0, 1822, 6, 0, 0, 0, 11, 0, 0, 14, 0, 1, 0, 4, 0, 0, 6, 22, 12, 0, 0, 332, 3, 146, 277, 0, 3, 1, 0, 3, 1008, 0, 4, 4, 1, 3, 0, 1, 0, 3, 0, 0, 6, 0, 27, 7, 0, 0, 0, 0, 33, 1, 27, 0, 177, 21, 0, 12, 0, 0, 0, 0, 1, 5, 110, 5, 59, 0, 0, 16, 3, 1, 0, 0, 0, 4, 3, 6, 0, 10988 }, { 4, 0, 6, 0, 40, 48, 5, 41, 7, 8, 0, 1, 14, 0, 1, 0, 0, 1, 4, 1, 7, 0, 0, 51, 6, 1, 0, 1, 0, 0, 25, 5, 22, 7, 1, 5, 1, 4, 17, 0, 1, 4, 1, 0, 0, 46, 1, 2982, 30, 7, 0, 0, 3, 3, 1, 143, 0, 4, 1, 0, 0, 0, 6, 0, 56, 0, 1, 0, 5, 0, 3, 3, 0, 24, 16, 1, 0, 7, 0, 0, 4, 1, 0, 0, 4, 0, 0, 0, 0, 3, 0, 1, 0, 0, 10, 0, 1, 7, 1, 0 }, { 3, 0, 0, 3, 0, 186, 5, 0, 16, 0, 0, 1, 4, 8, 0, 0, 0, 3, 0, 4, 1, 0, 0, 1, 438, 0, 0, 0, 578, 97, 75, 19, 1, 0, 0, 6, 0, 0, 0, 19, 1, 7, 0, 0, 5, 0, 0, 0, 93, 1, 3107, 0, 1, 0, 20, 887, 18, 1, 84, 10, 0, 0, 31, 111, 39, 37, 26, 3, 1, 0, 0, 0, 2287, 0, 1, 0, 0, 49, 0, 20, 0, 3, 11, 3, 13, 8, 0, 0, 1, 668, 0, 0, 0, 1, 0, 1, 76, 0, 5, 0 }, { 0, 21, 1, 0, 1, 0, 5, 12, 1, 0, 0, 8, 0, 50, 0, 1, 1010, 1, 17, 3, 1, 0, 49, 4, 1167, 58, 3, 0, 1, 1, 1, 4, 30, 60, 34, 3, 1, 16, 0, 15, 15, 0, 2073, 4, 17, 0, 8, 4, 0, 0, 0, 1, 0, 0, 28, 5, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 38, 0, 13, 0, 31, 7, 19, 0, 0, 15, 6, 1, 0, 4, 180, 9, 0, 13, 0, 70, 5, 1, 1, 3, 4, 32, 0, 0, 5, 1, 1, 9, 16, 0 }, { 1, 25, 3, 0, 4, 0, 1, 0, 4, 1, 75, 0, 0, 7, 0, 42, 0, 0, 0, 1, 1, 0, 0, 1, 8, 0, 15, 19, 1, 47, 44, 0, 44, 40, 3, 0, 8, 48, 4, 91, 137, 0, 0, 1, 0, 0, 0, 594, 4, 0, 1, 13, 4, 126, 0, 0, 0, 7, 1, 19, 0, 0, 0, 3, 0, 4, 0, 235, 1, 0, 124, 0, 859, 14, 1, 23, 5, 0, 0, 15, 0, 0, 3, 1, 0, 8, 0, 13, 3, 0, 1, 1, 0, 0, 0, 0, 4, 18, 103, 0 }, { 0, 13, 0, 0, 15, 0, 8, 1, 8, 0, 6, 1, 23, 21, 14, 0, 12, 26, 40, 1429, 1, 7, 0, 3, 0, 0, 3, 0, 1, 11, 0, 0, 0, 10, 4, 1, 1, 0, 11, 1, 1, 0, 0, 9, 3, 5, 1, 36, 0, 1, 0, 0, 0, 0, 1, 1, 0, 5, 1, 0, 0, 0, 71, 26, 0, 0, 0, 77, 12, 6, 0, 0, 6, 0, 5, 0, 5, 1, 230, 1, 0, 200, 0, 15, 0, 0, 4, 11, 6, 4, 4, 0, 5, 0, 3, 210, 13, 1, 1, 16 }, { 5, 35, 117, 0, 3, 0, 0, 0, 0, 1, 11, 0, 0, 0, 22, 68, 0, 136, 34, 0, 0, 953, 3, 0, 0, 0, 3, 4, 1, 7, 138, 3, 10, 0, 8, 13, 0, 3, 132, 0, 3, 4, 5, 55, 0, 3, 8, 0, 3, 0, 174, 7, 24, 0, 1, 0, 3, 0, 0, 0, 0, 4, 26, 4, 1, 864, 1, 1, 1, 5, 1, 0, 0, 0, 3, 6, 3, 55, 1326, 0, 0, 1, 74, 1, 0, 1779, 6, 1, 1, 1, 3, 0, 7, 6, 10, 0, 0, 3, 2372, 4 } };

        private static int NUM_ACCESSES = 100;
        private static string TEST_FILE = "Slashdot0902.txt";

        private static int numLeft;

        static void Main(string[] args) {
            KeyValueBaseServiceClient client = new KeyValueBaseServiceClient();
            client.Reset();
            client.Init(TEST_FILE);

            StreamWriter sw = new StreamWriter(@"C:\Users\Sebastian\Dropbox\Studie\Kurser\Datalogi\PCSD\Assignments\1\tex\lots-of-clients_100reqs_zipf0.5.txt", false);

            for (int num = 1; num <= 25; num++) {
                sw.WriteLine("== {0} simultaneous workers ==", num);
                Console.WriteLine("== {0} simultaneous workers ==", num);
                
                BackgroundWorker[] bws = new BackgroundWorker[num];

                numLeft = num;

                for (int i = 0; i < num; i++) {
                    bws[i] = new BackgroundWorker();
                    bws[i].DoWork += (obj, arg) => {
                        
                        KeyValueBaseServiceClient workerClient = new KeyValueBaseServiceClient();
                        int workerNum = (int)arg.Argument;
                        Stopwatch watch = Stopwatch.StartNew();

                        for (int j = 0; j < NUM_ACCESSES; j++) {
                            KeyImpl key = new KeyImpl() { Key = ids[workerNum, j] };
                            workerClient.Read(key);
                        }

                        watch.Stop();
                        lock (sw) {
                            sw.WriteLine("{1}", workerNum, watch.ElapsedMilliseconds);
                        }

                        Interlocked.Decrement(ref numLeft);
                    };

                    bws[i].RunWorkerAsync(i);
                }

                while (numLeft > 0) {
                    Thread.Sleep(100);
                }
            }

            sw.Flush();
            sw.Close();
        }
    }
}