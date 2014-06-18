using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Newtonsoft.Googleman.ViewModels
{
    public abstract class ViewModelBase
    {
        public Window View { get; set; }

        protected internal virtual void Loaded()
        {
        }

        protected void Show()
        {
            View.Show();
        }

        protected void Close()
        {
            View.Close();
        }

        protected void Hide()
        {
            View.Hide();
        }
    }
}