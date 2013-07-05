// -----------------------------------------------------------------------
// <copyright file="JurassicJavascriptDriver.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MarabouStork.Scripting.Javascript.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using CuttingEdge.Conditions;
	using System.Diagnostics;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	public class MessageEventArgs: EventArgs
	{
		public MessageEventArgs() { }

		public MessageEventArgs(string message) : this()
		{
			this.Message = message;
		}

		public string Message { get; set; }
	}

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class JurassicJavascriptDriver: IJavascriptSDriver
	{
		private readonly Jurassic.ScriptEngine _engine;
		private readonly string _scriptLocation;
		private List<string> _dependencies;

		public event EventHandler<MessageEventArgs> OnMessageRecieved;

		public JurassicJavascriptDriver(string scriptLocation)
		{
			Condition.Requires(scriptLocation).IsNotNullOrEmpty();

			this._scriptLocation = scriptLocation;
			this._engine = new Jurassic.ScriptEngine();

			// Used to load dependent libraries
			_engine.SetGlobalFunction("resolveDependency", new Action<string>(
				(file) => { 
					this.ResolveDependency(file); 
				}));
			
			// Used to recieve messages from javascript
			_engine.SetGlobalFunction("console_log", new Action<string>(
				(message) => { 
					Trace.WriteLine(message);
					if (this.OnMessageRecieved != null) this.OnMessageRecieved(this, new MessageEventArgs(message));
				}));
			
			var phantomMock = System.IO.File.ReadAllText(@"C:\_src\MarabouStork.kiln\trunk\Misc\ConsoleApplication2\TestProject1\bin\Debug\phantom.js");
			_engine.Evaluate(phantomMock);

			this._dependencies = new List<string>();
		}

		private void ResolveDependency(string name)
		{
			Trace.WriteLine(string.Format("Resolving dependency \"{0}\"", name));

			this.LoadLibrary(name);
		}

		public void LoadLibrary(string name)
		{
			if (!this._dependencies.Any(x => x.ToLower().Equals(name.ToLower())))
			{
				this._dependencies.Add(name);

				var fileContent = System.IO.File.ReadAllText(System.IO.Path.Combine(this._scriptLocation, name));
				if (!string.IsNullOrEmpty(fileContent))
				{
					this._engine.Evaluate(fileContent);
				}
				else
				{
					throw new ArgumentException(string.Format("Failed to load content for script \"{0}\"", name));
				}
			}
		}

		public string ExecFunction(string name, params string[] parameters)
		{
			var js = string.Format("{0}({1})", name, string.Join(", ", parameters.Select(x => string.Format("\"{0}\"", x.Replace("\\", "\\\\")))));

			Trace.WriteLine("Executing JS_UnitTest for function call: " + js);
			return this._engine.Evaluate(js) as string;
		}
	}
}
