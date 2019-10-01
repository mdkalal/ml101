using System;
using System.IO;
using System.Linq;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;


namespace MLSample
{
    class Program
    {

        private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

        static void Main(string[] args)
        {
            DoClassification();
            DoRegression();
        }



        static void DoClassification()
        {
            // Declare variables
            MLContext mlContext;
            string trainDataPath = Path.Combine(_appPath, "..", "..", "..", "Data", "fruit.csv");
            PredictionEngine<Fruit, FruitPrediction> predEngine;
            ITransformer trainedModel;
            IDataView trainingDataView;

            // Create MLContext 
            mlContext = new MLContext();

            // Load data from file
            trainingDataView = mlContext.Data.LoadFromTextFile<Fruit>(trainDataPath, hasHeader: true, separatorChar: ',');

            // ML.Net data process configuration
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Class", outputColumnName: "Label")
                           .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Color", outputColumnName: "ColorFeaturized"))
                           .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Size", outputColumnName: "SizeFeaturized"))
                           .Append(mlContext.Transforms.Concatenate("Features", "ColorFeaturized", "SizeFeaturized"));

            // Set the training type and algorithm, map Label to value
            var trainingPipeline = pipeline.Append(mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(DefaultColumnNames.Label, DefaultColumnNames.Features))
                                   .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));


            // Train the model
            trainedModel = trainingPipeline.Fit(trainingDataView);

            // Do a prediction test
            Fruit singleIssue = new Fruit() { Color = "2", Size = "3" };
            predEngine = trainedModel.CreatePredictionEngine<Fruit, FruitPrediction>(mlContext);
            
            var prediction = predEngine.Predict(singleIssue);
            Console.WriteLine("Prediction - Result: " + prediction.Class);

        }

        static void DoRegression()
        {
            MLContext mlContext;
            string trainDataPath = Path.Combine(_appPath, "..", "..", "..", "Data", "house.csv");
            PredictionEngine<House, HousePrediction> predEngine;
            ITransformer trainedModel;
            IDataView trainingDataView;

            // Create MLContext 
            mlContext = new MLContext();

            // Load data from file
            trainingDataView = mlContext.Data.LoadFromTextFile<House>(trainDataPath, hasHeader: true, separatorChar: ',');

            // ML.Net data process configuration
            var pipeline = mlContext.Transforms.CopyColumns(inputColumnName: "Price", outputColumnName: "Label")
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "Size", outputColumnName: "SizeEncoded"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "Land", outputColumnName: "LandEncoded"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "Rooms", outputColumnName: "RoomsEncoded"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "Granite", outputColumnName: "GraniteEncoded"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "Extra_Bathroom", outputColumnName: "Extra_BathroomEncoded"))
                    .Append(mlContext.Transforms.Concatenate("Features", "SizeEncoded", "LandEncoded", "RoomsEncoded", "GraniteEncoded", "Extra_BathroomEncoded"))

                    // Set the training type and algorithm
                    .Append(mlContext.Regression.Trainers.StochasticDualCoordinateAscent(DefaultColumnNames.Label, DefaultColumnNames.Features));


            // Train the model
            trainedModel = pipeline.Fit(trainingDataView);

            // Do a prediction test
            House singleIssue = new House() { Size = 1300, Land = 4000, Rooms = 3, Granite = "0", Extra_Bathroom = "0" };
            predEngine = trainedModel.CreatePredictionEngine<House, HousePrediction>(mlContext);

            var prediction = predEngine.Predict(singleIssue);
            Console.WriteLine("Prediction - Result: " + prediction.Price);
        }

        /*
        SaveModelAsFile(mlContext, _trainedModel);
        private static void SaveModelAsFile(MLContext mlContext, ITransformer model)
        {
            string _modelPath = Path.Combine(_appPath, "..", "..", "..", "Models", "model.zip");

            using (var fs = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                mlContext.Model.Save(model, fs);


            Console.WriteLine("The model is saved to {0}", _modelPath);
        }
        */

    }

    public class Fruit
    {

        [LoadColumn(0)]
        public string Color { get; set; }
        [LoadColumn(1)]
        public string Size { get; set; }
        [LoadColumn(2)]
        public string Class { get; set; }
    }


    public class FruitPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Class;
    }

    public class House
    {

        [LoadColumn(0)]
        public float Size { get; set; }
        [LoadColumn(1)]
        public float Land { get; set; }
        [LoadColumn(2)]
        public float Rooms { get; set; }
        [LoadColumn(3)]
        public string Granite { get; set; }
        [LoadColumn(4)]
        public string Extra_Bathroom { get; set; }
        [LoadColumn(5)]
        public float Price { get; set; }
    }


    public class HousePrediction
    {
        //[ColumnName("PredictedLabel")]
        [ColumnName("Score")]
        public float Price;
    }

}
