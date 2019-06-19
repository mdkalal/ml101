import java.io.BufferedReader;
import java.io.FileReader;
import weka.core.Instances;
import weka.classifiers.Classifier;
import weka.classifiers.evaluation.Evaluation;
import weka.classifiers.functions.LinearRegression;
import weka.classifiers.trees.J48;


public class wekaExample {

    public static void main(String[] args) throws Exception {
    	doTree();
    	doReg();

}


    public static void doTree() throws Exception {

    	//First, select classifier (decision tree)
    	Classifier j48tree = new J48();

    	//load training file
    	Instances train = new Instances(new BufferedReader(new FileReader("fruit.arff")));

    	//identify the class/label (typically the last 'column')
    	int lastIndex = train.numAttributes() - 1;
        train.setClassIndex(lastIndex);
        
        //train the model
        j48tree.buildClassifier(train);
        
        //load the test file
        Instances test = new Instances(new BufferedReader(new FileReader("fruit-test.arff")));

        //identify label (should be same as training set)
        test.setClassIndex(lastIndex);

        //Iterate through the test set and display prediction
        for(int i=0; i<test.numInstances(); i++) {
        
                double index = j48tree.classifyInstance(test.instance(i));
                String className = test.attribute(lastIndex).value((int)index);
                System.out.println("Fruit is: " + className);
        }
		
	/*	
        Evaluation trainEval = new Evaluation(train);
        trainEval.evaluateModel(j48tree, train);
        System.out.println("Evaluation:");
        System.out.println(trainEval.toSummaryString());
	*/
		
	}
    
	
    public static void doReg() throws Exception {

    	//First, select classifier (linear regression)
    	Classifier linReg = new LinearRegression();

    	//load training file
    	Instances train = new Instances(new BufferedReader(new FileReader("house.arff")));

    	//identify the class/label (typically the last 'column')
    	int lastIndex = train.numAttributes() - 1;
        train.setClassIndex(lastIndex);
        
        //train the model
        linReg.buildClassifier(train);
        
        //load the test file
        Instances test = new Instances(new BufferedReader(new FileReader("house-test.arff")));

        //identify label (should be same as training set)
        test.setClassIndex(lastIndex);
        
        //Iterate through the test set and display prediction        
        for(int i=0; i<test.numInstances(); i++) {
        
                double index = linReg.classifyInstance(test.instance(i));
                System.out.println("House price: " + index);
                
        }

    }
    
}
