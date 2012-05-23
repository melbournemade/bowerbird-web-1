(function(n,t){"use strict";var f,e,i,r,u;if(typeof n!="function")throw"SignalR: jQuery not found. Please ensure jQuery is referenced before the SignalR.js file.";if(!t.JSON)throw"SignalR: No JSON parser found. Please ensure json2.js is referenced before the SignalR.js file if you need to support clients without native JSON parsing support, e.g. IE<8.";i={onStart:"onStart",onStarting:"onStarting",onSending:"onSending",onReceived:"onReceived",onError:"onError",onReconnect:"onReconnect",onDisconnect:"onDisconnect"},r=function(n,i){if(i===!1)return;var r;if(typeof t.console=="undefined")return;r="["+(new Date).toTimeString()+"] SignalR: "+n,t.console.debug?t.console.debug(r):t.console.log&&t.console.log(r)},f=function(n,t,i){return new f.fn.init(n,t,i)},f.fn=f.prototype={init:function(n,t,i){this.url=n,this.qs=t,typeof i=="boolean"&&(this.logging=i)},logging:!1,reconnectDelay:2e3,start:function(r,u){var e=this,o={transport:"auto"},h,s=n.Deferred();return e.transport?(s.resolve(e),s):(n.type(r)==="function"?u=r:n.type(r)==="object"&&(n.extend(o,r),n.type(o.callback)==="function"&&(u=o.callback)),n(e).bind(i.onStart,function(){n.type(u)==="function"&&u.call(e),s.resolve(e)}),h=function(t,r){r=r||0;if(r>=t.length){e.transport||s.reject("SignalR: No transport could be initialized successfully. Try specifying a different transport or none at all for auto initialization.");return}var u=t[r],o=n.type(u)==="object"?u:f.transports[u];o.start(e,function(){e.transport=o,n(e).trigger(i.onStart)},function(){h(t,r+1)})},t.setTimeout(function(){n.ajax(e.url+"/negotiate",{global:!1,type:"POST",data:{},dataType:"json",error:function(t){n(e).trigger(i.onError,[t]),s.reject("SignalR: Error during negotiation request: "+t)},success:function(t){e.appRelativeUrl=t.Url,e.id=t.ConnectionId,e.webSocketServerUrl=t.WebSocketServerUrl;if(!t.ProtocolVersion||t.ProtocolVersion!=="1.0"){n(e).trigger(i.onError,"SignalR: Incompatible protocol version."),s.reject("SignalR: Incompatible protocol version.");return}n(e).trigger(i.onStarting);var u=[],r=[];n.each(f.transports,function(n){if(n==="webSockets"&&!t.TryWebSockets)return!0;r.push(n)}),n.isArray(o.transport)?n.each(o.transport,function(){var t=this;n.type(t)!=="object"&&(n.type(t)!=="string"||n.inArray(""+t,r)<0)||u.push(n.type(t)==="string"?""+t:t)}):n.type(o.transport)!=="object"&&n.inArray(o.transport,r)<0?u=r:u.push(o.transport),h(u)}})},0),s)},starting:function(t){var r=this,u=n(r);return u.bind(i.onStarting,function(){t.call(r),u.unbind(i.onStarting)}),r},send:function(n){var t=this;if(!t.transport)throw"SignalR: Connection must be started before data can be sent. Call .start() before .send()";return t.transport.send(t,n),t},sending:function(t){var r=this;return n(r).bind(i.onSending,function(){t.call(r)}),r},received:function(t){var r=this;return n(r).bind(i.onReceived,function(n,i){t.call(r,i)}),r},error:function(t){var r=this;return n(r).bind(i.onError,function(n,i){t.call(r,i)}),r},disconnected:function(t){var r=this;return n(r).bind(i.onDisconnect,function(){t.call(r)}),r},reconnected:function(t){var r=this;return n(r).bind(i.onReconnect,function(){t.call(r)}),r},stop:function(){var t=this;return t.transport&&(t.transport.stop(t),t.transport=null),delete t.messageId,delete t.groups,n(t).trigger(i.onDisconnect),t},log:r},f.fn.init.prototype=f.fn,u={addQs:function(t,i){return i.qs?typeof i.qs=="object"?t+"&"+n.param(i.qs):typeof i.qs=="string"?t+"&"+i.qs:t+"&"+escape(i.qs.toString()):t},getUrl:function(n,i,r){var u=n.url,f="transport="+i+"&connectionId="+t.escape(n.id);return n.data&&(f+="&connectionData="+t.escape(n.data)),r?(n.messageId&&(f+="&messageId="+n.messageId),n.groups&&(f+="&groups="+t.escape(JSON.stringify(n.groups)))):u=u+"/connect",u+="?"+f,u=this.addQs(u,n)},ajaxSend:function(r,u){var f=r.url+"/send?transport="+r.transport.name+"&connectionId="+t.escape(r.id);f=this.addQs(f,r),n.ajax(f,{global:!1,type:"POST",dataType:"json",data:{data:u},success:function(t){t&&n(r).trigger(i.onReceived,[t])},error:function(t,u){if(u==="abort")return;n(r).trigger(i.onError,[t])}})},processMessages:function(t,u){var f=n(t);if(u){if(u.Disconnect){r("Disconnect command received from server",t.logging),t.stop(),f.trigger(i.onDisconnect);return}u.Messages&&n.each(u.Messages,function(){try{f.trigger(i.onReceived,[this])}catch(u){r("Error raising received "+u,t.logging),n(t).trigger(i.onError,[u])}}),t.messageId=u.MessageId,t.groups=u.TransportData.Groups}},foreverFrame:{count:0,connections:{}}},f.transports={webSockets:{name:"webSockets",send:function(n,t){n.socket.send(t)},start:function(u,f,e){var o,h=!1,s;t.MozWebSocket&&(t.WebSocket=t.MozWebSocket);if(!t.WebSocket){e();return}u.socket||(u.webSocketServerUrl?o=u.webSocketServerUrl:(s=document.location.protocol==="https:"?"wss://":"ws://",o=s+document.location.host+u.appRelativeUrl),n(u).trigger(i.onSending),o+=u.data?"?connectionData="+u.data+"&transport=webSockets&connectionId="+u.id:"?transport=webSockets&connectionId="+u.id,u.socket=new t.WebSocket(o),u.socket.onopen=function(){h=!0,f&&f()},u.socket.onclose=function(t){h?typeof t.wasClean!="undefined"&&t.wasClean===!1&&n(u).trigger(i.onError):e&&e(),u.socket=null},u.socket.onmessage=function(f){var e=t.JSON.parse(f.data),o;e&&(o=n(u),e.Messages?n.each(e.Messages,function(){try{o.trigger(i.onReceived,[this])}catch(n){r("Error raising received "+n,u.logging)}}):o.trigger(i.onReceived,[e]))})},stop:function(n){n.socket!==null&&(n.socket.close(),n.socket=null)}},serverSentEvents:{name:"serverSentEvents",timeOut:3e3,start:function(f,e,o){var s=this,l=!1,c=n(f),h=!e,v,a;f.eventSource&&f.stop();if(!t.EventSource){o&&o();return}c.trigger(i.onSending),v=u.getUrl(f,this.name,h);try{f.eventSource=new t.EventSource(v)}catch(y){r("EventSource failed trying to connect with error "+y.Message,f.logging),o?o():(c.trigger(i.onError,[y]),h&&(r("EventSource reconnecting",f.logging),s.reconnect(f)));return}a=t.setTimeout(function(){l===!1&&(r("EventSource timed out trying to connect",f.logging),o&&o(),h?(r("EventSource reconnecting",f.logging),s.reconnect(f)):s.stop(f))},s.timeOut),f.eventSource.addEventListener("open",function(){r("EventSource connected",f.logging),a&&t.clearTimeout(a),l===!1&&(l=!0,e&&e(),h&&c.trigger(i.onReconnect))},!1),f.eventSource.addEventListener("message",function(n){if(n.data==="initialized")return;u.processMessages(f,t.JSON.parse(n.data))},!1),f.eventSource.addEventListener("error",function(n){if(!l){o&&o();return}r("EventSource readyState: "+f.eventSource.readyState,f.logging),n.eventPhase===t.EventSource.CLOSED?f.eventSource.readyState===t.EventSource.CONNECTING?(r("EventSource reconnecting due to the server connection ending",f.logging),s.reconnect(f)):(r("EventSource closed",f.logging),s.stop(f)):(r("EventSource error",f.logging),c.trigger(i.onError))},!1)},reconnect:function(n){var i=this;t.setTimeout(function(){i.stop(n),i.start(n)},n.reconnectDelay)},send:function(n,t){u.ajaxSend(n,t)},stop:function(n){n&&n.eventSource&&(n.eventSource.close(),n.eventSource=null,delete n.eventSource)}},foreverFrame:{name:"foreverFrame",timeOut:3e3,start:function(f,e,o){var h=this,l=u.foreverFrame.count+=1,c,a,s=n("<iframe data-signalr-connection-id='"+f.id+"' style='position:absolute;width:0;height:0;visibility:hidden;'></iframe>");if(t.EventSource){o&&o();return}n(f).trigger(i.onSending),c=u.getUrl(f,this.name),c+="&frameId="+l,s.prop("src",c),u.foreverFrame.connections[l]=f,s.bind("readystatechange",function(){n.inArray(this.readyState,["loaded","complete"])<0||(r("Forever frame iframe readyState changed to "+this.readyState+", reconnecting",f.logging),h.reconnect(f))}),f.frame=s[0],f.frameId=l,e&&(f.onSuccess=e),n("body").append(s),a=t.setTimeout(function(){f.onSuccess&&(h.stop(f),o&&o())},h.timeOut)},reconnect:function(n){var i=this;t.setTimeout(function(){var r=n.frame,t=u.getUrl(n,i.name,!0)+"&frameId="+n.frameId;r.src=t},n.reconnectDelay)},send:function(n,t){u.ajaxSend(n,t)},receive:u.processMessages,stop:function(t){t.frame&&(t.frame.stop?t.frame.stop():t.frame.document&&t.frame.document.execCommand&&t.frame.document.execCommand("Stop"),n(t.frame).remove(),delete u.foreverFrame.connections[t.frameId],t.frame=null,t.frameId=null,delete t.frame,delete t.frameId)},getConnection:function(n){return u.foreverFrame.connections[n]},started:function(t){t.onSuccess?(t.onSuccess(),t.onSuccess=null,delete t.onSuccess):n(t).trigger(i.onReconnect)}},longPolling:{name:"longPolling",reconnectDelay:3e3,start:function(r,f){var o=this;r.pollXhr&&r.stop(),r.messageId=null,t.setTimeout(function(){(function e(f,s){n(f).trigger(i.onSending);var a=f.messageId,l=a===null,v=u.getUrl(f,o.name,!l),c=null,h=!1;f.pollXhr=n.ajax(v,{global:!1,type:"GET",dataType:"json",success:function(r){var c=0,o=!1;s===!0&&h===!1&&(n(f).trigger(i.onReconnect),h=!0),u.processMessages(f,r),r&&n.type(r.TransportData.LongPollDelay)==="number"&&(c=r.TransportData.LongPollDelay),r&&r.TimedOut&&(o=r.TimedOut),c>0?t.setTimeout(function(){e(f,o)},c):e(f,o)},error:function(u,o){if(o==="abort")return;c&&clearTimeout(c),n(f).trigger(i.onError,[u]),t.setTimeout(function(){e(f,!0)},r.reconnectDelay)}}),s===!0&&(c=t.setTimeout(function(){h===!1&&(n(f).trigger(i.onReconnect),h=!0)},o.reconnectDelay))})(r),t.setTimeout(f,150)},250)},send:function(n,t){u.ajaxSend(n,t)},stop:function(n){n.pollXhr&&(n.pollXhr.abort(),n.pollXhr=null,delete n.pollXhr)}}},f.noConflict=function(){return n.connection===f&&(n.connection=e),f},n.connection&&(e=n.connection),n.connection=n.signalR=f})(window.jQuery,window)