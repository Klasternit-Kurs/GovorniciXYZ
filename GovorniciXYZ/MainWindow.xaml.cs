using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GovorniciXYZ
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ObservableCollection<Slusaoc> slu = new ObservableCollection<Slusaoc>();

		public MainWindow()
		{
			InitializeComponent();
			DataContext = new Govornik();

			dg.ItemsSource = slu;

			slu.Add(new Slusaoc("Pera", (DataContext as Govornik)));
			slu.Add(new Slusaoc("Neko", (DataContext as Govornik)));
			slu.Add(new Slusaoc("Trecko", (DataContext as Govornik)));
			slu.Add(new Slusaoc("Zklj", (DataContext as Govornik)));
		}

		private void Klik(object sender, RoutedEventArgs e)
		{
			(DataContext as Govornik).Govori();
			dg.ItemsSource = slu.ToList();
		}
	}

	public class Slusaoc
	{
		public string Ime { get; set; }
		public string ZadnjeReceno { get; set; }

		public Slusaoc(string i, Govornik g)
		{
			Ime = i;
			g.GovoriSe += Slusaj;
		}

		public void Slusaj(object s, GovorEventArgs e)
			=> ZadnjeReceno = e.govor;
	}

	public class GovorEventArgs : EventArgs
	{
		public string govor;
		public DateTime vremeGovora;
	}

	public class Govornik
	{
		public delegate void GovorDel(object KoSalje, GovorEventArgs e);

		public event GovorDel GovoriSe;

		public string Govor { get; set; }

		public void Govori()
		{
			GovorEventArgs a = new GovorEventArgs();
			a.govor = Govor;
			a.vremeGovora = DateTime.Now;
			GovoriSe?.Invoke(this, a);
		}
	}
}
