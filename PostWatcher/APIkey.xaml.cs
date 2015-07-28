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
using System.Windows.Shapes;
using System.Xml;
using Microsoft.Win32;

namespace PostWatcher
{
    /// <summary>
    /// Interaction logic for APIkey.xaml
    /// </summary>
    public partial class APIkey : Window
    {
        private string _APIkey;
        public APIkey()
        {
            InitializeComponent();

            var rk = Registry.CurrentUser.OpenSubKey("PostWatcher");

            if (rk == null)
            {
                rk = Registry.CurrentUser.CreateSubKey("PostWatcher");
            }
            else
            {
                var value = rk.GetValue("API key");
                if (value != null)
                    tb_input.Text = (string) value;

            }


        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _APIkey = tb_input.Text;
            
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("PostWatcher", true);
                rk.SetValue("API key", _APIkey);
              
        
              Close();
          

        }
    }
}
