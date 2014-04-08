using UnityEngine;
using System.Collections;

public class NetflixController : SiteController {
	
	// Use this for initialization
	public void Start () {
		base.Start();
		Prefix = "http://www.netflix.com/";
		
		Scripts.NavigateTo = @"
		(function(){
		    var chromeConsole = window.console;
		    
		    document.addEventListener('keyup', function (e) {
		      if(e.keyCode == 27){
		        restoreConsole();
		      }
		    });
		    
		    function restoreConsole(){
		        chromeConsole.log('Restoring Console...');
		        delete window.console;
		    	//Object.defineProperty(window, 'console', chromeConsole);
		    }
		    
		    function protectConsole(){
		    	Object.defineProperty(window, 'console', {value: window.console, configurable : false});
		    }
    
	    protectConsole();
		})();
		console.log(document.getElementsByTagName('script')[4]);
		console.log('head: ' + document.getElementsByTagName('head'));
		console.info(document.getElementsByTagName('body'));
		
		
		for(var i = 0, max = 10; i < max; i++){
			console.log(document.getElementsByTagName('script')[i]);
		}
		
		//document.createElement = function(contents){console.log('element creation intercepted: ' + contents)};
		";
		
		Scripts.Head = @"
		console.log('head script');
		console.log('startup script: ' + document.getElementsByTagName('script')[4]);
		
		for(var i = 0, max = 10; i < max; i++){
			console.log(document.getElementsByTagName('script')[i]);
		}
		
		console.log(document.getElementsByTagName('body')[0].children.length);
		";
	}
	
	// Update is called once per frame
	void Update () {
	}
}
