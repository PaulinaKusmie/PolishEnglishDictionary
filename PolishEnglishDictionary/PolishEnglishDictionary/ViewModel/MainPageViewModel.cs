using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PolishEnglishDictionary.ViewModel
{
     class MainPageViewModel : INotifyPropertyChanged
    {
        public ICommand StartLearningCommand { get; set; }
        public ICommand StudyProgressCommand { get; set; }

        public MainPageViewModel()
        {
            StartLearningCommand = new Command(StartLearning);
            StudyProgressCommand = new Command(StudyProgress);
        }


        private async void StartLearning()
        {
            await Navigation.PushModalAsync(new DictionaryPage());

        }



        INavigation Navigation => Application.Current.MainPage.Navigation;
        private  void StudyProgress()
        {
       

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
