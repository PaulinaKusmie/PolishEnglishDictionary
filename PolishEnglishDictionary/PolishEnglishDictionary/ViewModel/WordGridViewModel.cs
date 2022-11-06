using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text;

namespace PolishEnglishDictionary.ViewModel
{
    class WordGridViewModel : INotifyPropertyChanged
    {
       
        public ObservableCollection<Dictionary> Employees { get; set; }
        private string englishWord;
        private string polishWord;
        SqlConnection sqlConnection2;
        SqlConnection sqlConnection;
        bool iKnow = false;
        int id = 0;

        public string EnglishWord
        {
            get => englishWord;
            set
            {

                englishWord = value;
                OnPropertyChanged();
            }
        }


        public string PolishWord
        {
            get => polishWord;
            set
            {

                polishWord = value;
                OnPropertyChanged();
            }
        }


        public bool IKnow
        {
            get => iKnow;
            //{
            //    if (iKnow == true)
            //        return "Umiem";
            //    else
            //        return "Nie umiem";
            //} 
            set
            {

                iKnow = value;
                OnPropertyChanged();
            }
        }


        public int Id
        {
            get => id;
            set
            {

                id = value;
                OnPropertyChanged();
            }
        }





        public WordGridViewModel()
        {
            Dictionary dicon = new Dictionary();
            int count = dicon.FindCount();
            Employees = dicon.ListDictionary(count);
        }

      
        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    

    }
}
