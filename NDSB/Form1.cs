﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NDSB.SparseMethods;

namespace NDSB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadTrainBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            if (fdlg.ShowDialog() == DialogResult.OK)
                trainPathTbx.Text = fdlg.FileName;
        }

        private void loadTestBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            if (fdlg.ShowDialog() == DialogResult.OK)
                testPathTbx.Text = fdlg.FileName;
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            int nbNeighbours = Convert.ToInt32(nbNeighboursTbx.Text);

            Dictionary<string, double>[] trainPoints = CSRHelper.ImportPoints(trainPathTbx.Text);
            Dictionary<string, double>[] testPoints = CSRHelper.ImportPoints(testPathTbx.Text);
            int[] labels = DSCdiscountUtils.ReadLabels(labelsTbx.Text);

            string outfileName = Path.GetDirectoryName(trainPathTbx.Text) + "\\" + Path.GetFileNameWithoutExtension(trainPathTbx.Text) + "_knn_pred.txt";
            string[] predicted = new string[testPoints.Count()];

            for (int i = 0; i < trainPoints.Length; i++)
                trainPoints[i] = SparseVectorial.ToCube(trainPoints[i]);

            SparseKNNII.StampInverseDictionary(trainPoints, 0.5);

            Parallel.For(0, testPoints.Length, i =>
            {
                int[] pred = SparseKNNII.NearestNeighbours(labels, trainPoints, SparseVectorial.ToCube(testPoints[i]), nbNeighbours, SparseMetric.ManhattanDistance);
                predicted[i] = String.Join(";", pred);
            });
            File.AppendAllText(outfileName, String.Join(Environment.NewLine, predicted));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DSCdiscountUtils.TextToTFIDFCSR(testPathTbx.Text);
            DSCdiscountUtils.TextToTFIDFCSR(trainPathTbx.Text);
            DSCdiscountUtils.ExtractLabelsFromTraining(trainPathTbx.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            if (fdlg.ShowDialog() == DialogResult.OK)
                labelsTbx.Text = fdlg.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DownSample.Split(trainPathTbx.Text, Convert.ToInt32(maxOccurencesOfClassTbx.Text), DSCdiscountUtils.GetLabelCDiscountDB);
        }

        private void processBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(IntPtr.Size.ToString());
        }

        private void runPegasosBtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, double>[] trainPoints = CSRHelper.ImportPoints(trainPathTbx.Text);
            Dictionary<string, double>[] testPoints = CSRHelper.ImportPoints(testPathTbx.Text);
            int[] labels = DSCdiscountUtils.ReadLabels(labelsTbx.Text);

            string outfileName = Path.GetDirectoryName(trainPathTbx.Text) + "\\" + Path.GetFileNameWithoutExtension(trainPathTbx.Text) + "_pegasos_pred.txt";


            SparseMulticlassPerceptron model = new SparseMulticlassPerceptron();
            model.Train(trainPoints, labels, 0.3);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dictionary<string, double>[] trainPoints = CSRHelper.ImportPoints(trainPathTbx.Text);
            Dictionary<string, double>[] testPoints = CSRHelper.ImportPoints(testPathTbx.Text);
            int[] labels = DSCdiscountUtils.ReadLabels(labelsTbx.Text);

            string outfileName = Path.GetDirectoryName(trainPathTbx.Text) + "\\" + Path.GetFileNameWithoutExtension(trainPathTbx.Text) + "_rocchio_pred.txt";

            SparseCentroids sr = new SparseCentroids();
            sr.Train(labels, trainPoints);

            string[] predicted = new string[testPoints.Count()];
            Parallel.For(0, testPoints.Length, i =>
            //for(int i =0; i < testPoints.Length; i++)
            {
                int pred = sr.Predict(testPoints[i]);
                predicted[i] = String.Join(";", pred);
            });
            File.AppendAllText(outfileName, String.Join(Environment.NewLine, predicted));

        }
    }
}
