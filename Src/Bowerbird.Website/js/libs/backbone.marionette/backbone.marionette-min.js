// Backbone.Marionette v0.7.2
//
// Copyright (C)2011 Derick Bailey, Muted Solutions, LLC
// Distributed Under MIT License
//
// Documentation and Full License Available at:
// http://github.com/derickbailey/backbone.marionette

(function(a,b){if(typeof exports=="object"){var c=require("jquery"),d=require("underscore"),e=require("backbone");module.exports=b(c,d,e)}else typeof define=="function"&&define.amd&&define(["jquery","underscore","backbone"],b)})(this,function(a,b,c){return c.Marionette=function(a,b,c){var d={};d.version="0.7.2",d.View=a.View.extend({getTemplateSelector:function(){var a;return this.options&&this.options.template?a=this.options.template:a=this.template,b.isFunction(a)&&(a=a.call(this)),a},serializeData:function(){var a;return this.model?a=this.model.toJSON():this.collection&&(a={items:this.collection.toJSON()}),a},close:function(){this.beforeClose&&this.beforeClose(),this.unbindAll(),this.remove(),this.onClose&&this.onClose(),this.trigger("close"),this.unbind()}}),d.ItemView=d.View.extend({constructor:function(){var a=e.call(arguments);d.View.prototype.constructor.apply(this,a),b.bindAll(this,"render"),this.initialEvents()},initialEvents:function(){this.collection&&this.bindTo(this.collection,"reset",this.render,this)},render:function(){var a=this,b=c.Deferred(),d=this.serializeData();return this.beforeRender&&this.beforeRender(),this.trigger("item:before:render",a),c.when(d).then(function(d){var e=a.renderHtml(d);c.when(e).then(function(d){a.$el.html(d);var e={};a.onRender&&(e=a.onRender()),c.when(e).then(function(){a.trigger("item:rendered",a),a.trigger("render",a),b.resolve()})})}),b.promise()},renderHtml:function(a){var b=this.getTemplateSelector();return d.Renderer.render(b,a)},close:function(){this.trigger("item:before:close"),d.View.prototype.close.apply(this,arguments),this.trigger("item:closed")}}),d.CollectionView=d.View.extend({constructor:function(){d.View.prototype.constructor.apply(this,arguments),b.bindAll(this,"addItemView","render"),this.initialEvents()},initialEvents:function(){this.collection&&(this.bindTo(this.collection,"add",this.addChildView,this),this.bindTo(this.collection,"remove",this.removeItemView,this),this.bindTo(this.collection,"reset",this.render,this))},addChildView:function(a){var b=this.getItemView();return this.addItemView(a,b)},render:function(){var a=this,b=c.Deferred(),d=[],e=this.getItemView();return this.beforeRender&&this.beforeRender(),this.trigger("collection:before:render",this),this.closeChildren(),this.collection&&this.collection.each(function(b){var c=a.addItemView(b,e);d.push(c)}),b.done(function(){this.onRender&&this.onRender(),this.trigger("collection:rendered",this)}),c.when.apply(this,d).then(function(){b.resolveWith(a)}),b.promise()},getItemView:function(){var a=this.options.itemView||this.itemView;if(!a){var b=new Error("An `itemView` must be specified");throw b.name="NoItemViewError",b}return a},addItemView:function(a,b){var d=this,e=this.buildItemView(a,b);this.storeChild(e),this.trigger("item:added",e);var f=e.render();return c.when(f).then(function(){d.appendHtml(d,e)}),f},buildItemView:function(a,b){var c=new b({model:a});return c},removeItemView:function(a){var b=this.children[a.cid];b&&(b.close(),delete this.children[a.cid]),this.trigger("item:removed",b)},appendHtml:function(a,b){a.$el.append(b.el)},storeChild:function(a){this.children||(this.children={}),this.children[a.model.cid]=a},close:function(){this.trigger("collection:before:close"),this.closeChildren(),d.View.prototype.close.apply(this,arguments),this.trigger("collection:closed")},closeChildren:function(){this.children&&b.each(this.children,function(a){a.close()})}}),d.CompositeView=d.CollectionView.extend({constructor:function(a){d.CollectionView.apply(this,arguments),this.itemView=this.getItemView()},getItemView:function(){return this.itemView||this.constructor},render:function(){var a=this,b=c.Deferred(),d=this.renderModel();return c.when(d).then(function(d){a.$el.html(d),a.trigger("composite:model:rendered"),a.trigger("render");var e=a.renderCollection();c.when(e).then(function(){b.resolve()})}),b.done(function(){a.trigger("composite:rendered")}),b.promise()},renderCollection:function(){var a=d.CollectionView.prototype.render.apply(this,arguments);return a.done(function(){this.trigger("composite:collection:rendered")}),a.promise()},renderModel:function(){var a={};a=this.serializeData();var b=this.getTemplateSelector();return d.Renderer.render(b,a)}}),d.Region=function(a){this.options=a||{},b.extend(this,a);if(!this.el){var c=new Error("An 'el' must be specified");throw c.name="NoElError",c}this.initialize&&this.initialize.apply(this,arguments)},b.extend(d.Region.prototype,a.Events,{show:function(a,b){this.ensureEl(),this.close(),this.open(a,b),this.currentView=a},ensureEl:function(){if(!this.$el||this.$el.length===0)this.$el=this.getEl(this.el)},getEl:function(a){return c(a)},open:function(a,b){var d=this;b=b||"html",c.when(a.render()).then(function(){d.$el[b](a.el),a.onShow&&a.onShow(),d.onShow&&d.onShow(a),a.trigger("show"),d.trigger("view:show",a)})},close:function(){var a=this.currentView;if(!a)return;a.close&&a.close(),this.trigger("view:closed",a),delete this.currentView},attachView:function(a){this.currentView=a}}),d.Layout=d.ItemView.extend({constructor:function(){this.vent=new a.Marionette.EventAggregator,a.Marionette.ItemView.apply(this,arguments),this.regionManagers={}},render:function(){return this.initializeRegions(),a.Marionette.ItemView.prototype.render.call(this,arguments)},close:function(){this.closeRegions(),a.Marionette.ItemView.prototype.close.call(this,arguments)},initializeRegions:function(){var c=this;b.each(this.regions,function(b,d){var e=new a.Marionette.Region({el:b,getEl:function(a){return c.$(a)}});c.regionManagers[d]=e,c[d]=e})},closeRegions:function(){var a=this;b.each(this.regionManagers,function(b,c){b.close(),delete a[c]}),this.regionManagers={}}}),d.AppRouter=a.Router.extend({constructor:function(b){a.Router.prototype.constructor.call(this,b);if(this.appRoutes){var c=this.controller;b&&b.controller&&(c=b.controller),this.processAppRoutes(c,this.appRoutes)}},processAppRoutes:function(a,c){var d,e,f,g,h,i=[],j=this;for(f in c)c.hasOwnProperty(f)&&i.unshift([f,c[f]]);g=i.length;for(h=0;h<g;h++)f=i[h][0],e=i[h][1],d=b.bind(a[e],a),j.route(f,e,d)}}),d.Application=function(a){this.initCallbacks=new d.Callbacks,this.vent=new d.EventAggregator,b.extend(this,a)},b.extend(d.Application.prototype,a.Events,{addInitializer:function(a){this.initCallbacks.add(a)},start:function(a){this.trigger("initialize:before",a),this.initCallbacks.run(this,a),this.trigger("initialize:after",a),this.trigger("start",a)},addRegions:function(a){var b,c,e;for(e in a)a.hasOwnProperty(e)&&(b=a[e],typeof b=="string"?c=new d.Region({el:b}):c=new b,this[e]=c)}}),d.BindTo={bindTo:function(a,b,c,d){d=d||this,a.on(b,c,d),this.bindings||(this.bindings=[]);var e={obj:a,eventName:b,callback:c,context:d};return this.bindings.push(e),e},unbindFrom:function(a){a.obj.off(a.eventName,a.callback);var c=b.indexOf(this.bindings,a);Array.remove(this.bindings,c)},unbindAll:function(){var a=this;b.each(this.bindings,function(b,c){a.unbindFrom(b)}),this.bindings=[]}},d.Callbacks=function(){this.deferred=c.Deferred(),this.promise=this.deferred.promise()},b.extend(d.Callbacks.prototype,{add:function(a){this.promise.done(function(b,c){a.call(b,c)})},run:function(a,b){this.deferred.resolve(a,b)}}),d.EventAggregator=function(a){b.extend(this,a)},b.extend(d.EventAggregator.prototype,a.Events,d.BindTo,{bindTo:function(a,b,c){d.BindTo.bindTo.call(this,this,a,b,c)}}),d.TemplateCache={templates:{},loaders:{},get:function(a){var b=this,d=c.Deferred(),e=this.templates[a];if(e)d.resolve(e);else{var f=this.loaders[a];f?d=f:(this.loaders[a]=d,this.loadTemplate(a,function(c){delete b.loaders[a],b.templates[a]=c,d.resolve(c)}))}return d.promise()},loadTemplate:function(a,b){var d=c(a).html();if(!d||d.length===0){var e="Could not find template: '"+a+"'",f=new Error(e);throw f.name="NoTemplateError",f}d=this.compileTemplate(d),b.call(this,d)},compileTemplate:function(a){return b.template(a)},clear:function(){var a,b=arguments.length;if(b>0)for(a=0;a<b;a++)delete this.templates[arguments[a]];else this.templates={}}},d.Renderer={render:function(a,b){var e=this,f=c.Deferred(),g=d.TemplateCache.get(a);return c.when(g).then(function(a){var c=e.renderTemplate(a,b);f.resolve(c)}),f.promise()},renderTemplate:function(a,b){var c=a(b);return c}};var e=Array.prototype.slice,f=d.View.extend;return d.Region.extend=f,d.Application.extend=f,b.extend(d.View.prototype,d.BindTo),b.extend(d.Application.prototype,d.BindTo),b.extend(d.Region.prototype,d.BindTo),Array.remove||(Array.remove=function(a,b,c){var d=a.slice((c||b)+1||a.length);return a.length=b<0?a.length+b:b,a.push.apply(a,d)}),d}(c,b,window.jQuery||window.Zepto||window.ender),c.Marionette})