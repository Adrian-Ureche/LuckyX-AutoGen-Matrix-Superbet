using LuckyX__AutoGen;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuckySix___C_sharp
{
    public partial class Main : Form
    {
        private IWebDriver driver = new FirefoxDriver();
        static Random random = new Random();
        List<ElMatrix> elMatrixList = new List<ElMatrix>();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            driver.Url = "https://superbet.ro/virtuale/luckyx";
            generateMatrix();
            genInit21TextBox();
        }

        private void generateMatrix()
        {
            const int rows = 5;
            const int cols = 5;
            const int groupBoxWidth = 80;
            const int groupBoxHeight = 30;
            const int spacing = 5;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    GroupBox groupBox = new GroupBox();
                    groupBox.Text = $"M {i}{j}";
                    groupBox.Width = groupBoxWidth;
                    groupBox.Height = groupBoxHeight;
                    groupBox.Left = j * (groupBoxWidth + spacing);
                    groupBox.Top = i * (groupBoxHeight + spacing);

                    TextBox tbQuantity = new TextBox();
                    tbQuantity.Width = 30;
                    tbQuantity.Left = 0;
                    tbQuantity.Top = 10;
                    tbQuantity.TextChanged += (sender, e) => tbQuantity_TextChanged(sender, e, groupBox.Text, tbQuantity.Text);

                    TextBox tbLength = new TextBox();
                    tbLength.Width = 30;
                    tbLength.Left = 50;
                    tbLength.Top = 10;
                    tbLength.TextChanged += (sender, e) => tbLength_TextChanged(sender, e, groupBox.Text, tbLength.Text);

                    Button button = new Button();
                    button.Text = "*";
                    button.Width = 20;
                    button.Left = 30;
                    button.Top = 10;
                    button.Click += (sender, e) => createElMatrix(sender, e, groupBox.Text, tbQuantity, tbLength);

                    groupBox.Controls.Add(tbQuantity);
                    groupBox.Controls.Add(button);
                    groupBox.Controls.Add(tbLength);

                    gbMatrix.Controls.Add(groupBox);
                }
            }
        }

        private void createElMatrix(object sender, EventArgs e, string groupBoxText, TextBox tbQuantity, TextBox tbLength)
        {
            elMatrixList.Add(new ElMatrix(groupBoxText, tbQuantity.Text, tbLength.Text, ref tvList));
        }

        private void tbQuantity_TextChanged(object sender, EventArgs e, string groupBoxText, string newQuantity)
        {
            ElMatrix elMatrixToUpdate = elMatrixList.FirstOrDefault(em => em.Id == groupBoxText);

            if (elMatrixToUpdate != null)
            {
                elMatrixToUpdate.Quantity = newQuantity;
            }
        }

        private void tbLength_TextChanged(object sender, EventArgs e, string groupBoxText, string newLength)
        {
            ElMatrix elMatrixToUpdate = elMatrixList.FirstOrDefault(em => em.Id == groupBoxText);

            if (elMatrixToUpdate != null)
            {
                elMatrixToUpdate.Length = newLength;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = comboBox1.Text;
            listView1.Items.Clear();
            if (File.Exists(@"Fisiere\" + comboBox1.Text + ".adr"))
                loadSolutions(@"Fisiere\" + comboBox1.Text + ".adr");
        }

        void loadSolutions(string path)
        {
            int counter = 0;
            string line;
            string[] solution;

            System.IO.StreamReader file =
                new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                solution = line.Split(',');
                var listViewItem = new ListViewItem(solution);
                listView1.Items.Add(listViewItem);
                counter++;
            }

            label2.Text = counter.ToString();
            file.Close();
        }

        private void genInit21TextBox()
        {
            // Dimensiunile și poziționarea inițială a primului TextBox
            int textBoxWidth = 30;
            int textBoxHeight = 20;
            int spacing = 5;
            int startX = 10;
            int startY = 10;

            int textBoxCount = 20;
            int textBoxsPerRow = 10;
            int rows = (int)Math.Ceiling((double)textBoxCount / textBoxsPerRow);

            for (int i = 0; i < textBoxCount; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Width = textBoxWidth;
                textBox.Height = textBoxHeight;

                // Calcularea poziției TextBox-ului pe ecran
                int row = i / textBoxsPerRow;
                int col = i % textBoxsPerRow;
                int x = startX + (textBoxWidth + spacing) * col;
                int y = startY + (textBoxHeight + spacing) * row;
                
                textBox.PreviewKeyDown += TextBox_PreviewKeyDown; // Adăugați evenimentul KeyPress pentru fiecare TextBox

                textBox.Location = new System.Drawing.Point(x, y);
                gb21Tb.Controls.Add(textBox);

                // Adaugare al 21-lea TextBox pe al doilea rând, la final, după al 20-lea
                if (i == textBoxCount - 1)
                {
                    TextBox textBox21 = new TextBox();
                    textBox21.Width = textBoxWidth;
                    textBox21.Height = textBoxHeight;

                    int x21 = startX + (textBoxWidth + spacing) * (col + 1);
                    int y21 = startY + (textBoxHeight + spacing) * row;

                    textBox21.PreviewKeyDown += TextBox_PreviewKeyDown; // Adăugați evenimentul KeyPress pentru al 21-lea TextBox

                    textBox21.Location = new System.Drawing.Point(x21, y21);
                    gb21Tb.Controls.Add(textBox21);
                }
            }
        }

        private void TextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Tab)
            {
                TextBox currentTextBox = sender as TextBox;

                // Verificăm dacă textul din TextBox-ul curent se regăsește în alt TextBox
                foreach (Control control in gb21Tb.Controls)
                {
                    if (control is TextBox otherTextBox && otherTextBox != currentTextBox)
                    {
                        if (currentTextBox.Text == otherTextBox.Text)
                        {
                            // Ștergeți textul din TextBox-ul curent
                            currentTextBox.Text = "";

                            // Setăm focus pe TextBox-ul curent
                            currentTextBox.Focus();
                            keepCurrentTextBoxFocused(currentTextBox);
                        }
                    }
                }
            }
        }

        private void keepCurrentTextBoxFocused(TextBox currentTextBox)
        {
            // Obținem indexul TextBox-ului curent în lista de controale din gb20Tb
            int currentIndex = gb21Tb.Controls.IndexOf(currentTextBox);
            if (currentIndex > 0)
            {
                TextBox prevTextBox = gb21Tb.Controls[currentIndex - 1] as TextBox;
                prevTextBox.Focus();
            }
        }

        private void BtnStergeGrid_Click(object sender, EventArgs e)
        {
            if (tvList.SelectedNode != null)
            {
                elMatrixList.RemoveAll(element => element.Id == tvList.SelectedNode.Text);
                tvList.Nodes.Remove(tvList.SelectedNode);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                IWebElement src = this.driver.FindElement(By.XPath("//iframe[@id='seven-plugin-live']"));
                driver.Url = src.GetProperty("src");
            }
            catch
            {
                MessageBox.Show("Protectia nu a fost scoasa! Verificati daca in pagina din Mozila este deschis Lucky Six din pariuri virtuale. Daca nu este, intrati manual din Mozila pe Lucky Six (Virtuale) si nu uitati sa va autentificati contul.");
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                generateSolutionFiles();
                BrowserView browserView = new BrowserView(this.driver);
                browserView.pariaza(tbCost.Text, tbUrm.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ceva nu a mers bine! \nEroare: " + ex.ToString() + "\n\nMesajul erorii: " + ex.Message);
            }
        }

        private void generateSolutionFiles()
        {
            int gNumber = 1;
            int gCurrentQuantity = 0;

            if (Directory.Exists("Fisiere"))
            {
                Directory.Delete("Fisiere", true);
                Directory.CreateDirectory("Fisiere");
            }
            else
            {
                Directory.CreateDirectory("Fisiere");
            }

            foreach (ElMatrix elMatrix in elMatrixList)
            {
                List<List<string>> solutions = generateSolutions(int.Parse(elMatrix.Quantity), int.Parse(elMatrix.Length));
                foreach (List<string> solution in solutions)
                {
                    writeListOfNumbersInFile(solution, @"Fisiere\G" + gNumber.ToString() + ".adr");
                    gCurrentQuantity++;
                    if (gCurrentQuantity % 10 == 0)
                    {
                        gNumber++;
                        gCurrentQuantity = 0;
                    }
                }
            }
        }

        private List<List<string>> generateSolutions(float quantity, int length)
        {
            List<List<string>> result = new List<List<string>>();

            if (cbAuto.Checked)
            {
                result = generateSolutionsAuto(quantity, length);
            }
            else
            {
                List<string> mandatoryNumbers = new List<string>();

                if (cbObligatoriu.Checked)
                {
                    mandatoryNumbers = getMandatoryNumbers();
                }

                if (cbAleatoriu1.Checked)
                {
                    result = generateSoultionBasedOnFirstAleatoriu(tbAleatoriu1.Text, quantity, length, mandatoryNumbers);
                    return result;
                }
                if (cbAleatoriu2.Checked)
                {
                    result = generateSoultionBasedOnFirstAleatoriu(tbAleatoriu2.Text, quantity, length, mandatoryNumbers);
                    return result;
                }
                if (cbAleatoriu3.Checked)
                {
                    result = generateSoultionBasedOnFirstAleatoriu(tbAleatoriu3.Text, quantity, length, mandatoryNumbers);
                    return result;
                }
                if (cbAleatoriu4.Checked)
                {
                    result = generateSoultionBasedOnFirstAleatoriu(tbAleatoriu4.Text, quantity, length, mandatoryNumbers);
                    return result;
                }

                result = generateSolutinBasedOnGroupsAandB(quantity, length, mandatoryNumbers);
            }

            return result;
        }

        private List<List<string>> generateSolutionsAuto(float quantity, int length)
        {
            List<List<string>> result = new List<List<string>>();
            List<string> allTexts = new List<string>();

            allTexts = rtbAuto.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

            for (int i = 0; i < quantity; i++)
            {
                Shuffle(allTexts);
                List<string> sublist = allTexts.Take(length).ToList();
                result.Add(sublist);
            }

            return result;
        }

        private List<string> getMandatoryNumbers()
        {
            List<string> mandatoryNumbers = new List<string>();

            if (tbObligatoriu1.Text != "")
            {
                mandatoryNumbers.Add(tbObligatoriu1.Text);
            }
            if (tbObligatoriu2.Text != "")
            {
                mandatoryNumbers.Add(tbObligatoriu2.Text);
            }
            if (tbObligatoriu3.Text != "")
            {
                mandatoryNumbers.Add(tbObligatoriu3.Text);
            }
            if (tbObligatoriu4.Text != "")
            {
                mandatoryNumbers.Add(tbObligatoriu4.Text);
            }

            return mandatoryNumbers;
        }

        private List<List<string>> generateSoultionBasedOnFirstAleatoriu(string aleatoriu, float quantity, int length, List<string> mandatoryNumbers)
        {
            List<List<string>> result = new List<List<string>>();
            int mandatoryNumbersSize = mandatoryNumbers.Count();

            List<string> allTextsAleatoriu = gb21Tb.Controls
                .OfType<TextBox>()
                .Take(int.Parse(aleatoriu))
                .Where(tb => !string.IsNullOrEmpty(tb.Text))
                .Select(tb => tb.Text)
                .ToList();

            for (int i = 0; i < quantity; i++)
            {
                Shuffle(allTextsAleatoriu);
                List<string> sublist = allTextsAleatoriu.Take(length - mandatoryNumbersSize).ToList();
                sublist.AddRange(mandatoryNumbers);
                result.Add(sublist);
            }

            return result;
        }

        private List<List<string>> generateSolutinBasedOnGroupsAandB(float quantity, int length, List<string> mandatoryNumbers)
        {
            List<List<string>> result = new List<List<string>>();

            List<string> allTextsA = gb21Tb.Controls
                .OfType<TextBox>()
                .Take(10)
                .Where(tb => !string.IsNullOrEmpty(tb.Text))
                .Select(tb => tb.Text)
                .ToList();

            List<string> allTextsB = gb21Tb.Controls
                .OfType<TextBox>()
                .Where(tb => !string.IsNullOrEmpty(tb.Text))
                .Select(tb => tb.Text)
                .ToList();

            for (int i = 0; i < quantity; i++)
            {
                if (length < 9)
                {
                    if (i < (quantity / 2))
                    {
                        Shuffle(allTextsA);
                        List<string> sublist = allTextsA.Take(length - mandatoryNumbers.Count).ToList();
                        sublist.AddRange(mandatoryNumbers);
                        result.Add(sublist);
                    }
                    else
                    {
                        Shuffle(allTextsB);
                        List<string> sublist = allTextsB.Take(length - mandatoryNumbers.Count).ToList();
                        sublist.AddRange(mandatoryNumbers);
                        result.Add(sublist);
                    }
                }
                else
                {
                    Shuffle(allTextsB);
                    List<string> sublist = allTextsB.Take(length - mandatoryNumbers.Count).ToList();
                    sublist.AddRange(mandatoryNumbers);
                    result.Add(sublist);
                }
            }

            return result;
        }

        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void writeListOfNumbersInFile(List<string> numbers, string filePath)
        {
            bool fileExists = File.Exists(filePath);

            using (StreamWriter writer = new StreamWriter(filePath, fileExists))
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                    writer.Write(numbers[i]);
                    if (i < numbers.Count - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();
            }
        }

        private void btnSterge20Nr_Click(object sender, EventArgs e)
        {
            gb21Tb.Controls.Clear();
            genInit21TextBox();
            if (Directory.Exists("Fisiere"))
            {
                Directory.Delete("Fisiere", true);
                label2.Text = "0";
                listView1.Items.Clear();
            }
        }

        private void btnStergeTLv_Click(object sender, EventArgs e)
        {
            if (File.Exists("Fisiere/" + comboBox1.Text + ".adr"))
            {
                File.Delete("Fisiere/" + comboBox1.Text + ".adr");
                label2.Text = "0";
                listView1.Items.Clear();
            }
        }
    }
}
