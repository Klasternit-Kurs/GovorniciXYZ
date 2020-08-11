using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
		private string _pretraga;
		public string Pretraga 
		{ 
			get => _pretraga; 
			set
			{
				_pretraga = value;
				dg.ItemsSource = slu.Where(slus => slus.Ime.ToUpper().Contains(_pretraga.ToUpper())).ToList();
			}
		}

		public MainWindow()
		{
			InitializeComponent();
			DataContext = new Govornik();
			prt.DataContext = this;
			dg.ItemsSource = slu;

			slu.Add(new Slusaoc("Pera", (DataContext as Govornik)));
			slu.Add(new Slusaoc("Neko", (DataContext as Govornik)));
			slu.Add(new Slusaoc("Trecko", (DataContext as Govornik)));
			slu.Add(new Slusaoc("Zklj", (DataContext as Govornik)));

			List<string> nesto = new List<string>();
			nesto.Add("Jen");
			nesto.Add("Dva");
			nesto.Add("Tri");
			cbox.ItemsSource = slu;
			cbox.DisplayMemberPath = "Ime";
			if (nesto.Count > 0)
				cbox.SelectedIndex = 0;
		}

		private void Klik(object sender, RoutedEventArgs e)
		{
			(DataContext as Govornik).Govori();
			//dg.ItemsSource = slu.ToList();
		}
	}

	public class Slusaoc : INotifyPropertyChanged
	{
		private Govornik gov;

		private bool _slusam;
		public bool Slusam 
		{ 
			get => _slusam; 
			set
			{
				_slusam = value;
				if (_slusam)
					gov.GovoriSe += Slusaj;
				else
					gov.GovoriSe -= Slusaj;
			}
		}

		public string Ime { get; set; }

		public ObservableCollection<string> SveReceno { get; set; } = new ObservableCollection<string>(); 

		public Slusaoc(string i, Govornik g)
		{
			Ime = i;
			gov = g;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void Slusaj(object s, GovorEventArgs e)
			=> SveReceno.Add(e.govor);

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
