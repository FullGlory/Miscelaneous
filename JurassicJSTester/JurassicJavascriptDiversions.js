var phantom = {
	exit: function () {
		// exit
	},

	injectJs: function (fileName) {
		resolveDependency(fileName);
	}
}

var console = {
	log: function (str) {
		console_log(str);
	}
}

var symphony = {
	debug: function (str) {
		console_log(str);
	}
}
