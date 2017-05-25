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
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Starukhin_KDZ
{


    public partial class MainWindow : Window
    {
        List<Stars> starslist = new List<Stars>();
        List<Galaxy> galaxylist = new List<Galaxy>();
        List<string> galaxytype = new List<string>() { "Эллиптическая", "Спиральная", "Линзовидная", "Неправильная" };
        BinaryFormatter formatter = new BinaryFormatter();

        public MainWindow()
        {

            InitializeComponent();

            using (FileStream fs = new FileStream("stars.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                {
                    starslist = (List<Stars>)formatter.Deserialize(fs);
                }
            }

            using (FileStream fs = new FileStream("galaxies.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                {
                    galaxylist = (List<Galaxy>)formatter.Deserialize(fs);
                }
            }

            StarsRefresh(starslist);
            GalaxyRefresh(galaxylist);
            GalaxyTypebox.ItemsSource = galaxytype;

        }

        private bool CheckIfDouble(string input)
        {
            double output;
            if (double.TryParse(input, out output))
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        private bool CheckInputStars()
        {
            if (Namebox.Text == "" || Distancebox.Text == "" || Radiusbox.Text == "" || Brightnessbox.Text == "")
            {
                MessageBox.Show("Требуется заполнить все поля", "Некоторые поля незаполнены",
              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            else if (!CheckIfDouble(Distancebox.Text) || !CheckIfDouble(Radiusbox.Text) || double.Parse(Distancebox.Text) < 0 || double.Parse(Radiusbox.Text) < 0 || !CheckIfDouble(Brightnessbox.Text) || double.Parse(Radiusbox.Text) < 0)
            {
                MessageBox.Show("Неверный формат вводимых данных", "Неверный формат вводимых данных",
              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            { return true; }

        }
        private bool CheckInputGalaxy()
        {
            if (GalaxyNamebox.Text == "" || GalaxyDistancebox.Text == "" || GalaxyRadiusbox.Text == "" || GalaxyTypebox.SelectedItem == null)
            {
                MessageBox.Show("Требуется заполнить все поля", "Некоторые поля незаполнены",
              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            else if (!CheckIfDouble(GalaxyDistancebox.Text) || !CheckIfDouble(GalaxyRadiusbox.Text) || double.Parse(GalaxyDistancebox.Text) < 0 || double.Parse(GalaxyRadiusbox.Text) < 0)
            {
                MessageBox.Show("Неверный формат вводимых данных", "Неверный формат вводимых данных",
              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            { return true; }

        }

        private void StarsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int i = StarsBox.SelectedIndex;
                Namebox.Text = starslist[i]._name;
                Distancebox.Text = starslist[i]._distance.ToString();
                Radiusbox.Text = starslist[i]._radius.ToString();
                Brightnessbox.Text = starslist[i]._brightness.ToString();
            }
            catch
            { }
        }

        private void GetGalaxyInfo(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int i = GalaxyBox.SelectedIndex;
                GalaxyNamebox.Text = galaxylist[i]._name;
                GalaxyDistancebox.Text = galaxylist[i]._distance.ToString();
                GalaxyRadiusbox.Text = galaxylist[i]._radius.ToString();

                for (int g = 0; i < 4; i++)
                {
                    if (galaxylist[i]._type == galaxytype[g])
                    {
                        GalaxyTypebox.Text = galaxytype[g].ToString();
                    }
                }

            }
            catch { }

        }
        private void Delete_Galaxy(object sender, RoutedEventArgs e)
        {
            int i = GalaxyBox.SelectedIndex;
            if (i != -1)
            {
                galaxylist.Remove(galaxylist[i]);
                GalaxyRefresh(galaxylist);
                GalaxyNamebox.Text = "";
                GalaxyDistancebox.Text = "";
                GalaxyRadiusbox.Text = "";
                GalaxyTypebox.SelectedIndex = -1;

                GalaxyRefresh(galaxylist);
            }
            else
            {
                MessageBox.Show("Выберете желаемый объект из списка слева", "Объект не выбран",
              MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void Delete_Star(object sender, RoutedEventArgs e)
        {
            int i = StarsBox.SelectedIndex;
            if (i != -1)
            {

                starslist.Remove(starslist[i]);
                StarsRefresh(starslist);
                Namebox.Text = "";
                Distancebox.Text = "";
                Radiusbox.Text = "";
                Brightnessbox.Text = "";
            }
            else
            {
                MessageBox.Show("Выберете желаемый объект из списка слева", "Объект не выбран",
              MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void StarsRefresh(List<Stars> starslist)
        {
            List<Stars> clear = new List<Stars>();
            StarsBox.ItemsSource = clear;
            StarsBox.DisplayMemberPath = "_name";
            StarsBox.ItemsSource = starslist;
            StarsBox.DisplayMemberPath = "_name";
        }

        private void GalaxyRefresh(List<Galaxy> galaxylist)
        {
            List<Galaxy> clear = new List<Galaxy>();
            GalaxyBox.ItemsSource = clear;
            GalaxyBox.DisplayMemberPath = "_name";
            GalaxyBox.ItemsSource = galaxylist;
            GalaxyBox.DisplayMemberPath = "_name";
        }

        private void EditGalaxyInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = GalaxyBox.SelectedIndex;
                if (i != -1)
                {
                    if (CheckInputGalaxy())
                    {

                        galaxylist[i]._distance = double.Parse(GalaxyDistancebox.Text);
                        galaxylist[i]._radius = double.Parse(GalaxyRadiusbox.Text);
                        galaxylist[i]._type = GalaxyTypebox.SelectedItem.ToString();
                        Namebox.Text = galaxylist[i]._name;
                        GalaxyRefresh(galaxylist);
                    }

                }
                else
                {
                    MessageBox.Show("Выберете желаемый объект из списка слева", "Объект не выбран",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {

            }
        }
        private void EditStarInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = StarsBox.SelectedIndex;
                if (i != -1)
                {
                    if (CheckInputStars())
                    {

                        starslist[i]._distance = double.Parse(Distancebox.Text);
                        starslist[i]._radius = double.Parse(Radiusbox.Text);
                        starslist[i]._brightness = double.Parse(Brightnessbox.Text);
                        Namebox.Text = starslist[i]._name;
                        StarsRefresh(starslist);
                    }

                }
                else
                {
                    MessageBox.Show("Выберете желаемый объект из списка слева", "Объект не выбран",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            catch
            {

            }
        }
        private bool CheckIfExistsGalaxy(string name, List<Galaxy> galaxylist)
        {
            foreach (Galaxy item in galaxylist)
            {
                if (name == item._name)
                {
                    MessageBox.Show("Такой объект уже существует", "Объект с таким именем уже существует",
            MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }

        private bool CheckIfExistsStar(string name, List<Stars> starslist)
        {
            foreach (Stars item in starslist)
            {
                if (name == item._name)
                {
                    MessageBox.Show("Такой объект уже существует", "Объект с таким именем уже существует",
            MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }
        private void AddGalaxy_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfExistsGalaxy(GalaxyNamebox.Text, galaxylist) && CheckInputGalaxy())
            {
                Galaxy NewGalaxy = new Galaxy(GalaxyNamebox.Text, double.Parse(GalaxyDistancebox.Text), double.Parse(GalaxyRadiusbox.Text), GalaxyTypebox.SelectedItem.ToString());
                GalaxyNamebox.Text = "";
                GalaxyDistancebox.Text = "";
                GalaxyRadiusbox.Text = "";
                GalaxyTypebox.SelectedIndex = -1;
                galaxylist.Add(NewGalaxy);
                GalaxyRefresh(galaxylist);
            }
        }

        private void AddStar_Click(object sender, RoutedEventArgs e)
        {

            if (CheckIfExistsStar(Namebox.Text, starslist) && CheckInputStars())
            {
                Stars Newstar = new Stars(Namebox.Text, double.Parse(Distancebox.Text), double.Parse(Radiusbox.Text), double.Parse(Brightnessbox.Text));
                Namebox.Text = "";
                Distancebox.Text = "";
                Radiusbox.Text = "";
                Brightnessbox.Text = "";
                starslist.Add(Newstar);
                StarsRefresh(starslist);
            }
        }



        private void SaveAndExit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (FileStream fs = new FileStream("stars.dat", FileMode.OpenOrCreate))
            {

                formatter.Serialize(fs, starslist);
            }

            using (FileStream fs = new FileStream("galaxies.dat", FileMode.OpenOrCreate))
            {

                formatter.Serialize(fs, galaxylist);
            }
        }


    }
}

