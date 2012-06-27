({
    baseUrl: "../src/bowerbird.website/js/bowerbird/",
    paths: {
        jquery: '../libs/jquery/jquery-1.7.2', // jQuery is now AMD compliant
        json2: '../libs/json/json2',
        underscore: '../libs/underscore/underscore',
        backbone: '../libs/backbone/backbone',
        marionette: '../libs/backbone.marionette/backbone.marionette',
        noext: '../libs/require/noext',
        async: '../libs/require/async',
        goog: '../libs/require/goog',
        propertyParser: '../libs/require/propertyparser',
        ich: '../libs/icanhaz/icanhaz',
        jqueryui: '../libs/jqueryui',
        datepicker: '../libs/bootstrap/bootstrap-datepicker',
        date: '../libs/date/date',
        multiselect: '../libs/jquery.multiselect/jquery.multiselect',
        loadimage: '../libs/jquery.fileupload/load-image',
        fileupload: '../libs/jquery.fileupload/jquery.fileupload',
        signalr: '../libs/jquery.signalr/jquery.signalr',
        timeago: '../libs/jquery.timeago/jquery.timeago',
        log: '../libs/log/log',
        'bootstrap-data': 'empty:',
        '/templates': 'empty:',
         hubs: 'hubs'
    },
//    shim: {
//        //'/signalr/hubs': ['signalr', 'jquery'] // Load non-AMD signalr hubs script
//        hubs: ['signalr', 'jquery'] // Load non-AMD signalr hubs script
//    },
    name: "../main",
    include: [
        'app',
        'ich',
        'log',
        'jquery',
        'json2',
        'underscore',
        'backbone',
        'marionette',
        'controllers/usercontroller',
        'controllers/activitycontroller',
        'controllers/groupusercontroller',
        'controllers/homecontroller',
        'controllers/observationcontroller',
        'controllers/organisationcontroller',
        'controllers/postcontroller',
        'controllers/projectcontroller',
        'controllers/referencespeciescontroller',
        'controllers/speciescontroller',
        'controllers/teamcontroller',
        'controllers/accountController',
        'views/headerview',
        'views/footerview',
        'views/sidebarlayoutview',
        'views/notificationscompositeview',
        'views/chatcompositeview',
        'hubs'
    ],
    out: "../main-min.js",
    optimize: "uglify",
    uglify: {
        toplevel: true,
        ascii_only: true,
        beautify: false,
        max_line_length: 1000
    },
    findNestedDependencies: true
})