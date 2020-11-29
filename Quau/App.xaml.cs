using Quau.ViewModels;
using Quau.ViewModels.OptionsData;
using Quau.Views.OptionsData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Quau
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        MainWindowViewModel mainWindowViewModel;

        public App()
        {
            displayRootRegistry.RegisterWindowType<MainWindowViewModel, MainWindow>();
            displayRootRegistry.RegisterWindowType<OptionsDataViewModel, OptionsDataWindow>();
            displayRootRegistry.RegisterWindowType<DistributionDataViewModel, DistributionDataWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            mainWindowViewModel = new MainWindowViewModel();

            await displayRootRegistry.ShowModalPresentation(mainWindowViewModel);

            Shutdown();
        }
    }
}
