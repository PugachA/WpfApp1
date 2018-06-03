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
using Microsoft.Win32;
using System.IO;
using System.ComponentModel; // CancelEventArgs

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        string File_Name = ""; //храним имя файла для сохранения
        string File_Format = ""; //храним формат файла для сохранения
        bool flag_change = false; //поднимается когда в файл вносятся изменения
        bool flag_check = true; //флаг для проверки орфографии

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (flag_change)
            {
                MessageBoxResult result = MessageBox.Show("Файл не сохранён или изменения в файле не были сохранены. Сохранить?",
                                                      "Текстовый редактор",
                                                      MessageBoxButton.YesNoCancel,
                                                      MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    Open_File(); //открываем файл
                }
                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(sender, e);
                    Open_File(); //открываем файл
                }

            }
            else
            {
                Open_File(); //открываем файл
            }
        }
        public void Open_File() //вспомогательный метод для открытия файла
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*"; //форматы загружаемых файлов

            if (ofd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(rchTxtBox.Document.ContentStart, rchTxtBox.Document.ContentEnd); //создаем контейнер для документа

                //открытие файла
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                {
                    if (Path.GetExtension(ofd.FileName).ToLower() == ".rtf")
                    {
                        doc.Load(fs, DataFormats.Rtf);
                        File_Format = DataFormats.Rtf;
                    }
                    else if (Path.GetExtension(ofd.FileName).ToLower() == ".txt")
                    {
                        doc.Load(fs, DataFormats.Text);
                        File_Format = DataFormats.Text;
                    }
                    else
                    {
                        doc.Load(fs, DataFormats.Xaml);
                        File_Format = DataFormats.Xaml;
                    }
                    File_Name = ofd.FileName;
                    flag_change = false;
                }
            }
        }


        public void Save_Click(object sender, RoutedEventArgs e)
        {
            TextRange doc = new TextRange(rchTxtBox.Document.ContentStart, rchTxtBox.Document.ContentEnd); //создаем контейнер для документа

            if (File_Name == "")
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*"; //форматы сохранения файлов
                if (sfd.ShowDialog() == true)
                {
                    using (FileStream fs = File.Create(sfd.FileName))
                    {
                        if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                        {
                            doc.Save(fs, DataFormats.Rtf);
                            File_Format = DataFormats.Rtf;
                        }
                        else if (Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                        {
                            doc.Save(fs, DataFormats.Text);
                            File_Format = DataFormats.Text;
                        }
                        else
                        {
                            doc.Save(fs, DataFormats.Xaml);
                            File_Format = DataFormats.Xaml;
                        }
                        File_Name = sfd.FileName;
                        flag_change = false;
                    }
                }
            }
            else
            {
                using (FileStream fs = File.Create(File_Name))
                {
                    doc.Save(fs, File_Format);
                    flag_change = false;
                }
                    
            }
        }
        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            flag_change = true;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {

            if (flag_change)
            {
                MessageBoxResult result = MessageBox.Show("Файл не сохранён или изменения в файле не были сохранены. Сохранить?",
                                                      "Текстовый редактор",
                                                      MessageBoxButton.YesNoCancel,
                                                      MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    rchTxtBox.Document.Blocks.Clear(); //очищаем richtextbox
                    flag_change = false;
                }
                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(sender, e);
                    rchTxtBox.Document.Blocks.Clear(); //очищаем richtextbox
                    flag_change = false;
                }
                   
            }
            else
            {
                rchTxtBox.Document.Blocks.Clear(); //очищаем richtextbox
                flag_change = false;
            }
              
        }

        private void Save_As_Click(object sender, RoutedEventArgs e) //сохранить как
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(rchTxtBox.Document.ContentStart, rchTxtBox.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                    {
                        doc.Save(fs, DataFormats.Rtf);
                        File_Format = DataFormats.Rtf;
                    }
                    else if (Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                    {
                        doc.Save(fs, DataFormats.Text);
                        File_Format = DataFormats.Text;
                    }
                    else
                    {
                        doc.Save(fs, DataFormats.Xaml);
                        File_Format = DataFormats.Xaml;
                    }
                    File_Name = sfd.FileName;
                    flag_change = false;
                }
            }
        }
        void DataWindow_Closing(object sender, CancelEventArgs e) //обработка закрытия приложения
        {
            if (flag_change)
            {
                MessageBoxResult result = MessageBox.Show("Файл не сохранён или изменения в файле не были сохранены. Сохранить?",
                                                      "Текстовый редактор",
                                                      MessageBoxButton.YesNoCancel,
                                                      MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    Application.Current.Shutdown(); //закрываем приложение
                }
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true; //закрываем окно сообщения
                }
                if (result == MessageBoxResult.Yes)
                {
                    TextRange doc = new TextRange(rchTxtBox.Document.ContentStart, rchTxtBox.Document.ContentEnd); //создаем контейнер для документа

                    if (File_Name == "")
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*"; //форматы сохранения файлов
                        if (sfd.ShowDialog() == true)
                        {
                            using (FileStream fs = File.Create(sfd.FileName))
                            {
                                if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                                {
                                    doc.Save(fs, DataFormats.Rtf);
                                    File_Format = DataFormats.Rtf;
                                }
                                else if (Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                                {
                                    doc.Save(fs, DataFormats.Text);
                                    File_Format = DataFormats.Text;
                                }
                                else
                                {
                                    doc.Save(fs, DataFormats.Xaml);
                                    File_Format = DataFormats.Xaml;
                                }
                                File_Name = sfd.FileName;
                                flag_change = false;
                                Application.Current.Shutdown(); //закрываем приложение
                            }
                        }
                    }
                    else
                    {
                        using (FileStream fs = File.Create(File_Name))
                        {
                            doc.Save(fs, File_Format);
                            flag_change = false;
                        }
                        Application.Current.Shutdown(); //закрываем приложение

                    }
                }

            }
            else
            {
                Application.Current.Shutdown(); //закрываем приложение
            }
        }
        private void ColorBack_Checked(object sender, RoutedEventArgs e)
        {
                rchTxtBox.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(System.Windows.Media.Colors.Yellow));//изменение цвета фона на желтый
            ColorBack.Background = Brushes.Yellow;
            
        }
        private void ColorBack_Unchecked(object sender, RoutedEventArgs e)
        {
            rchTxtBox.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(System.Windows.Media.Colors.White));//изменение цвета фона обратно на белый
            ColorBack.Background = Brushes.LightYellow;
        }
        private void ColorFore_Checked(object sender, RoutedEventArgs e)
        {
            rchTxtBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(System.Windows.Media.Colors.Red));//изменение цвета символов на красный

        }
        private void ColorFore_Unchecked(object sender, RoutedEventArgs e)
        {
            rchTxtBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(System.Windows.Media.Colors.Black));//изменение цвета символов обратно на черный
        }
        private void SpellCheck_Click(object sender, RoutedEventArgs e)//проверка орфографии
        {
            if(flag_check)
            {
                rchTxtBox.SpellCheck.IsEnabled = true;//включение проверки орфографии
                flag_check = !flag_check;
            }
            else
            {
                rchTxtBox.SpellCheck.IsEnabled = false;//выключение проверки орфографии
                flag_check = !flag_check;
            }
            
        }
        private void Up_Click(object sender, RoutedEventArgs e)
        {
            rchTxtBox.Selection.Text = rchTxtBox.Selection.Text.ToUpper();//приведение к верхнему регистру
        }

        private void Low_Click(object sender, RoutedEventArgs e)
        {
            rchTxtBox.Selection.Text = rchTxtBox.Selection.Text.ToLower();//приведение к нижнему регистру
        }

        private void Print_Click(object sender, RoutedEventArgs e)//печать
        {
            PrintDialog pd = new PrintDialog();
            if ((pd.ShowDialog() == true))
            {      
                pd.PrintVisual(rchTxtBox as Visual, "printing as visual");
               // pd.PrintDocument((((IDocumentPaginatorSource)rchTxtBox.Document).DocumentPaginator), "printing as paginator");
            }
        }
    }

}
