using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace oop1
{
    public partial class FormPaint : Form
    {
        protected Pen pen;
        public Graphics graphics;
        protected Bitmap image;
        protected List<Figure> figures;
        private bool recordingPoints = false;
        private Figure currFigure;
        private List<RadioButton> radioButtons;
        private string currDocumentPath = "";
        private bool isChahged = false;
        enum DialogOption
        {
            Open, Save
        }
        Color color;
        int penWidth;
        public FormPaint()
        {
            InitializeComponent();
            image = new Bitmap(pictureBoxMain.Width, pictureBoxMain.Height);
            graphics = Graphics.FromImage(image);
            color = Color.Black;
            penWidth = 2;
            pictureBoxMain.Image = image;
            figures = new List<Figure>();
            radioButtons = new List<RadioButton>();
            radioButtonLine.Tag = typeof(Line);
            radioButtonRect.Tag = typeof(Rect);
            radioButtonSquare.Tag = typeof(Square);
            radioButtonEllipse.Tag = typeof(Ellipse);
            radioButtonCircle.Tag = typeof(Circle);
            radioButtonPolygon.Tag = typeof(Polygon);
            radioButtons.Add(radioButtonLine);
            radioButtons.Add(radioButtonRect);
            radioButtons.Add(radioButtonSquare);
            radioButtons.Add(radioButtonEllipse);
            radioButtons.Add(radioButtonCircle);
            radioButtons.Add(radioButtonPolygon);
        }

        private Type GetFigureType()
        {
            Type result = null;
            foreach (RadioButton rb in radioButtons)
                if (rb.Checked)
                    result = (Type)rb.Tag;
            return result;
        }

        private void radioButtonClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            figures.Clear();
        }

        public void ClearForm() {
            graphics.Clear(pictureBoxMain.BackColor);
            Invalidate();
        }

        private void FormPaint_Paint(object sender, PaintEventArgs e)
        {
            pictureBoxMain.Refresh();
        }

        private Figure CreateFigure(Type figureType) {
            ConstructorInfo constructor = figureType.GetConstructors()[0];
            Figure figure = (Figure)constructor.Invoke(new object[] { color });
            //Figure figure = (Figure)System.Activator.CreateInstance(figureType);
            return figure;
        }

        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            Type figureType = GetFigureType();
            Figure figure;

            if (figureType == null)
                return;
           
            if (!recordingPoints)
            {
                figure = CreateFigure(figureType);
                figure.RecordPoints = true;
                currFigure = figure;
                figures.Add(figure);
                recordingPoints = currFigure.RecordPoints;
                isChahged = true;
            }
            currFigure.points.Add(new Point(e.X, e.Y));
        }

        private void RedrawAllFigures()
        {
            foreach (Figure fig in figures)
                fig.Draw(graphics);
            Invalidate();
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (recordingPoints)
            {
                currFigure.UpdateState(new Point(e.X, e.Y));
                ClearForm();
                RedrawAllFigures();
            }
        }

        private void pictureBoxMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (currFigure != null)
                recordingPoints = currFigure.RecordPoints;            
        }

        private string GetFileName(DialogOption option) {
            DialogResult dlgResult = DialogResult.Abort;
            string result = "";
            switch (option)
            {
                case DialogOption.Open:
                    dlgResult = openFileDialog.ShowDialog();
                    result = openFileDialog.FileName;
                    break;
                case DialogOption.Save:
                    dlgResult = saveFileDialog.ShowDialog();
                    result = saveFileDialog.FileName;
                    break;
            }
            if (dlgResult == DialogResult.Abort || dlgResult == DialogResult.Cancel)
                return "";
            else return result;
        }

        private void Serialize(string filename)
        {       
            if (filename != "")
                SerializeAll(filename);
        }

        private void Serialize()
        {
            string filename = GetFileName(DialogOption.Save);
            Serialize(filename);
        }

        private void SerializeAll(string filename)
        {
            var formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, figures);
            }
            isChahged = false;
        }

        private void DeserializeAll(string filename)
        {
            var formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                figures = (List<Figure>)formatter.Deserialize(fs);
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = DialogResult.Cancel;
            while (isChahged && !(dlgResult == DialogResult.No)) {
                dlgResult = AskForSavingChanges();
                switch (dlgResult)
                {
                    case DialogResult.Cancel: return;
                    case DialogResult.No: break;
                }
            }
                
            ClearForm();
            string filename = GetFileName(DialogOption.Open);
            if (filename != "")
                DeserializeAll(filename);
            RedrawAllFigures();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = GetFileName(DialogOption.Save);
            Serialize(filename);
        }

        private DialogResult AskForSavingChanges()
        {
            DialogResult dlgResult = DialogResult.Cancel;
            if (isChahged)
                dlgResult = MessageBox.Show(
                    "There are unsaved changes of your brilliant work. Save them?",
                    "Save?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button3
                    );
            if (dlgResult == DialogResult.Yes)
                Serialize();
            return dlgResult;
        }

        private void FormPaint_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dlgResult = DialogResult.Cancel;
            while (isChahged && dlgResult != DialogResult.No)
            {
                dlgResult = AskForSavingChanges();
                if (dlgResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currDocumentPath == "")
                Serialize();
            else Serialize(currDocumentPath);
        }

        private void buttonChooseColor_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            color = colorDialog.Color;
            buttonChooseColor.BackColor = colorDialog.Color;
        }
    }
}
