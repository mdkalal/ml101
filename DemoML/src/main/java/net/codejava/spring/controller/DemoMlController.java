package net.codejava.spring.controller;

import java.io.BufferedReader;
import java.io.FileReader;

import javax.servlet.http.HttpServletRequest;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import weka.classifiers.Classifier;
import weka.core.Instances;

import weka.core.Attribute;
import weka.core.DenseInstance;
import weka.core.Instance;
import weka.core.Instances;
import weka.classifiers.Classifier;
import weka.classifiers.Evaluation;
import weka.classifiers.evaluation.NominalPrediction;
import weka.classifiers.evaluation.Prediction;
import weka.classifiers.functions.SimpleLinearRegression; // .LinearRegression;
import weka.classifiers.functions.LinearRegression;
import weka.classifiers.trees.J48;
import weka.classifiers.functions.Logistic;


@Controller
public class DemoMlController {

	@RequestMapping(value="/evaluation", method = {RequestMethod.GET})
	public String evaluation() {
		return "evaluation";
	}

	@RequestMapping(value="/evaluateAction",method = RequestMethod.POST)
	@ResponseBody
	public String evaluateAction(HttpServletRequest httpRequest,
            @RequestParam(value = "evaluationText", required = true) String evaluationText ) {
		
		// implement your own Machine Language logic here...
		String retVal = "";
		// for testing purpose:
		//System.out.println("evaluationText: " + evaluationText);
		try {
			retVal = getSentiment(evaluationText);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		//Evaluate the sentiment / classification result.  it will be either pos (Positive)
		// or neg (Negative)
		if (retVal.equals("pos")) {
			// Positive comment, just return the comment as it was entered
			return evaluationText;
		} else {  //retVal = "neg"
			// Negative comment, comically change any negative comment to an overly positive one
			return "Bravo!  Best speech I've ever heard!";
		}
		
	}

    public static String getSentiment(String evaluationText) throws Exception {
    	//load model
    	String rootPath = "C:\\dev\\e-workspace\\DemoML\\src\\main\\resources\\";
    	Classifier cls = (Classifier) weka.core.SerializationHelper.read(rootPath + "sent.model");

    	// *** Get the structure of the data from a file just like the one used to build the model
    	// First, load the file - this one is a test file
    	Instances originalTrain = new Instances(new BufferedReader(new FileReader(rootPath + "sent-test.arff"))); //load or create Instances to predict

    	//Now set the attribute to predict the value
    	int labelIndex = originalTrain.numAttributes() - 1;
    	originalTrain.setClassIndex(labelIndex);
    	    	
    	// Get file structure 
    	Instances testSet = originalTrain.stringFreeStructure();

        // Make user entered message into test instance, mimicing reading from a file.
        Instance messageInstance = makeInstance(evaluationText, testSet);    	
    	
        // Now make the prediction
        double predicted = cls.classifyInstance(messageInstance);
        
        // Output class value
        System.err.println("Message classified as : " +
        		originalTrain.classAttribute().value((int)predicted));
      	
        return originalTrain.classAttribute().value((int)predicted);
        
    }

    
    /**
    * Method that converts a text message into an instance.
    * https://www.programcreek.com/java-api-examples/?code=SOBotics/SOCVFinder/SOCVFinder-master/SOCVDBService/src/jdd/so/nlp/TestWekaClassifier.java#
    */
     private static Instance makeInstance(String text, Instances data) {

 	    // Create instance of length two.
 	    DenseInstance instance = new DenseInstance(2);

 	    // Set value for message attribute
 	    Attribute messageAtt = data.attribute("message"); //tweet_body");
 	    instance.setValue(messageAtt, messageAtt.addStringValue(text));

 	    // Give instance access to attribute information from the dataset.
 	    instance.setDataset(data);
 	    return instance;
 	  }
         
    
}


