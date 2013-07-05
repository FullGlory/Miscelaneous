using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using CuttingEdge.Conditions;
using MarabouStork.Common.Helpers;

namespace MarabouStork.Scripting.Javascript.Tests
{
	[TestClass]
	public class ScriptingJSIOTests
	{
		private JurassicJavascriptDriver _driver = null; 

		[TestInitialize]
		public void Setup()
		{
			this._driver = new JurassicJavascriptDriver(@"C:\_src\MarabouStork.kiln\trunk\Libraries\ScriptIncludes\PhantomJS\");
			this._driver.LoadLibrary("MarabouStork.Scripting.JS.IO.js");
		}

		[TestMethod]
		public void FixLongFilenames_WithFilenameThatIsNotTooLong_SameFilenameReturned()
		{
			var testString = "ThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouW.xml";

			var result = _driver.ExecFunction("fixLongFilenames", testString);

			Assert.AreEqual(testString, result, "Files under 248 characters long require no change");
		}

		[TestMethod]
		public void FixLongFilenames_WithFilenameThatIsNotTooLong_JavascriptAndCSharpResultsAreTheSame()
		{
			var testString = "ThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouW.xml";

			var jsResult = _driver.ExecFunction("fixLongFilenames", testString);
			var csResult = FileHelper.FixLongFilenames(testString);

			Assert.AreEqual(csResult, jsResult, "The results from the FixLongFilenames function should be the same for Javascript and C#");
		}
		
		[TestMethod]
		public void FixLongFilenames_WithVeryLongFilename_CentralPortionOfStringRemoved()
		{
			var testString = "ThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWextraCharacters.xml";
			var expectation = "ThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWextraCharacters.xml";

			var result = _driver.ExecFunction("fixLongFilenames", testString);

			Assert.AreEqual(expectation, result, "Files over 248 characters long should have the centre part of the filename removed until they are no more than 248 characters long");
		}

		[TestMethod]
		public void FixLongFilenames_WithVeryLongFilename_JavascriptAndCSharpResultsAreTheSame()
		{
			var testString = "ThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWereToTryToCreateAFileFromItThisIsAReallyLongFilenameAndWouldNormallyCauseABigProblemIfYouWextraCharacters.xml";

			var jsResult = _driver.ExecFunction("fixLongFilenames", testString);
			var csResult = FileHelper.FixLongFilenames(testString);

			Assert.AreEqual(csResult, jsResult, "The results from the FixLongFilenames function should be the same for Javascript and C#");
		}

		[TestMethod]
		public void FixLongFilenames_FilenameIncludingPathThatIsNotTooLong_ReturnsOriginalFilename()
		{
			var testString = @"c:\_src\MarbabouStork\SomeFolder\AnotherImportantFolder\AnotherFolder\AnotherFolder\AnotherFolder\AFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenam.xml";
			
			var jsResult = _driver.ExecFunction("fixLongFilenames", testString);

			Assert.AreEqual(testString, jsResult, "Files under 248 characters long require no change");
		}

		[TestMethod]
		public void FixLongFilenames_FilenameIncludingPathThatIsNotTooLong_JavascriptAndCSharpResultsAreTheSame()
		{
			var testString = @"c:\_src\MarbabouStork\SomeFolder\AnotherImportantFolder\AnotherFolder\AnotherFolder\AnotherFolder\AFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenam.xml";

			var jsResult = _driver.ExecFunction("fixLongFilenames", testString);
			var csResult = FileHelper.FixLongFilenames(testString);

			Assert.AreEqual(csResult, jsResult, "The results from the FixLongFilenames function should be the same for Javascript and C#");
		}

		[TestMethod]
		public void FixLongFilenames_FilenameIncludingPathThatIsTooLong_CentralPortionRemovedButPathIsIntact()
		{
			var testString = @"c:\_src\MarbabouStork\SomeFolder\AnotherImportantFolder\AnotherFolder\AnotherFolder\AnotherFolder\AFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenamExtraCharacters.xml";
			var expectation = @"c:\_src\MarbabouStork\SomeFolder\AnotherImportantFolder\AnotherFolder\AnotherFolder\AnotherFolder\AFairlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenameInAFairlyLongFrlyLongFilenameInAFairlyLongFolderNameAFairlyLongFilenamExtraCharacters.xml";

			var result = _driver.ExecFunction("fixLongFilenames", testString);

			Assert.AreEqual(expectation, result, "Files over 248 characters long should have the centre part of the filename removed until they are no more than 248 characters long");
		}
	}
}
