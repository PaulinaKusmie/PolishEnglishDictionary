using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Data.SqlClient;

namespace PolishEnglishDictionary.ViewModel
{
     class MainPageViewModel : INotifyPropertyChanged
    {
        public ICommand StartLearningCommand { get; set; }
        public ICommand StudyProgressCommand { get; set; }
        public ICommand AddWorldCommand { get; set; }

        

        public MainPageViewModel()
        {
            StartLearningCommand = new Command(StartLearning);
            StudyProgressCommand = new Command(StudyProgress);
            AddWorldCommand = new Command(AddWorld);

          
        }


        private async void StartLearning()
        {
            await Navigation.PushModalAsync(new DictionaryPage());
        }

        private async void AddWorld()
        {
            await Navigation.PushModalAsync(new AddWord());
        }


        INavigation Navigation => Application.Current.MainPage.Navigation;
        private async void StudyProgress()
        {
            await Navigation.PushModalAsync(new WordGrid());
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
     }
}
