# SCICHART LICENSING TEST APP 

This is a troubleshooting application for applying licenses to SciChart WPF Charts in both WPF applications and WinForms. 

For more information, see the [SciChart Licensing Troubleshooting](https://www.scichart.com/licensing-scichart-wpf/) page. 


### Steps to Troubleshoot Licenses

Licensing in SciChart is two-step.

- You will need to [activate a developer license](https://www.scichart.com/licensing-scichart-wpf/) to debug your apps or view them in the Visual Studio designer. 
- You will need to apply a Runtime key to distribute those apps to other computers.

Follow these steps to debug licenses. 
 
 1. Download this Licensing Test App and unzip it to your computer.
 2. Enter your Runtime License key from your Profile page at [www.scichart.com/profile](https://www.scichart.com/profile/) in App.xaml.cs & compile the Licensing Test App
 3. Run the Licensing Test App on the other computer where you are experiencing problems. 
  3.1 Make sure this is a computer without an activated developer license. This is because the Activated Developer license will interfere with this test.
  3.2 **Expected Result:** You should not see a ‘Powered by SciChart’ watermark or a trial expired when running the Licensing Test App.
  3.3 If you do, contact us, you’ve probably found a bug 🙂
 4. Debug the Licensing Test App on another computer.
  4.1 Make sure this is a computer without an activated developer license. This is because the Activated Developer license will interfere with this test.
  4.2 **Expected Result:** You should see a ‘Powered by SciChart’ watermark or a trial expired notice.
  4.3 This is expected behaviour as SciChart has detected you are running inside Visual Studio and is searching for a developer license.
 5. Activate your developer licnse and repeat the test above running the app inside debugger. 
  5.1 **Expected Result:** You should not see a ‘Powered by SciChart’ watermark or a trial expired when running the Licensing Test App.
	 
If either expectation above fails, please contact tech support and we will be glad to assist. Please tell us what you tried, and what version of SciChart you are using and share your runtime key with us.

If the expectations above pass, it is likely that the Runtime key has been set incorrectly in your main application. Please double check you have set the Runtime Key once and once only in your app. It should be set before any SciChartSurface instances are created. A good guide to do this is our FAQ on [Licensing in a DLL](https://www.scichart.com/questions/question/license-in-dll).



