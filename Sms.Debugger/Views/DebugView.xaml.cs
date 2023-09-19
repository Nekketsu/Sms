using Sms.Debugger.ViewModels;
using System.Windows.Controls;

namespace Sms.Debugger.Views
{
    /// <summary>
    /// Interaction logic for DebugView.xaml
    /// </summary>
    public partial class DebugView : UserControl
    {
        public DebugView()
        {
            InitializeComponent();
            DataContext = new DebugViewModel();
        }
    }
}
