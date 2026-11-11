# SkeptiCamp Timer Version 2



Code by Mitchell Scott Lampert  Mitch@MitchLampert.net



## A Timer That Counts Down Twice



This is a timer application designed for conferences where speakers are required to spend some time doing Q\&A for their presentation.



When it first reaches 00:00, it will reset to 5 minutes (05:00), and continue counting down from there, now with a "Mandatory Q\&A" label visible on the screen.



For example, if a speaker is given a 30 minute time slot, this timer will count down from 25 minutes. When it reaches 0, it will pop a "Q\&A" label on top, reset itself to 5 more minutes, and then automatically count down from there. (assuming 5 minutes of their 30 minute slot is required to be Q\&A.)



This application was originally built for SkeptiCamp NYC.



SkeptiCamp is an open conference on science, skepticism, and critical thinking. Based, in part, on the BarCamp model of unconferences, just about anyone can be a speaker, as long as their topic is relevant to the theme of the event.



Timing rules can vary among different SkeptiCamp events. The SkeptiCamp in New York City requires all speakers to spend at least 5 minutes doing Q\&A with the audience. So, this timer program was built to help facilitate that rule.



## Requirements



This is a Windows Presentation Foundation (WPF) application project, coded with C#/.NET 10.0. It has been tested to work on Microsoft Windows 7, 10 and 11.



It does NOT have any third-party dependencies.



## Prior Versions

In case you're curious, version 1 was a Windows Form application. It has the largely same code behind it. But, the text didn't resize with the window in that version.

This new version resizes the text on screen to fit the size of its window.



(As a side note, NONE of this code was generated through A.I., though it wasn't exactly an engineering challenge to put together, either.)

