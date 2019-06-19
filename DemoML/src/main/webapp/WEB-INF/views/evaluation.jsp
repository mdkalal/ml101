<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://www.springframework.org/tags/form" prefix="form" %>
<%@ taglib prefix="spring" uri="http://www.springframework.org/tags" %>
  
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" 
	"http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
   <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
   <title>Registration</title>
   <link href="<c:url value="/resources/css/style.css" />" rel="stylesheet">
   <script src="<c:url value="/resources/js/jquery-1.12.2.min.js" />"></script>
</head>

<body>
	<div align="center">
		<form id="target" name="evaluationform" action="URL_to_script" method="POST">

            <h1 style="text-decoration: underline;">Presentation Evaluation</h1>
	        <p style="font-weight: bold;">What did you think about Mark's presentation?</p>        
            
            <textarea class="textInputArea" rows=2" cols="15" 
                      maxlength="20" id="evaluateName" name="evaluateName" placeholder="Name"></textarea>  
			

            <textarea required class="textInputArea" rows=2" cols="45" 
                      maxlength="1000" id="evaluationText" name="evaluationText" placeholder="Comment"></textarea>  
            <br><br>
                        
            <input class="aSubmitButton" id="theSubmitButton" name="submit" type="submit" value="Submit"/>

		</form>
		<br>
        <h3 >RESULTS:</h1>
		<p style="font-weight: bold; color: green;" id="displayData"></p>
		
	</div>
</body>
</html>

<script type="text/javascript">

$( "#target" ).submit(function( event ) {
   event.preventDefault();

   evaluationText = $('#evaluationText').val();
   eName = $('#evaluateName').val();
   var result;
   
   request = $.ajax({
		  url: "${pageContext.servletContext.contextPath}/evaluateAction",
		  method: "POST",
		  //beforeSend: function( xhr ) {xhr.setRequestHeader(header, token);},
		  data: { evaluationText:evaluationText }
	   })
   .done(function(data) {
	      result = $("<p></p>").text(eName + " - " + data);
	      $("#displayData").prepend(result);
    })
    .fail(function(data) {
    	  result = $("<p></p>").text("Unexpected error! - " + data);
    	  $("#displayData").append(result);
    });   

   $('#evaluationText').val("");
   $('#evaluateName').val("");
   //$('#target').focus();
});

</script>
