using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using CoreWebApplication1.Models;
using Microsoft.ML;
using MyMLAppML.Model.DataModels;


namespace CoreWebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private const string MODEL_FILEPATH = "C:\\dev\\Model\\MLModel.zip";
        public IActionResult Index()
        {
            return View();
        }

        public string CheckSentiment(string pValue)
        {

            MLContext mlContext = new MLContext();
            string retVal = "";


            ITransformer mlModel = mlContext.Model.Load(GetAbsolutePath(MODEL_FILEPATH), out DataViewSchema inputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            
            ModelInput sampleData = new ModelInput();
            sampleData.SentimentText = pValue;

            
            ModelOutput predictionResult = predEngine.Predict(sampleData);
            

            if (predictionResult.Prediction == true)  // value = 1, or negative comment
            {
                //Comically change it to an overly positive comment!
                retVal = "Bravo!  That was the greatest speech ever!";
            }
            else  //value = 0, or positive comment
            {
                retVal = pValue;
            }

            return retVal;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
