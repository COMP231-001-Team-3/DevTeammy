using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace teammy.ViewModels
{
    //Serves as parent class of all viewmodels
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Sets property when the property value is not the given value.
        /// </summary>
        /// <typeparam name="T">Type of caller</typeparam>
        /// <param name="member">The property instance who calls this method</param>
        /// <param name="value">Value to be set</param>
        /// <param name="name">Name of the calling property</param>
        public void SetProperty<T>(ref T member, T value, [CallerMemberName] string name = null)
        {
            //If member is non-existent or current value is equal to new value...then
            if (member == null || member.Equals(value))
            {
                return;
            }

            member = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        ///     PropertyChanged Event Invoker
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
