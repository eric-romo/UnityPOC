"use strict";var TweenMax=window.TweenMax,statusText=$("#status-text");TweenMax.fromTo(statusText,.5,{opacity:0},{opacity:1});var coreText;coreText="Loading";var numDots=0,dotInterval=setInterval(function(){statusText.text(coreText+new Array(numDots+1).join(".")),numDots++,numDots%=4},200);setTimeout(function(){statusText.css("left",statusText.position().left-20),coreText="Logging in",statusText.text(coreText+new Array(numDots+1).join("."))},2e3),setTimeout(function(){TweenMax.fromTo(statusText,.5,{opacity:1},{opacity:0}),statusText.text(coreText),clearInterval(dotInterval)},6e3),setTimeout(function(){TweenMax.to($("#loading-animation"),.5,{opacity:0}),TweenMax.to($(".diamond"),.5,{opacity:0})},8e3);