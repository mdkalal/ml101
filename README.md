# ml101
Machine Learning 101 talk resources

Project descriptions:

CoreWebApplication1 - demo sentiment analysis, in C#
	In order to run this app-
	You need .net Core 2.2 (download available from Microsoft)
	You may be prompted to install a self signed certificate, this is because it is configured to use HTTPS.

	The app is coded to look for the model file in C:\dev\Model\MLModel.zip.
	You can drop the Model folder in a C:\dev folder, or else change the locaton in 
	HomeController:
		private const string MODEL_FILEPATH = "C:\\dev\\Model\\MLModel.zip";

	The pieces that check sentiment and replace negative comments are in
	HomeController/CheckSentiment

myMLApp - ML.Net models and training data

DemoML - demo sentiment analysis, in Java
	The project structure is set up for Eclipse and the Weka libraries (and related dependencies) were imported using Maven

MLNet example - Classification and Regression examples, in c#

Model - ML model used by CoreWebApplication1 - sentiment analysis demo

Data - examples of Weka ready data files

WekaExample  - Classification and Regression examples, in Java
	Project structure is set up for Eclipse

