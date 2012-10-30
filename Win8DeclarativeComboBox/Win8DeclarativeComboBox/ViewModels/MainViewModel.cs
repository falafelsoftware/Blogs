using System.ComponentModel;

namespace Win8DeclarativeComboBox.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int selectedDirection;
        public int SelectedDirection
        {
            get
            {
                return this.selectedDirection;
            }
            set
            {
                this.selectedDirection = value;
                this.OnPropertyChanged("SelectedDirection");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
