using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

/* ==============================================================================

Code by Mitchell Scott Lampert  Mitch@MitchLampert.net

This is a timer application designed for conferences where speakers are required
to spend some time doing Q&A for their presentation.

When it first reaches 00:00, it will reset to 5 minutes (05:00), and continue
counting down from there, now with a "Mandatory Q&A" label visible on the screen.

For example, if a speaker is given a 30 minute time slot, this timer will count
down from 25 minutes. When it reaches 0, it will pop a "Q&A" label on top, reset 
itself to 5 more minutes, and then automatically count down from there. (assuming
5 minutes of their 30 minute slot is required to be Q&A.)

This application was originally built for SkeptiCamp NYC.

SkeptiCamp is an open conference on science, skepticism, and critical thinking.
Based, in part, on the BarCamp model of unconferences, just about anyone can be a
speaker, as long as their topic is relevant to the theme of the event.

Timing rules can vary among different SkeptiCamp events. The SkeptiCamp in 
New York City requires all speakers to spend at least 5 minutes doing Q&A with the
audience. So, this timer program was built to help facilitate that rule.
============================================================================= */

/*
This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or distribute this software, either in source code form or as a compiled binary, for any purpose, commercial or non-commercial, and by any means.
In jurisdictions that recognize copyright laws, the author or authors of this software dedicate any and all copyright interest in the software to the public domain. We make this dedication for the benefit of the public at large and to the detriment of our heirs and successors. We intend this dedication to be an overt act of relinquishment in perpetuity of all present and future rights to this software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace SCTimerV2
{
    internal enum TimerStates
    {
        TimerStopped,
        TimerRunning,
        TimerPaused
    }

    public partial class MainWindow : Window
    {
        private DispatcherTimer TheTimer = new DispatcherTimer();

        private TimerStates TimerState = TimerStates.TimerStopped;

        private int TimeMins = 25;
        private int TimeSecs = 0;

        private bool IsQATime = false;

        public MainWindow()
        {
            InitializeComponent();

            this.WindowState = WindowState.Maximized;

            TheTimer.Interval = TimeSpan.FromSeconds(1);
            TheTimer.Tick += TheTimer_Tick;

            BtnStartStop.Content = "Start";
            BtnPause.Content = "Pause";
            LblTimeDisplay.Content = "25:00";
            LblQA.Visibility = Visibility.Hidden;
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(TxtMins.Text, out TimeMins);
            if (TimeMins < 0) TimeMins = 0;
            //This timer is not meant to be used for more than about an hour or so. But, we'll set the max to 48 hours, just in case we need that for some bizarre reason.
            if (TimeMins > 2880) TimeMins = 2880;
            TimeSecs = 0;

            if (TimerState != TimerStates.TimerStopped)
            {
                TheTimer.Stop();
                BtnStartStop.Content = "Start";
                TimerState = TimerStates.TimerStopped;
            }
            else if (TimeMins > 0) //Don't allow the timer to start if the minutes are set to 0 or lower.
            {
                TheTimer.Start();
                BtnStartStop.Content = "Stop/Reset";
                TimerState = TimerStates.TimerRunning;
            }

            //Any time we stop or reset the timer, we are no longer in Q&A mode, if we were in there already:
            //(See TheTimer_Tick for when these values flip.)
            IsQATime = false;
            LblQA.Visibility = Visibility.Hidden;

            BtnPause.Content = "Pause";
            LblTimeDisplay.Content = TimeMins.ToString("00") + ":00";
        } //Start/Stop Button


        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (TimerState == TimerStates.TimerPaused)
            {
                BtnPause.Content = "Pause";
                TheTimer.Start();
                TimerState = TimerStates.TimerRunning;
            }
            else if (TimerState == TimerStates.TimerRunning)
            {
                BtnPause.Content = "Continue";
                TheTimer.Stop();
                TimerState = TimerStates.TimerPaused;
            }
        } //Pause/Continue Button


        private void TheTimer_Tick(object? sender, EventArgs e)
        {
            if (TimeSecs == 0)
            {
                TimeMins--;
                TimeSecs = 59;
            }
            else
            {
                TimeSecs--;
            }

            if (TimeMins < 5)
                LblTimeDisplay.Foreground = Brushes.Red;
            else if (TimeMins < 10)
                LblTimeDisplay.Foreground = Brushes.Orange;
            else LblTimeDisplay.Foreground = Brushes.Black;

            if (TimeMins == 0 & TimeSecs == 0)
            {
                //This is where the unique functionality of this particular timer program happens:
                //When the timer reaches 00:00 for the first time, it will go into "Q&A Mode" and automatically reset for 5 more minutes.
                //When it reaches 00:00 again, it will actually stop entirely, until the user resets it.
                if (IsQATime)
                {
                    TheTimer.Stop();
                    TimerState = TimerStates.TimerStopped;
                }
                else //Wasn't QA time already? Then go into that mode now:
                {                    
                    IsQATime = true;
                    LblQA.Visibility = Visibility.Visible;
                    TimeMins = 5;
                }
            } //Mins and Sec = 0

            LblTimeDisplay.Content = TimeMins.ToString("00") + ":" + TimeSecs.ToString("00");
        } //Timer Tick

    } //Window Class
} //namespace