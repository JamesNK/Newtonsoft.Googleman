using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Newtonsoft.Googleman.ViewModels;

namespace Newtonsoft.Googleman.Common
{
    public class WindowManager
    {
        public static WindowManager Instance { get; set; }

        private readonly IDictionary<Type, Type> _viewMappings;

        public WindowManager(IDictionary<Type, Type> viewMappings)
        {
            _viewMappings = viewMappings;
        }

        private Window CreateView(ViewModelBase viewModel)
        {
            Type viewType = _viewMappings[viewModel.GetType()];

            Window view = (Window)Activator.CreateInstance(viewType);
            viewModel.View = view;

            view.DataContext = viewModel;
            view.Loaded += (sender, args) => viewModel.Loaded();
            return view;
        }

        public void ShowWindow(ViewModelBase viewModel)
        {
            Window view = CreateView(viewModel);
            view.Show();
        }

        public void ShowDialog(ViewModelBase viewModel)
        {
            Window view = CreateView(viewModel);
            view.ShowDialog();
        }
    }
}