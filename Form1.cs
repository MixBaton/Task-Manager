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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Проектик
{
    public partial class Form1: Form
    {


        private const string FileName = "tasks.txt"; //создаем файл, в который будут сохраняться значения из listBox 
        string time;
        string remindTime;
        
        public Form1()
        {
            InitializeComponent();
            LoadTasks(); // Загружаем задачи при открытии формы
        }
     

        private void buttonAdd_Click(object sender, EventArgs e)
        {

            remindTime = maskedTime.Text; //Задаем время выполнения 

            if ((string)textBoxName.Text != "") //проверка на пустое значение задачи
            {
                string newListElement = textBoxName.Text; //Задаем название задачи
                DateTime dueDate = dateTimePicker.Value;  //Задаем дату выполнения


                if (DateTime.Today <= dueDate) //Проверяем, больше ли заданная дата сегодняшней
                {

                    var newTask = new Task //Создаем новый объект с названием и датой
                    {
                        Title = newListElement,
                        DueDate = dueDate
                    };

                    listBox.Items.Add(newTask + " Время: " + remindTime); //Сохраняем значение в listBox 
                    textBoxName.Clear(); //Очищаем поля ввода
                    maskedTime.Clear();
                }
                else
                {
                    MessageBox.Show("Дата выполнения должна быть сегодня или позже.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
       
        private void SaveTasks() //Создаем функцию для сохранения задач
        {
            using (StreamWriter writer = new StreamWriter(FileName))  //работа с файлом
            {
                foreach (var item in listBox.Items) //Проверим задачи из listBox
                {
                    writer.WriteLine(item.ToString()); // Сохраняем каждую задачу в файл
                }
            }
        }

        private void LoadTasks()  //Выявление сохраненных задач на форме
        {
            if (File.Exists(FileName))
            {
                using (StreamReader reader = new StreamReader(FileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)  //чтение строк из файла
                    {
                        listBox.Items.Add(line); // Загружаем каждую задачу из файла
                    }
                }
            }
        }


        private void buttonRem_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null) //проверка задачи на null
            {
                listBox.Items.Remove(listBox.SelectedItem); //удаление выбранной задачи
            }

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            //Сщздание диалогового окна
            DialogResult result = MessageBox.Show(
        "Удалить все задачи из списка?",
        "Вопрос на миллион",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);

            this.TopMost = true;

            if (result == DialogResult.Yes) //при утвердительном ответе, удалить все значения
                listBox.Items.Clear();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();  //Функция закрытия формы
            SaveTasks();  //Сохранить после закрытия
        }
       
        private void timer1_Tick(object sender, EventArgs e)

        {
            time = DateTime.Now.ToString("HH;mm");  //таймер
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = textBoxSearch.Text.ToLower(); // Получаем поисковый запрос

            // Очищаем выделение в ListBox
            listBox.ClearSelected();

            // Перебираем элементы в ListBox
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                // Сравниваем элемент с поисковым запросом
                if (listBox.Items[i].ToString().ToLower().Contains(searchTerm))
                {
                    listBox.SelectedIndex = i; // Выделяем найденный элемент
                    break; // Выходим из цикла после первого найденного элемента
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }

    public class Task  //Создаем класс таск
    {
        public string Title { get; set; }  //Значение для названия
        public DateTime DueDate { get; set; } //Значение для даты

        public override string ToString()  //Выводим значение
        {
            return $"{Title} (Дата: {DueDate.ToShortDateString()})";
        }
    }
}
