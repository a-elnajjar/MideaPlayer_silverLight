using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;



namespace MideaPlayer_silverLight
{
	public partial class MainPage : UserControl
	{
        private DispatcherTimer timer;
		private double UnMuteVolume;
		private bool IsScrubberLocked{get;set;}
		public MainPage()
        {
		
			// Required to initialize variables
			InitializeComponent();
            //states for ply buttton 
            btnPlayPause.Checked += new RoutedEventHandler(btnPlayPause_Checked);
            btnPlayPause.Unchecked += new RoutedEventHandler(btnPlayPause_Unchecked);
			
			//Scrubbing even 
			timeSlider.MouseLeftButtonUp += new MouseButtonEventHandler(timeSlider_MouseLeftButtonUp);
			timeSlider.MouseLeftButtonDown +=new MouseButtonEventHandler(timeSlider_MouseLeftButtonDown);

            //states for time slider 
            MyME.CurrentStateChanged +=new  RoutedEventHandler(MyME_CurrentStateChanged);

            //running time 
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += new EventHandler(timer_Tick);
		}
		
		//MouseLeftButtonDown event 
		void timeSlider_MouseLeftButtonDown(object sender,MouseButtonEventArgs e)
		{
			IsScrubberLocked = true;
		}
		//MouseLeftButtonUp event 
		void timeSlider_MouseLeftButtonUp(object sender,MouseButtonEventArgs e)
		{
			IsScrubberLocked =false;
			MyME.Position = TimeSpan.FromMilliseconds((MyME.NaturalDuration.TimeSpan.TotalMilliseconds * timeSlider.Value));
		}
			
        //time tick methode
        void timer_Tick(object sender, EventArgs e)
        {
            if (MyME.NaturalDuration.TimeSpan.TotalSeconds > 0 && !IsScrubberLocked)
            {
                txtTime.Text = string.Format("{0:00}:{1:00}", MyME.Position.Minutes, MyME.Position.Seconds);
                timeSlider.Value = MyME.Position.TotalSeconds / MyME.NaturalDuration.TimeSpan.TotalSeconds;
            }
        }
		
		//states change
        void MyME_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (MyME.CurrentState == MediaElementState.Playing)
            {
                timer.Start();
            }

            else
            {
                timer.Stop();
            }

        }
		
		//btnPlayPause Play event
        public  void btnPlayPause_Unchecked(object sender, RoutedEventArgs e)
        {
            MyME.Play();
            btnPlayPause.Content = "Pause";
        }
		
		//btnPlayPause Pause event
        void btnPlayPause_Checked(object sender, RoutedEventArgs e)
        {
            MyME.Pause();
            btnPlayPause.Content = "play";
        }

        private void SliderVolume_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
        	// TODO: Add event handler implementation here.
			MyME.Volume = SliderVolume.Value;
        }

        private void btnMute_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			UnMuteVolume = MyME.Volume;
			MyME.Volume  = 0;
			btnMute.Content = "UnMute";
        }

        private void btnMute_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			MyME.Volume = UnMuteVolume;
			btnMute.Content = "Mute";
        }



        


	}
}