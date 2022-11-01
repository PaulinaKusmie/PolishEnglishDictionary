using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PolishEnglishDictionary.ViewModel
{
    class DictionaryPageViewModel : INotifyPropertyChanged
    {
        #region Fields
        private string word = string.Empty;
        private string userTranslate = string.Empty;
        private string effect = string.Empty;
        private Color effectColor;
        private Dictionary dic = new Dictionary();

        INavigation Navigation => Application.Current.MainPage.Navigation;
        #endregion

        public ICommand TranslateCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand RememberCommand { get; set; }
        public ICommand NoRememberCommand { get; set; }


        #region Properties
        public string Word
        {
            get => word;
            set
            {

                word = value;

                OnPropertyChanged();
            }
        }

        public string UserTranslate
        {
            get => userTranslate;
            set
            {

                userTranslate = value;

                OnPropertyChanged();
            }
        }



        public string Effect
        {
            get => effect;
            set
            {

                effect = value;

                OnPropertyChanged();
            }
        }


        public Color EffectColor
        {
            get => effectColor;
            set
            {

                effectColor = value;

                OnPropertyChanged();
            }
        }




        #endregion

        public DictionaryPageViewModel()
        {
            TranslateCommand = new Command(Translate);
            CheckCommand = new Command(Check);
            RememberCommand = new Command(Remember);
            NoRememberCommand = new Command(NoRemember);
            Word = dic.PolishWorld;

        }

        private void Remember()
        {
            dic.IKnowing(dic.Id, true);
            ClearState();
            dic.Connect();
            Word = dic.PolishWorld;
        }


        private void NoRemember()
        {
            ClearState();
            dic.Connect();
            Word = dic.PolishWorld;
        }


        private void Translate()
        {

            Effect = "Tłumaczenie słowa to " + dic.EnglishWord;
            EffectColor = Color.WhiteSmoke;
        }


        
        private void Check()
        {

           if(UserTranslate.ToLower() == dic.EnglishWord.ToLower())
           {
                Effect = "Gratulacje! Poprawna odpowiedź!";
                EffectColor = Color.Green;
           }
           else
           {
                Effect = $"Nie udało się, poprawna odpowiedź to {dic.EnglishWord}";
                EffectColor = Color.Red;
           }

        }

        private void ClearState()
        {
            UserTranslate = string.Empty;
            Word = string.Empty;
            Effect = string.Empty;
        }

        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class Dictionary : INotifyPropertyChanged
    {
        private string englishWord;
        private string polishWorld;
        SqlConnection sqlConnection;
        string answear = string.Empty;
        string world = string.Empty;
        bool iKnow = false;
        int id = 0;



        #region properties
        public int Id 
        {
            get => id;
            set
            {

                id = value;
                Connect();
                OnPropertyChanged();
            }
        }

        public string EnglishWord
        {
            get => englishWord;
            set
            {

                englishWord = value;
                OnPropertyChanged();
            }
        }

        public string PolishWorld
        {
            get => polishWorld;
            set
            {

                polishWorld = value;
                OnPropertyChanged();
            }
        }

        public bool IKnow
        {
            get => iKnow;
            set
            {

                iKnow = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public Dictionary()
        {
            Connection();
            Connect();
        }

 
        public void IKnowing(int id, bool know)
        {
            Connection();
            sqlConnection.Open();
            using (SqlCommand command2 = new SqlCommand("Update Words set IKnow = @IKnow where Id = @PW", sqlConnection))
            {
                command2.Parameters.Add(new SqlParameter("@PW", id));
                command2.Parameters.Add(new SqlParameter("@IKnow", know));
                command2.ExecuteNonQuery();
            }         
            sqlConnection.Close();
        }

        public void Connect()
        {
            id++;
            sqlConnection.Open();
            string query = "SELECT  * FROM Words where Id = @PW";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Parameters.Add(new SqlParameter("@PW", id));

            SqlDataReader rader = command.ExecuteReader();
            while (rader.Read())
            {
                PolishWorld = (string)rader["PolishWorld"];
                EnglishWord = (string)rader["EnglishWorld"];
                IKnow = (bool)rader["IKnow"];
            }
            sqlConnection.Close();
        }

        private void Connection()
        {
            string srvrbdname = "DictionaryDatabase";
            string srvrname = "172.20.10.5";
            string srvarusername = "Paulina";
            string srvrpassword = "123456";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrbdname};User ID={srvarusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);
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
