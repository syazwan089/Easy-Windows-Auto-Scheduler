﻿
using System.ComponentModel;
using System.Diagnostics;


namespace ShutdownSch
{
    internal class MainViewModel : INotifyPropertyChanged
    {

        private bool isCountDown;
		private TimeSpan timePicker;

		public TimeSpan SelectedTimePicker
		{
			get { return timePicker; }
			set
            {
                timePicker = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTimePicker"));
            }
		}

        private TimeSpan spanTime;

        public TimeSpan selectedTime
        {
            get { return spanTime; }
            set { spanTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectedTime"));
            }
        }


        private string currentTime;

        public string CurrentTimeSpan
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentTimeSpan"));
            }
        }

        private string formatedTime;

        public string FormatedTime
        {
            get { return formatedTime; }
            set { formatedTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FormatedTime"));
            }
        }



        public Command SetButton { get; set; }


        public MainViewModel()
        {
            isCountDown = true;
            SelectedTimePicker = new TimeSpan(18, 00, 00);
            DateTime DateTimeValue = DateTime.Today.Add(SelectedTimePicker);
            FormatedTime = DateTimeValue.ToString("hh:mm tt");
          
           
            SetButton = new Command(setTime);
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000; // 1 second
            timer.Elapsed += (s, e) => CheckTime();
            timer.Start();

        }

        private void setTime(object obj)
        {
            selectedTime = SelectedTimePicker;
            DateTime DateTimeValue = DateTime.Today.Add(selectedTime);
            FormatedTime = DateTimeValue.ToString("hh:mm tt");
            PlaySound();
            App.Current.MainPage.DisplayAlert("Time has been set", $"Your computer will shutdown at {selectedTime}", "Close");
        }

        public TimeSpan SelectedTime { get; set; }

 

        private void CheckTime()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            DateTime DateTimeValue = DateTime.Today.Add(currentTime);
            FormatedTime = DateTimeValue.ToString("hh:mm tt");
            CurrentTimeSpan = FormatedTime;

            TimeSpan notification = selectedTime.Subtract(TimeSpan.FromMinutes(10));
           

            if (notification.Hours == currentTime.Hours && notification.Minutes == notification.Minutes)
            {
                PlaySound();
            }

            if (selectedTime.Hours == currentTime.Hours && selectedTime.Minutes == currentTime.Minutes && isCountDown)
            {
                Task.Run(ExecuteCommand);              
                isCountDown = false;
            }

            else
            {
                isCountDown = true;
            }
        }

        private async void ExecuteCommand()
        {
            string command = "shutdown";
            string arguments = "-s -t 100"; // 100 seconds delay, adjust as needed

            try
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = command;
                startInfo.Arguments = arguments;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                process.StartInfo = startInfo;
                process.Start();

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Command Executed", "Shutdown command executed!", "OK");
                });
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                });
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public void PlaySound()
        {
            try
            {
                var filePath = "sound_file.mp3"; // Path to your sound file
                Preferences.Set("SoundFilePath", filePath);
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await TextToSpeech.SpeakAsync("You computer will shutdown in 5 minutes"); // Optional: You can use TextToSpeech to announce the sound playback.                  
                });
            }
            catch (Exception ex)
            {
                // Handle any exceptions
            }
        }
    }
}
