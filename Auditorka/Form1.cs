using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auditorka
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите наименование задачи!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string taskName = textBox1.Text.Trim();
            DateTime startDate = monthCalendar1.SelectionStart;
            DateTime endDate = monthCalendar2.SelectionStart;

            if (startDate > endDate)
            {
                MessageBox.Show("Начальная дата не может быть позже конечной!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Задача"].Value != null && row.Cells["Задача"].Value.ToString() == taskName)
                {
                    MessageBox.Show("Задача с таким именем уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    return;
                }
            }

            dataGridView1.Rows.Add(taskName, "Не выполнено", startDate.ToShortDateString(), endDate.ToShortDateString());
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string taskName = textBox1.Text.Trim();

            bool taskFound = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Задача"].Value != null && row.Cells["Задача"].Value.ToString() == taskName)
                {
                    row.Cells["Состояние"].Value = "Выполнено";
                    taskFound = true;
                    break;
                }
            }

            if (!taskFound)
            {
                MessageBox.Show("Задача не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string taskName = textBox1.Text.Trim();

            bool taskFound = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Задача"].Value != null && row.Cells["Задача"].Value.ToString() == taskName)
                {
                    dataGridView1.Rows.Remove(row);
                    taskFound = true;
                    break;
                }
            }

            if (!taskFound)
            {
                MessageBox.Show("Задача не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DateTime endDate;
                if (row.Cells["End"].Value != null && DateTime.TryParse(row.Cells["End"].Value.ToString(), out endDate))
                {
                    if (endDate < DateTime.Today && row.Cells["Состояние"].Value.ToString() != "Выполнено")
                    {
                        row.Cells["Состояние"].Value = "Просрочено";
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //LoadFromFile

            if (textBox2.Text == "")
            {
                MessageBox.Show("Путь к файлу не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string filePath = textBox2.Text.Trim();

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл не существует или неверный путь к файлу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = File.ReadAllLines(filePath);

            dataGridView1.Rows.Clear();

            foreach (string line in lines)
            {
                string[] cells = line.Split(',');
                if (cells.Length == dataGridView1.Columns.Count)
                {
                    dataGridView1.Rows.Add(cells);
                }
                else
                {
                    MessageBox.Show("Некорректное количество столбцов в файле!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            MessageBox.Show("Таблица успешно загружена из файла", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //SaveFromFile

            if (textBox2.Text == "")
            {
                MessageBox.Show("Путь к файлу не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string filePath = textBox2.Text.Trim();

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл не существует или неверный путь к файлу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> lines = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string rowData = string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value));
                    lines.Add(rowData);
                }
            }

            File.WriteAllLines(filePath, lines);

            MessageBox.Show("Таблица успешно сохранена в файл", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
