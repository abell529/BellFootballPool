<%@ Page Language="C#" AutoEventWireup="true" CodeFile="overseascourse-finder.aspx.cs" Inherits="_Default_SA" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Study Abroad Search</title>

<style>

		.program_search_container {
			padding: 0;
		}		
		
		.program_search {
    		background-color: #ffffff;
		}
		
		.program_search_container .search_input {
			font-size: 1rem;
			height: 60px;
			background-color: #e6e6e6;
			border: 1px solid #e6e6e6;
			margin-bottom: 0;
			padding: 0 10px 0 22px;
		}	
		
		button[type=submit]{
			background-color: #f3f3f3;
			border: 1px solid #e6e6e6;
			padding: 1px 6px;
			font-size: 1.125rem;
		}
		
		.program_search_results_table {
			border: 0;
		}	
		
		.program_search_results_table th {
			padding: 20px;
		}		
		
		.program_search_results_table tr:nth-child(even) {
			background-color: #d0d0d0;
		}	
		
		.program_search_results_table tr:nth-child(odd) {
			background-color: #ffffff;
		}		
		
		.program_search_results_table table > thead > tr {
			background-color: #d0d0d0 !important;
		}		
		
		.ugrad-pick {
			width: 310px;
			display: block;
		}
		
		.grad-pick {
			width: 500px;
			display: block;
		}
		
		.picker-radio-ug {
			float: left;
			padding-right: 9px;
			height: 50px;
			padding-top: 9px;
			
		}
		
		.picker-radio-grad {
			float: left;
			padding-right: 9px;
			height: 50px;
			padding-top: 9px;
		}
		
		.pickerug {
			float: left;
			padding-top: 8px;
		}
		
		.pickergrad {
			float: left;
			padding-top: 8px;
		}
		
		.viewug {
			float: left;
		}
		
		.viewgrad {
			float: left;
		}
		
		@media only screen and (min-width: 500px) and (max-width: 979px) {
  			.ugrad-pick {
				width: 250px;
				float: none;
			}
			.grad-pick {
				width: 400px;
				float: none;
			}
		}
		
		@media only screen and (max-width: 499px) {
  			.grad-pick {
				width: 300px;
			}
			
			.ugrad-pick {
				width: 250px;
				display: block;
			}
		
			.pickerug {
				float: none;
				padding-top: 6px;
			}
		
			.pickergrad {
				float: none;
				padding-top: 6px;
			}
		}
		
	</style>	

    

</head>
<body>


<p>Courses in this database have already been reviewed by an academic department!</p>
<ul>
<li>Print out of the TU Study Abroad Course Database information page</li>
<li>Fill out your sections of the Study Abroad Course Form</li>
<li>Get your Academic Advisor's signature on the form</li>
<li>Sign it yourself and turn it in to the Study Abroad Office</li>
</ul>
<p>Course not in this database?</p>
<ul>
<li>Follow the directions on the Study Abroad Course Form</li>
<li>Get your Academic Advisor's signature on the form</li>
<li>Have the form and courses reviewed by the academic department</li>
<li>Sign the form yourself and turn it in to the Study Abroad Office</li>
</ul>

    <section class="program_search">
<div class="program_search_container">
    <form id="form1" runat="server">
    <input name="type" type="hidden" value="major,minor,masters,doctoral,certificate,program" />
<div class="search_input_container">
	<label for="query">Search courses</label><br /> 
	<input id="query" class="search_input" name="query" type="text" /> &nbsp;<br />&nbsp;<br />
	<label for="tuequivalent">TU Course:</label><br /> 
	<input id="tuequivalent" class="search_input" name="tuequivalent" type="text" /> &nbsp;<br />&nbsp;<br />	
	<button type="submit">Search</button></div>
</form></div>
<div class="program_search_results_container">
<div class="all-full program_search_results">
<p class="program_search_results_count">Search Results: <!--<span class="results_count">0</span>--></p>
<a class="program_search_close" href="{{d:1206267}}">Close</a>
<div class="program_search_results_table">&nbsp;</div>
</div>
</div>
</section>


<div id="results1" runat="server">
    <!-- Search Results Content Goes Here -->
</div>

