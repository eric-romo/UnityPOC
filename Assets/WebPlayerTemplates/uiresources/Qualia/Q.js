(function(){
		var engine = window.engine;
		if(!engine){
			console.log('Qualia not detected. Exiting..');
			return;
		}
	
		window.Q = {
			Holo: {
				displayModel: function(url){
					var options = {};
					options.__Type = 'DisplayModelOptions';
					options.Url = url;
					console.log('displaying model');
					console.log(window);
					console.log(window.engine);
					window.engine.call('DisplayModel', options);
				}
			}
		};
	})();