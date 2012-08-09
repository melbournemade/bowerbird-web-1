﻿/// <reference path="../../libs/log.js" />
/// <reference path="../../libs/require/require.js" />
/// <reference path="../../libs/jquery/jquery-1.7.2.js" />
/// <reference path="../../libs/underscore/underscore.js" />
/// <reference path="../../libs/backbone/backbone.js" />
/// <reference path="../../libs/backbone.marionette/backbone.marionette.js" />

// ObservationDetailsView
// ----------------------

define(['jquery', 'underscore', 'backbone', 'app'],
function ($, _, Backbone, app) 
{
    var PostDetailsView = Backbone.Marionette.ItemView.extend({
        className: 'post-details',

        template: 'PostDetails',

        serializeData: function () {
            var json = { Model: { Post: this.model.toJSON() } };
            //json.Model.ShowThumbnails = this.model.get('Media').length > 1 ? true : false;
            return json;
        },

        onShow: function () {
            this._showDetails();
        },

        showBootstrappedDetails: function () {
            this._showDetails();
        },

        _showDetails: function () {
            
        }
    });

    return PostDetailsView;

});